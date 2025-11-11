using NUnit.Framework;
using ParentRepeaterLayout;

namespace UnitTests;

[TestFixture]
public class BasicOutputTests
{
    [Test]
    public void very_basic_output()
    {
        Assert.Inconclusive("not tested yet");
    }


    [Test]
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