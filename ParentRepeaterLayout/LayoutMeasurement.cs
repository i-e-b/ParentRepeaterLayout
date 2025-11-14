using System.Text;

namespace ParentRepeaterLayout;

/// <summary>
/// A measurement plus its unit of measure
/// </summary>
public class LayoutMeasurement: IEquatable<LayoutMeasurement>, IComparable<LayoutMeasurement>
{
    /// <summary>
    /// A zero-sized measurement.
    /// This can be an actual zero-sized element, or measurement should be determined by content
    /// </summary>
    public static LayoutMeasurement Zero => new() { Unit = UnitOfMeasure.Invalid, Value = 0.0 };

    /// <summary>
    /// Value of the measurement
    /// </summary>
    public double Value { get; init; }

    /// <summary>
    /// Unit of the measurement
    /// </summary>
    public UnitOfMeasure Unit { get; init; }

    /// <summary>
    /// Parse a string as a measurement.
    /// Values without a unit of measure are assumed to be pixels.
    /// Failure to parse will result in the value <see cref="Zero"/>
    /// </summary>
    public static LayoutMeasurement Parse(string s)
    {
        int start = 0;
        int end   = -1;

        if (s == "_") return Zero;

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
                end = i+1;
                clean.Append(c);
            }
            else if (c is '.' or ',')
            {
                end = i+1;
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

        if (end >= s.Length) return new LayoutMeasurement { Value = value, Unit = UnitOfMeasure.Pixel };

        var unitStr = s[end..].Trim().ToLowerInvariant();

        var unit = unitStr switch
        {
            "em" => UnitOfMeasure.Em,
            "%" => UnitOfMeasure.Percent,
            _ => UnitOfMeasure.Pixel
        };

        return new LayoutMeasurement { Value = value, Unit = unit };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Unit switch
        {
            UnitOfMeasure.Invalid => "0",
            UnitOfMeasure.Pixel => $"{Value}px",
            UnitOfMeasure.Em => $"{Value}em",
            UnitOfMeasure.Percent => $"{Value}%",
            _ => "<invalid>"
        };
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

    /// <inheritdoc />
    public bool Equals(LayoutMeasurement? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (Value == 0 && other.Value == 0) return true; // all zeros are created equal.

        return Value.Equals(other.Value) && Unit == other.Unit;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((LayoutMeasurement)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, (int)Unit);
    }

    /// <inheritdoc />
    public int CompareTo(LayoutMeasurement? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var valueComparison = Value.CompareTo(other.Value);
        if (valueComparison != 0) return valueComparison;
        return Unit.CompareTo(other.Unit);
    }
}