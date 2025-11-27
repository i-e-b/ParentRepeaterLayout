using System.Collections;

namespace ParentRepeaterLayout;

/// <summary>
/// Core of the layout engine
/// </summary>
public class LayoutCore
{
    private readonly ContentChain _chain;

    /// <summary>
    /// Create a layout engine with a set of rules
    /// </summary>
    public LayoutCore(IEnumerable<LayoutRule> rules, double width, double height, double emSize)
    {
        _chain = new ContentChain(rules, width, height, emSize);
    }


    /// <summary>
    /// Perform a layout with the rule set and given tokens
    /// </summary>
    public LayoutResult Layout(IEnumerable<LayoutToken> input)
    {
        ResetInitialChain();
        // TODO: Continue from 'function run(chain, input)'
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set up node zero on all but the last type in the chain
    /// </summary>
    private void ResetInitialChain()
    {
        foreach (var node in _chain)
        {
            node.Tokens.Clear(); // 'Tokens' is called 'nodes' in the JS.
        }

        _chain[0].Tokens.Add(new PositionedToken
        {
            Token = null,
            X = 0,
            Y = 0,
            Width = _chain[0].Type.Width.Value,
            Height = _chain[0].Type.Height.Value,
            Parent = 0
        });

        for (var i = 1; i < _chain.Count; i++) {
            var n = new PositionedToken{
                Width=_chain[i].Type.Width.Value,
                Height=_chain[i].Type.Height.Value,
                Parent=0
            };
            // initial position is prev->prev
            RelPos(_chain[i-1].Type.Prev, _chain[i].Type.Prev, _chain[i-1].LastNode(), n);
            _chain[i].Tokens.Add(n);
        }
    }

    /// <summary>
    /// Update the x and y position of 'nextNode'
    /// </summary>
    private static void RelPos(Joiner prev, Joiner next, PositionedToken prevNode, PositionedToken nextNode)
    {
        var pRefX = (prev.XPos * nextNode.Width) + nextNode.X;
        var pRefY = (prev.YPos * nextNode.Height) + nextNode.Y;

        var nRefX = (next.XPos * prevNode.Width) + prevNode.X;
        var nRefY = (next.YPos * prevNode.Height) + prevNode.Y;

        nextNode.X = pRefX + prev.XOffset.Value + nRefX + next.XOffset.Value;
        nextNode.Y = pRefY + prev.YOffset.Value + nRefY + next.YOffset.Value;
    }
}

/// <summary>
/// Layout of tokens, plus metadata
/// </summary>
public class LayoutResult
{
}

/// <summary>
/// A set of layout nodes with completed sizes
/// </summary>
public class ContentChain : IEnumerable<ContentNode>
{
    private readonly double            _emSize;
    private readonly List<ContentNode> _nodes = [];

    /// <summary>
    /// Count of nodes
    /// </summary>
    public int Count => _nodes.Count;

    /// <summary>
    /// Convert layout rules to a layout with fixed sizes
    /// </summary>
    public ContentChain(IEnumerable<LayoutRule> rules, double width, double height, double emSize)
    {
        _emSize = emSize;
        var prev = new ContentNode(width, height);

        foreach (var rule in rules)
        {
            var node = Normalise(rule, prev);
            _nodes.Add(node);
            prev = node;
        }
    }

    /// <summary>
    /// Fix a relative measure into an absolute one.
    /// </summary>
    private double Fix(LayoutMeasurement measure, LayoutMeasurement? relative)
    {
        return measure.Unit switch
        {
            UnitOfMeasure.Pixel => measure.Value,
            UnitOfMeasure.Em => measure.Value * _emSize,
            UnitOfMeasure.Percent => measure.Value * 0.01 * (relative?.Value ?? 0.0),
            UnitOfMeasure.Invalid => 0.0,
            _ => throw new Exception($"Invalid measure unit: {(int)measure.Unit}")
        };
    }

    private ContentNode Normalise(LayoutRule type, ContentNode parent)
    {
        return new ContentNode
        {
            Type = new LayoutRule
            {
                Name = type.Name ?? "untitled",
                Width = Fix(type.Width, parent.Type.Width),
                Height = Fix(type.Height, parent.Type.Height),
                Glue = type.Glue,
                Next = new Joiner
                {
                    XPos = type.Next.XPos,
                    YPos = type.Next.YPos,
                    XOffset = Fix(type.Next.XOffset, parent.Type.Width),
                    YOffset = Fix(type.Next.YOffset, parent.Type.Height)
                },
                Prev = new Joiner
                {
                    XPos = type.Prev.XPos,
                    YPos = type.Prev.YPos,
                    XOffset = Fix(type.Prev.XOffset, parent.Type.Width),
                    YOffset = Fix(type.Prev.YOffset, parent.Type.Height)
                }
            },
        };
    }

    /// <summary>
    /// Get node at index
    /// </summary>
    public ContentNode this[int i] => _nodes[i];

    /// <inheritdoc />
    public IEnumerator<ContentNode> GetEnumerator() => _nodes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Node for <see cref="ContentChain"/>
/// </summary>
public class ContentNode
{
    /// <summary>
    /// Create an empty content node with fixed size
    /// </summary>
    public ContentNode(double width, double height)
    {
        Type = new LayoutRule
        {
            Name = null,
            Width = new LayoutMeasurement
            {
                Value = width,
                Unit = UnitOfMeasure.Pixel
            },
            Height = new LayoutMeasurement
            {
                Value = height,
                Unit = UnitOfMeasure.Pixel
            }
        };
    }

    /// <summary>
    /// Create a content node
    /// </summary>
    public ContentNode() { }

    /// <summary>
    /// Rule for this node. This should have only 'pixel' measures
    /// </summary>
    public LayoutRule Type { get; set; } = new();

    /// <summary>
    /// Content tokens for this node
    /// </summary>
    public List<PositionedToken> Tokens { get; set; } = [];


    /// <summary>
    /// Return the last token in this node
    /// </summary>
    public PositionedToken LastNode() => Tokens[^1];
}

/// <summary>
/// Position of a token after layout
/// </summary>
public class PositionedToken
{
    /// <summary>
    /// Source token
    /// </summary>
    public LayoutToken? Token { get; set; }

    /// <summary> X position </summary>
    public double X { get; set; }
    /// <summary> Y position </summary>
    public double Y { get; set; }

    /// <summary> Width </summary>
    public double Width { get; set; }
    /// <summary> Height </summary>
    public double Height { get; set; }

    /// <summary>
    /// Index of parent node
    /// </summary>
    public int Parent { get; set; }
}