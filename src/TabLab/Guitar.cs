namespace TabLab;

public static class Guitar
{
    public static readonly HashSet<char> StringsSet = [
         'e',
         'B',
         'G',
         'D',
         'A',
         'E'];

    public static readonly double[] OpenStringFrequencies = [329.63f, 246.94f, 196.00f, 146.83f, 110.00f, 82.41f];
    public static readonly IReadOnlyDictionary<char, double> OpenStringFrequenciesMap = new Dictionary<char, double>
    {
        { 'e', 329.63 },
        { 'B', 246.94 },
        { 'G', 196.00 },
        { 'D', 146.83 },
        { 'A', 110.00 },
        { 'E', 82.41 }
    };

    public static readonly char[] Strings = ['e', 'D', 'G', 'B', 'A', 'E'];

    public static double Frequency(char @string, int fret) =>
        OpenStringFrequenciesMap[@string] * Math.Pow(2, fret / 12.0);

    public static double Frequency(int @string, int fret) =>
        Frequencies[@string][fret];

    public const int MaxFrets = 24;

    // [string, fret] -> frequency
    public static readonly double[][] Frequencies = [.. OpenStringFrequencies
        .Select(openFreq => Enumerable.Range(0, MaxFrets + 1)
            .Select(fret => openFreq * Math.Pow(2, fret / 12.0))
            .ToArray())];

    // 120 BPM
    public const double QuarterNoteBeat = 0.5d;
}
