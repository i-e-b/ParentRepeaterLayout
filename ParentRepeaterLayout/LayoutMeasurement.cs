using System.Text;

namespace ParentRepeaterLayout;

/// <summary>
/// A measurement plus its unit of measure
/// </summary>
public class LayoutMeasurement
{
    /// <summary>
    /// A zero-sized measurement.
    /// This can be an actual zero-sized element, or measurement should be determined by content
    /// </summary>
    public static LayoutMeasurement Zero => new() { Unit = UnitOfMeasure.Invalid, Value = 0.0 };

    /// <summary>
    /// Value of the measurement
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Unit of the measurement
    /// </summary>
    public UnitOfMeasure Unit { get; set; }

    /// <summary>
    /// Parse a string as a measurement.
    /// Values without a unit of measure are assumed to be pixels.
    /// Failure to parse will result in the value <see cref="Zero"/>
    /// </summary>
    public static LayoutMeasurement Parse(string s)
    {
        int start = 0;
        int end   = -1;

        var clean = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (char.IsWhiteSpace(c))
            {
                if (start > end) start = i;
                else
                {
                    end = i;
                    break;
                }
            }
            else if (char.IsNumber(c))
            {
                end = i;
                clean.Append(c);
            }
            else if (c is '.' or ',')
            {
                end = i;
                clean.Append('.');
            }
            else
            {
                end = i;
                break;
            }
        }

        if (start >= end) return Zero;

        if (!double.TryParse(clean.ToString(), out var value))
        {
            return Zero;
        }

        var result = new LayoutMeasurement { Value = value, Unit = UnitOfMeasure.Pixel };

        if (end >= s.Length) return result;

        var unit = s[end..].Trim().ToLowerInvariant();

        result.Unit = unit switch
        {
            "em" => UnitOfMeasure.Em,
            "%" => UnitOfMeasure.Percent,
            "px" => UnitOfMeasure.Pixel,
            _ => result.Unit
        };

        return result;
    }


    /// <summary>
    /// Parse a string to a <see cref="LayoutMeasurement"/>
    /// </summary>
    public static implicit operator LayoutMeasurement(string toParse)
    {
        return Parse(toParse);
    }

    /// <summary>
    /// Convert an integer to a number of pixels
    /// </summary>
    public static implicit operator LayoutMeasurement(int pixels)
    {
        return new LayoutMeasurement
        {
            Value = pixels,
            Unit = UnitOfMeasure.Pixel
        };
    }

    /// <summary>
    /// Convert an double to a number of pixels
    /// </summary>
    public static implicit operator LayoutMeasurement(double pixels)
    {
        return new LayoutMeasurement
        {
            Value = pixels,
            Unit = UnitOfMeasure.Pixel
        };
    }
}