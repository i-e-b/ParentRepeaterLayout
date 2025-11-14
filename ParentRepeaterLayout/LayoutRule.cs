namespace ParentRepeaterLayout;

/// <summary>
/// A single rule that should be part of a set to perform a layout
/// </summary>
public class LayoutRule
{
    /// <summary>
    /// Create a rule with default values
    /// </summary>
    public LayoutRule() { }

    /// <summary>
    /// Create a rule with values
    /// </summary>
    public LayoutRule(string? name = null,
        LayoutMeasurement? width = null, LayoutMeasurement? height = null,
        Joiner? next = null, Joiner? prev = null, bool glue = false)
    {
        Name = name;
        Glue = glue;

        if (width is not null) Width = width;
        if (height is not null) Height = height;
        if (next is not null) Next = next;
        if (prev is not null) Prev = prev;
    }

    /// <summary>
    /// [Optional] Name for this rule
    /// </summary>
    public string? Name { get; set; }

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

    /// <summary>
    /// Parse a description into a set of layout rules
    /// </summary>
    public static LayoutRule[] ParseRules(string description)
    {
        var result = new List<LayoutRule>();

        var lines = description.Split('\r', '\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            result.Add(ParseSingleRule(line));
        }

        return result.ToArray();
    }

    private static LayoutRule ParseSingleRule(string line)
    {
        var pairs = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var name   = "";
        var width  = LayoutMeasurement.Zero;
        var height = LayoutMeasurement.Zero;
        var next   = Joiner.TopRight;
        var prev   = Joiner.TopLeft;
        var glue   = false;

        foreach (var pair in pairs)
        {
            var bits = pair.Split('=', StringSplitOptions.TrimEntries);
            if (bits.Length < 2) continue;

            switch (bits[0].ToLowerInvariant())
            {
                case "name":
                    name = bits[1];
                    break;
                case "width":
                    width = LayoutMeasurement.Parse(bits[1]);
                    break;
                case "height":
                    height = LayoutMeasurement.Parse(bits[1]);
                    break;
                case "next":
                    next = Joiner.Parse(bits[1]);
                    break;
                case "prev":
                    prev = Joiner.Parse(bits[1]);
                    break;
                case "glue":
                    glue = bool.Parse(bits[1]);
                    break;
            }
        }

        return new LayoutRule
        {
            Name = name,
            Width = width,
            Height = height,
            Glue = glue,
            Next = next,
            Prev = prev
        };
    }
}