namespace ParentRepeaterLayout;

/// <summary>
/// Unit of measurement
/// </summary>
public enum UnitOfMeasure
{
    /// <summary>
    /// Unit is not valid. Value should be exactly <c>0.0</c>.
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// [Absolute] Measurement is in pixels. Value should normally be greater than <c>0.0</c>
    /// </summary>
    Pixel = 1,

    /// <summary>
    /// [Absolute] Measurement is relative to size of text
    /// </summary>
    Em = 2,

    /// <summary>
    /// [Relative] Measurement is a percentage. Value should normally be <c>0.0</c> to <c>100.0</c> inclusive
    /// </summary>
    Percent = 3,
}