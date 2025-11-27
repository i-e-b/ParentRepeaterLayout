namespace ParentRepeaterLayout;

/// <summary>
/// Token to be added to the layout.
/// </summary>
public class LayoutToken
{
    /// <summary>
    /// Number of rules to escape after placing this token.
    /// Should normally be <c>0</c>
    /// </summary>
    public int Break { get; set; }

    /// <summary>
    /// Thing to render in this token
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Width of the token
    /// </summary>
    public LayoutMeasurement? Width { get; set; }

    /// <summary>
    /// Height of the token
    /// </summary>
    public LayoutMeasurement? Height { get; set; }
}