namespace TabLab;

public sealed record Note(
    char String,
    int Fret,
    float Frequency,
    float Duration)
{
    public override string ToString() => $"[{this.String}{this.Fret}]";
}
