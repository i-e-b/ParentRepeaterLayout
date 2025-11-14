namespace UnitTests;

/// <summary>
/// Guess string width based on common fonts.
/// This is only a rough estimate for when reading a real font is not practical.
/// </summary>
public static class FontGuess
{
    /// <summary> A value to adjust measurements </summary>
    private const double FudgeFactor = 0.85;

    public static double MeasureStrings(double fontSize, ReadOnlySpan<char> str)
    {
        var guess       = 0.0;

        foreach (var c in str)
        {
            guess += CharSize(c) * fontSize * FudgeFactor;
        }

        return guess;
    }

    public static double MeasureCharacter(double fontSize, char c)
    {
        return CharSize(c) * fontSize * FudgeFactor;
    }

    private static double CharSize(char c)
    {
        return c >= _charWidths.Length ? 0.8 : _charWidths[c];
    }

    /// <summary> Approximate widths of characters in Calibri Regular </summary>
    private static readonly double[] _charWidths =
    [
        1.1, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.1, 1.0, 1.0, 1.0, 1.0,
        1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, /*   */ 1.089,
        /* ! */ 0.205, /* " */ 0.515, /* # */ 0.925, /* $ */ 0.816, /* % */ 1.289, /* & */ 1.280,
        /* ' */ 0.170, /* ( */ 0.357, /* ) */ 0.356, /* * */ 0.681, /* + */ 0.859, /* , */ 0.321,
        /* - */ 0.478, /* . */ 0.210, /* / */ 0.759, /* 0 */ 0.874, /* 1 */ 0.735, /* 2 */ 0.791,
        /* 3 */ 0.792, /* 4 */ 0.909, /* 5 */ 0.798, /* 6 */ 0.830, /* 7 */ 0.824, /* 8 */ 0.852,
        /* 9 */ 0.832, /* : */ 0.205, /* ; */ 0.317, /* < */ 0.830, /* = */ 0.812, /* > */ 0.831,
        /* ? */ 0.705, /* @ */ 1.617, /* A */ 1.089, /* B */ 0.833, /* C */ 0.917, /* D */ 0.972,
        /* E */ 0.703, /* F */ 0.664, /* G */ 1.028, /* H */ 0.910, /* I */ 0.168, /* J */ 0.466,
        /* K */ 0.824, /* L */ 0.655, /* M */ 1.374, /* N */ 0.955, /* O */ 1.130, /* P */ 0.776,
        /* Q */ 1.285, /* R */ 0.843, /* S */ 0.777, /* T */ 0.945, /* U */ 0.952, /* V */ 1.068,
        /* W */ 1.659, /* X */ 0.943, /* Y */ 0.915, /* Z */ 0.837, /* [ */ 0.340, /* \ */ 0.760,
        /* ] */ 0.340, /* ^ */ 0.785, /* _ */ 1.007, /* ` */ 0.455, /* a */ 0.722, /* b */ 0.809,
        /* c */ 0.693, /* d */ 0.809, /* e */ 0.809, /* f */ 0.595, /* g */ 0.832, /* h */ 0.757,
        /* i */ 0.199, /* j */ 0.390, /* k */ 0.722, /* l */ 0.161, /* m */ 1.304, /* n */ 0.757,
        /* o */ 0.879, /* p */ 0.809, /* q */ 0.809, /* r */ 0.514, /* s */ 0.622, /* t */ 0.574,
        /* u */ 0.758, /* v */ 0.837, /* w */ 1.327, /* x */ 0.783, /* y */ 0.838, /* z */ 0.623,
        /* { */ 0.482, /* | */ 0.151, /* } */ 0.484, /* ~ */ 0.881, 1.0
    ];
}