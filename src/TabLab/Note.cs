namespace TabLab;

public sealed record Note(
    char String,
    int Fret,
    double Frequency,
    double Duration)
{
    public override string ToString() => $"[{this.String}{this.Fret}]";
    public bool IsRest => this.String == '-';
}
