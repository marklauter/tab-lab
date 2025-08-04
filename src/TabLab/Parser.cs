namespace TabLab;

public static class Parser
{
    private static string[] ReadLines(string tab) =>
        [.. tab
        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Trim())];

    public static Note[][] Parse(string tab)
    {
        var lines = ReadLines(tab);
        return lines.Length != 6
            ? throw new InvalidDataException("need six strings, buddy")
            : [.. lines[0]
            .Select((c, i) => (c, i))
            .Where(x => x.c != '|' && !Guitar.StringsSet.Contains(x.c))
            .Select(x => lines
                .Select((line, s) => Char.IsDigit(line[x.i])
                    ? new Note(Guitar.Strings[s], line[x.i] - '0', Guitar.Frequency(s, line[x.i] - '0'), Guitar.QuarterNoteBeat / 4)
                    : new Note('-', 0, 0.0, Guitar.QuarterNoteBeat / 4)) // rest
                .ToArray())];
    }
}
