
namespace ParentRepeaterLayout;

/// <summary>
/// Joining position of sibling elements.
/// The next sibling in a chain is placed so that its <see cref="LayoutRule.Prev"/> is at the same location
/// as the previous sibling's <see cref="LayoutRule.Next"/>.
/// </summary>
public class Joiner: IEquatable<Joiner>
{
    #region Preset values
    /// <summary>
    /// Joining point is at top-left of element
    /// </summary>
    public static Joiner TopLeft => new() { XPos = 0.0, YPos = 0.0 };

    /// <summary>
    /// Joining point is at left of element, at mid-line
    /// </summary>
    public static Joiner MiddleLeft => new() { XPos = 0.0, YPos = 0.5 };

    /// <summary>
    /// Joining point is at bottom-left of element
    /// </summary>
    public static Joiner BottomLeft => new() { XPos = 0.0, YPos = 1.0 };

    /// <summary>
    /// Joining point is at top-right of element
    /// </summary>
    public static Joiner TopRight => new() { XPos = 1.0, YPos = 0.0 };

    /// <summary>
    /// Joining point is at right of element, at mid-line
    /// </summary>
    public static Joiner MiddleRight => new() { XPos = 1.0, YPos = 0.5 };

    /// <summary>
    /// Joining point is at bottom-right of element
    /// </summary>
    public static Joiner BottomRight => new() { XPos = 1.0, YPos = 1.0 };


    /// <summary>
    /// Joining point is at top of element, at centre
    /// </summary>
    public static Joiner TopCentre => new() { XPos = 0.5, YPos = 0.0 };

    /// <summary>
    /// Joining point is at bottom of element, at centre
    /// </summary>
    public static Joiner BottomCentre => new() { XPos = 0.5, YPos = 1.0 };

    /// <summary>
    /// Joining point is at dead-centre of element
    /// </summary>
    public static Joiner MiddleCentre => new() { XPos = 0.5, YPos = 0.5 };

    #endregion Preset values

    /// <summary>
    /// X-coordinate of joiner, relative to element left, as a fraction of width.
    /// Value should normally be <c>0.0</c> to <c>1.0</c> inclusive.
    /// </summary>
    public double XPos { get; init; }

    /// <summary>
    /// Y-coordinate of joiner, relative to element top, as a fraction of height.
    /// Value should normally be <c>0.0</c> to <c>1.0</c> inclusive.
    /// </summary>
    public double YPos { get; init; }

    /// <summary>
    /// X offset of joiner. If this is relative (<see cref="UnitOfMeasure.Percent"/>),
    /// the offset is calculated based on the parent element's size.
    /// </summary>
    public LayoutMeasurement XOffset { get; init; } = LayoutMeasurement.Zero;

    /// <summary>
    /// Y offset of joiner. If this is relative (<see cref="UnitOfMeasure.Percent"/>),
    /// the offset is calculated based on the parent element's size.
    /// </summary>
    public LayoutMeasurement YOffset { get; init; } = LayoutMeasurement.Zero;

    /// <summary>
    /// Create a new joiner based on this one,
    /// with a different offset
    /// </summary>
    public Joiner Offset(LayoutMeasurement dx, LayoutMeasurement dy)
    {
        return new Joiner
        {
            XPos = XPos,
            YPos = YPos,
            XOffset = dx,
            YOffset = dy
        };
    }

    /// <summary>
    /// <p>Parse a description of a joiner</p>
    /// Two-letter code for XOff and YOff, or two fractional decimals.
    /// Then optional XOffset and YOffset as <see cref="LayoutMeasurement"/>
    /// <code>
    /// TR
    /// TR 2% 0
    /// 0.75 0.25 5px 5px
    /// </code>
    /// </summary>
    public static Joiner Parse(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return TopLeft;

        var bits = str.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (bits.Length < 1) return TopLeft;

        // Full description of XPos, YPos, XOffset, YOffset
        if (bits.Length >= 4)
        {
            return new Joiner
            {
                XPos = double.Parse(bits[0]),
                YPos = double.Parse(bits[1]),
                XOffset = LayoutMeasurement.Parse(bits[2]),
                YOffset = LayoutMeasurement.Parse(bits[3])
            };
        }

        var xPos = 0.0;
        var yPos = 0.0;

        // two letter code for XPos,YPos
        switch (bits[0].ToUpperInvariant())
        {
            case "TL":
                xPos = 0.0;
                yPos = 0.0;
                break;
            case "ML":
                xPos = 0.0;
                yPos = 0.5;
                break;
            case "BL":
                xPos = 0.0;
                yPos = 1.0;
                break;

            case "TC":
                xPos = 0.5;
                yPos = 0.0;
                break;
            case "MC":
                xPos = 0.5;
                yPos = 0.5;
                break;
            case "BC":
                xPos = 0.5;
                yPos = 1.0;
                break;

            case "TR":
                xPos = 1.0;
                yPos = 0.0;
                break;
            case "MR":
                xPos = 1.0;
                yPos = 0.5;
                break;
            case "BR":
                xPos = 1.0;
                yPos = 1.0;
                break;
        }

        var xOff = LayoutMeasurement.Zero;
        var yOff = LayoutMeasurement.Zero;

        if (bits.Length > 1)
        {
            xOff = LayoutMeasurement.Parse(bits[1]);
        }

        if (bits.Length > 2)
        {
            yOff = LayoutMeasurement.Parse(bits[2]);
        }

        return new Joiner
        {
            XPos = xPos,
            YPos = yPos,
            XOffset = xOff,
            YOffset = yOff
        };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{XPos} {YPos} {XOffset} {YOffset}";
    }

    /// <inheritdoc />
    public bool Equals(Joiner? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return XPos.Equals(other.XPos) && YPos.Equals(other.YPos) && XOffset.Equals(other.XOffset) && YOffset.Equals(other.YOffset);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Joiner)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(XPos, YPos, XOffset, YOffset);
    }
}