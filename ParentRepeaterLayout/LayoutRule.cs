namespace ParentRepeaterLayout;

/// <summary>
/// A single rule that should be part of a set to perform a layout
/// </summary>
public class LayoutRule
{
    /// <summary>
    /// <p>Width of elements at this rule.</p>
    /// <p>The first layout rule must have a non-zero <see cref="Width"/> and <see cref="Height"/>.</p>
    /// <p>All other rules assume a flexible size if this is <see cref="LayoutMeasurement.Zero"/></p>
    /// </summary>
    public LayoutMeasurement Width { get; set; } = LayoutMeasurement.Zero;

    /// <summary>
    /// <p>Height of elements at this rule. If this is a percentage, </p>
    /// <p>The first layout rule must have a non-zero <see cref="Width"/> and <see cref="Height"/>.</p>
    /// <p>All other rules assume a flexible size if this is <see cref="LayoutMeasurement.Zero"/></p>
    /// </summary>
    public LayoutMeasurement Height { get; set; } = LayoutMeasurement.Zero;

    /// <summary>
    /// If <c>true</c>, all siblings will be kept together,
    /// and the content will break it's parent container (starting a new container)
    /// if not all items can be fit together.
    /// If a new container cannot fit the siblings, <see cref="Glue"/> is ignored
    /// </summary>
    public bool Glue { get; set; } = false;

    /// <summary>
    /// Joining position on 'this' element where the 'next' sibling is added.
    /// </summary>
    public Joiner Next { get; set; } = Joiner.TopRight;

    /// <summary>
    /// Joining position on the 'next' sibling.
    /// </summary>
    public Joiner Prev { get; set; } = Joiner.TopLeft;
}