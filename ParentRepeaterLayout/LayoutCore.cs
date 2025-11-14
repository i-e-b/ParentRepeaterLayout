namespace ParentRepeaterLayout;

/// <summary>
/// Core of the layout engine
/// </summary>
public class LayoutCore
{
    private readonly IEnumerable<LayoutRule> _rules;

    /// <summary>
    /// Create a layout engine with a set of rules
    /// </summary>
    public LayoutCore(IEnumerable<LayoutRule> rules)
    {
        _rules = rules;
    }


    /// <summary>
    /// Perform a layout with the rule set and given tokens
    /// </summary>
    public LayoutResult Layout(IEnumerable<LayoutToken> tokens)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Layout of tokens, plus metadata
/// </summary>
public class LayoutResult
{
}

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