using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace TabLab;

public static class Player
{
    public static async Task PlayMoments(Note[][] moments)
    {
        using var waveOut = new WaveOutEvent();
        var mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
        var activeNotes = new Dictionary<char, ContinuousNote>();

        var hasInitialized = false;
        foreach (var moment in moments)
        {
            var maxDuration = moment.Max(n => n.Duration);

            foreach (var note in moment.Where(n => !n.IsRest))
            {
                if (activeNotes.TryGetValue(note.String, out var existing))
                {
                    existing.Stop();
                    _ = activeNotes.Remove(note.String);
                }

                var continuousNote = new ContinuousNote(note.Frequency, 0.25);
                mixer.AddMixerInput(continuousNote);
                activeNotes[note.String] = continuousNote;

                // Initialize on first note
                if (!hasInitialized)
                {
                    waveOut.Init(mixer);
                    waveOut.Play();
                    hasInitialized = true;
                }
            }

            Console.Write($"Playing {moment.Length} note(s) {moment.AsString()}");
            await Task.Delay((int)(maxDuration * 1000));
            Console.WriteLine(".");
        }

        await Task.Delay(5000);
    }

    private static string AsString(this Note[] moment) => String.Join(',', moment.Where(n => !n.IsRest).Select(n => n.ToString()));

    private sealed class ContinuousNote
        : ISampleProvider
    {
        private readonly SignalGenerator oscillator;
        private readonly double decayRate;
        private int samplePosition;
        private bool isStopped;
        private bool isFadingOut;
        private int fadeOutPosition;
        private readonly int fadeOutSamples = (int)(0.05 * 44100); // 50ms fade

        public WaveFormat WaveFormat { get; }

        public ContinuousNote(double frequency, double gain)
        {
            this.oscillator = new SignalGenerator(44100, 1)
            {
                Gain = gain,
                Frequency = frequency,
                Type = SignalGeneratorType.Triangle
            };

            this.decayRate = 0.7;
            this.WaveFormat = this.oscillator.WaveFormat;
        }

        public void Stop() => this.isFadingOut = true;

        public int Read(float[] buffer, int offset, int count)
        {
            if (this.isStopped)
            {
                return 0;
            }

            var samplesRead = this.oscillator.Read(buffer, offset, count);

            for (var i = 0; i < samplesRead; i++)
            {
                var timeSeconds = (this.samplePosition + i) / (double)this.WaveFormat.SampleRate;
                var decayMultiplier = Math.Exp(-this.decayRate * timeSeconds);

                var sample = buffer[offset + i] * (float)decayMultiplier;

                // Apply fade-out if stopping
                if (this.isFadingOut)
                {
                    var fadeMultiplier = 1.0f - (float)this.fadeOutPosition / this.fadeOutSamples;
                    sample *= Math.Max(0, fadeMultiplier);
                    this.fadeOutPosition++;

                    if (this.fadeOutPosition >= this.fadeOutSamples)
                    {
                        this.isStopped = true;
                    }
                }

                buffer[offset + i] = sample;
            }

            this.samplePosition += samplesRead;
            return samplesRead;
        }
    }
}

