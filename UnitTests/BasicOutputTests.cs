using NUnit.Framework;
using ParentRepeaterLayout;
using SkiaSharp;

namespace UnitTests;

[TestFixture]
public class BasicOutputTests
{
    [Test(Description = "Turn a text description into layout rule objects")]
    public void parsing_entire_layout()
    {
        var subject = LayoutRule.ParseRules(@"
  name=Root, width=600px, height=400px
name=Column, width=31%,   height=100%, next=TR 3.3% 0,  prev=TL
  name=Line,                           next=BL 0 0.5em
  name=Word,                           next=TR 0.5em 0
 name=Glyph, glue=true
");

        Assert.That(subject.Length, Is.EqualTo(5), "number of rules");

        Assert.That(subject[0].Name, Is.EqualTo("Root"));
        Assert.That(subject[0].Width, Is.EqualTo(LayoutMeasurement.Parse("600px")));
        Assert.That(subject[0].Height, Is.EqualTo(LayoutMeasurement.Parse("400px")));
        Assert.That(subject[0].Next, Is.EqualTo(Joiner.TopRight));
        Assert.That(subject[0].Prev, Is.EqualTo(Joiner.TopLeft));
        Assert.That(subject[0].Glue, Is.False);

        Assert.That(subject[1].Name, Is.EqualTo("Column"));
        Assert.That(subject[1].Width, Is.EqualTo(LayoutMeasurement.Parse("31%")));
        Assert.That(subject[1].Height, Is.EqualTo(LayoutMeasurement.Parse("100%")));
        Assert.That(subject[1].Next, Is.EqualTo(Joiner.TopRight.Offset("3.3%", 0)));
        Assert.That(subject[1].Prev, Is.EqualTo(Joiner.TopLeft));
        Assert.That(subject[1].Glue, Is.False);

        Assert.That(subject[2].Name, Is.EqualTo("Line"));
        Assert.That(subject[2].Width, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[2].Height, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[2].Next, Is.EqualTo(Joiner.BottomLeft.Offset(0, "0.5em")));
        Assert.That(subject[2].Prev, Is.EqualTo(Joiner.TopLeft));
        Assert.That(subject[2].Glue, Is.False);

        Assert.That(subject[3].Name, Is.EqualTo("Word"));
        Assert.That(subject[3].Width, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[3].Height, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[3].Next, Is.EqualTo(Joiner.TopRight.Offset("0.5em", 0)));
        Assert.That(subject[3].Prev, Is.EqualTo(Joiner.TopLeft));
        Assert.That(subject[3].Glue, Is.False);

        Assert.That(subject[4].Name, Is.EqualTo("Glyph"));
        Assert.That(subject[4].Width, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[4].Height, Is.EqualTo(LayoutMeasurement.Zero));
        Assert.That(subject[4].Next, Is.EqualTo(Joiner.TopRight));
        Assert.That(subject[4].Prev, Is.EqualTo(Joiner.TopLeft));
        Assert.That(subject[4].Glue, Is.True);
    }

    [Test(Description = "A copy of the original sketch")]
    public void very_basic_output()
    {
        var sampleRules = LayoutRule.ParseRules(
            """

            name=Root,   width=600px, height=400px
            name=Column, width=31%,   height=100%, next=TR 3.3% 0, prev=TL
            name=Line,                             next=BL 0 0.5em
            name=Word,                             next=TR 0.5em 0
            name=Glyph,  glue=true

            """);

        var emHeight = FontGuess.MeasureStrings(10, "M");

        var tokenSet = SampleData.ExampleInput.Select(c =>
        {
            if (char.IsWhiteSpace(c))
            {
                var brk = (c is '\r' or '\n')
                    ? 2 // Paragraph starts a new line (break: Glyph->Word->Line)
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

    [Test(Description = "Parsing joiner specs")]
    public void joiner_parsing()
    {
        Assert.That(Joiner.Parse("TL"), Is.EqualTo(Joiner.TopLeft), "TL");
        Assert.That(Joiner.Parse("ML"), Is.EqualTo(Joiner.MiddleLeft), "ML");
        Assert.That(Joiner.Parse("BL"), Is.EqualTo(Joiner.BottomLeft), "BL");

        Assert.That(Joiner.Parse("TC"), Is.EqualTo(Joiner.TopCentre), "TC");
        Assert.That(Joiner.Parse("MC"), Is.EqualTo(Joiner.MiddleCentre), "MC");
        Assert.That(Joiner.Parse("BC"), Is.EqualTo(Joiner.BottomCentre), "BC");

        Assert.That(Joiner.Parse("TR"), Is.EqualTo(Joiner.TopRight), "TR");
        Assert.That(Joiner.Parse("MR"), Is.EqualTo(Joiner.MiddleRight), "MR");
        Assert.That(Joiner.Parse("BR"), Is.EqualTo(Joiner.BottomRight), "BR");

        Assert.That(Joiner.Parse("TL 0 0"), Is.EqualTo(Joiner.TopLeft), "TL 0 0");

        Assert.That(Joiner.Parse("ML 1 2em"), Is.EqualTo(Joiner.MiddleLeft.Offset("1px", "2em")), "ML 1 2em");
        Assert.That(Joiner.Parse("BL _ 3%"), Is.EqualTo(Joiner.BottomLeft.Offset(0, "3%")), "BL");

        Assert.That(Joiner.Parse("TC 1 _"), Is.EqualTo(Joiner.TopCentre.Offset(1, 0)), "TC 1 _");
        Assert.That(Joiner.Parse("MC 2em _"), Is.EqualTo(Joiner.MiddleCentre.Offset("2em", 0)), "MC 2em _");
        Assert.That(Joiner.Parse("BC 3% _"), Is.EqualTo(Joiner.BottomCentre.Offset("3%", 0)), "BC 3% _");

        Assert.That(Joiner.Parse("0 0 1 1"), Is.EqualTo(Joiner.TopLeft.Offset(1, 1)), "0 0 1 1");
        Assert.That(Joiner.Parse("0.75 0.25 1em 3%"), Is.EqualTo(new Joiner
        {
            XPos = 0.75,
            YPos = 0.25,
            XOffset = "1em",
            YOffset = "3%"
        }), "0.75 0.25 1em 3%");
        Assert.That(Joiner.Parse(".75 .25 _ 1px"), Is.EqualTo(new Joiner
        {
            XPos = 0.75,
            YPos = 0.25,
            XOffset = 0,
            YOffset = "1px"
        }), ".75 .25 _ 1px");
    }

    [Test(Description = "Check parsing of measures")]
    [TestCase("_", 0.0, UnitOfMeasure.Invalid)] // to allow 'placeholder' underscores
    [TestCase("1.5%", 1.5, UnitOfMeasure.Percent)]
    [TestCase("1.5", 1.5, UnitOfMeasure.Pixel)]
    [TestCase("1", 1, UnitOfMeasure.Pixel)]
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