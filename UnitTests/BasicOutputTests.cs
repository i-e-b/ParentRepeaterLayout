using NUnit.Framework;
using ParentRepeaterLayout;
using SkiaSharp;

namespace UnitTests;

[TestFixture]
public class BasicOutputTests
{
    [Test(Description = "A copy of the original sketch")]
    public void very_basic_output()
    {
        var sampleRules = new[]
        {
            new LayoutRule (name: "Root", width: 600, height: 400),
            new LayoutRule (name: "Column", width: "31%", height: "100%", next: Joiner.TopRight.Offset("3.3%", 0), prev: Joiner.TopLeft),
            new LayoutRule (name: "Line", next: Joiner.BottomLeft.Offset(0, "0.5em")),
            new LayoutRule (name: "Word", next: Joiner.TopRight.Offset("0.5em", 0)),
            new LayoutRule (name: "Glyph", glue: true)
        };

        var emHeight = FontGuess.MeasureStrings(10, "M");

        var tokenSet = SampleData.ExampleInput.Select(c =>
        {
            if (char.IsWhiteSpace(c))
            {
                var brk = (c is '\r' or '\n')
                    ? 2  // Paragraph starts a new line (break: Glyph->Word->Line)
                    : 1; // Add a space (break: Glyph->Word)

                return new LayoutToken { Break = brk };
            }

            var charWidth = FontGuess.MeasureCharacter(10, c);
            return new LayoutToken { Break = 0, Content = c, Width = charWidth, Height = emHeight };
        }).ToList();

        var subject = new LayoutCore(sampleRules);

        var layout = subject.Layout(tokenSet);


        Assert.That(layout, Is.Not.Null);
    }

    [Test(Description = "Check that we can generate graphics from tests")]
    public void skia_test()
    {
        var bmp = new SKBitmap(128, 128, SKColorType.Rgba8888, SKAlphaType.Opaque);

        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.DarkGoldenrod);

        var random = Random.Shared;
        var paint  = new SKPaint { Color = SKColors.Cornsilk };

        for (int i = 0; i < 10_000; i++)
        {
            SKPoint point = new(random.Next(bmp.Width), random.Next(bmp.Height));
            canvas.DrawPoint(point, paint);
        }

        using var fs = new SKFileWStream("points.png");
        bmp.Encode(fs, SKEncodedImageFormat.Png, 95);
        fs.Flush();

        Assert.That(File.Exists("points.png"), Is.True);
    }


    [Test(Description = "Check parsing of measures")]
    [TestCase("1.5%", 1.5, UnitOfMeasure.Percent)]
    [TestCase("1.5", 1.5, UnitOfMeasure.Pixel)]
    [TestCase("1.5px", 1.5, UnitOfMeasure.Pixel)]
    [TestCase("1,5em", 1.5, UnitOfMeasure.Em)] // Allow comma or point as decimal place (no group separators)
    [TestCase(".5em", 0.5, UnitOfMeasure.Em)] // Allow lack of leading zero
    [TestCase(".5%", 0.5, UnitOfMeasure.Percent)]
    [TestCase("0000.5%", 0.5, UnitOfMeasure.Percent)]
    [TestCase("1000000.00000001", 1000000.00000001, UnitOfMeasure.Pixel)] // allow a large range
    [TestCase(" 1.5\t%  ", 1.5, UnitOfMeasure.Percent)]
    [TestCase("\t1.5 em\r\n", 1.5, UnitOfMeasure.Em)]
    [TestCase("\t1.5   px\r\n", 1.5, UnitOfMeasure.Pixel)]
    public void layout_measurement_parsing(string input, double expected, UnitOfMeasure unit)
    {
        var result = LayoutMeasurement.Parse(input);

        Assert.That(result.Value, Is.EqualTo(expected).Within(0.0001));
        Assert.That(result.Unit, Is.EqualTo(unit));
    }
}