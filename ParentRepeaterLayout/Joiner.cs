namespace ParentRepeaterLayout;

/// <summary>
/// Joining position of sibling elements.
/// The next sibling in a chain is placed so that its <see cref="LayoutRule.Prev"/> is at the same location
/// as the previous sibling's <see cref="LayoutRule.Next"/>.
/// </summary>
public class Joiner
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
    public double XPos { get; set; } = 0.0;

    /// <summary>
    /// Y-coordinate of joiner, relative to element top, as a fraction of height.
    /// Value should normally be <c>0.0</c> to <c>1.0</c> inclusive.
    /// </summary>
    public double YPos { get; set; } = 0.0;

    /// <summary>
    /// X offset of joiner. If this is relative (<see cref="UnitOfMeasure.Percent"/>),
    /// the offset is calculated based on the parent element's size.
    /// </summary>
    public LayoutMeasurement XOffset { get; set; } = LayoutMeasurement.Zero;

    /// <summary>
    /// Y offset of joiner. If this is relative (<see cref="UnitOfMeasure.Percent"/>),
    /// the offset is calculated based on the parent element's size.
    /// </summary>
    public LayoutMeasurement YOffset { get; set; } = LayoutMeasurement.Zero;
}