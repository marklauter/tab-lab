using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace TabLab;

public static class Player
{
    public static async Task PlayMoments(Note[][] moments)
    {
        using var waveOut = new WaveOutEvent();
        foreach (var moment in moments)
        {
            var maxDuration = moment.Max(n => n.Duration);
            var voicedNotes = moment.Where(n => !n.IsRest).ToArray();

            if (voicedNotes.Length > 0)
            {
                var mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));

                foreach (var note in voicedNotes)
                {
                    var oscillator = new SignalGenerator(mixer.WaveFormat.SampleRate, mixer.WaveFormat.Channels)
                    {
                        Gain = 0.25,
                        Frequency = note.Frequency,
                        Type = SignalGeneratorType.Triangle
                    };

                    var envelopedNote = new EnvelopedNote(oscillator, note.Duration, 0.01, 0.1);
                    mixer.AddMixerInput(envelopedNote);
                }

                waveOut.Init(mixer);
                waveOut.Play();
            }

            Console.Write($"Playing {moment.Length} note(s) {moment.AsString()}");
            await Task.Delay((int)(maxDuration * 1000));
            Console.WriteLine(".");

            if (voicedNotes.Length > 0)
            {
                waveOut.Stop();
            }
        }
    }

    private static string AsString(this Note[] moment) => String.Join(',', moment.Where(n => !n.IsRest).Select(n => n.ToString()));

    // Custom sample provider that applies an envelope (fade in/out) to prevent audio pops
    private sealed class EnvelopedNote
        : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly int totalSamples;
        private readonly int fadeInSamples;
        private readonly int fadeOutSamples;
        private int samplePosition;

        public EnvelopedNote(ISampleProvider source, double duration, double fadeInTime, double fadeOutTime)
        {
            this.source = source;
            this.WaveFormat = source.WaveFormat;

            this.totalSamples = (int)(duration * this.WaveFormat.SampleRate);
            this.fadeInSamples = (int)(fadeInTime * this.WaveFormat.SampleRate);
            this.fadeOutSamples = (int)(fadeOutTime * this.WaveFormat.SampleRate);
            this.samplePosition = 0;
        }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = 0;

            for (var i = 0; i < count; i++)
            {
                if (this.samplePosition >= this.totalSamples)
                {
                    // End of note reached
                    break;
                }

                // Read one sample from the source
                var sourceSample = new float[1];
                var read = this.source.Read(sourceSample, 0, 1);
                if (read == 0)
                {
                    break;
                }

                var sample = sourceSample[0];
                var envelope = 1.0f;

                // Apply fade in
                if (this.samplePosition < this.fadeInSamples)
                {
                    envelope = (float)this.samplePosition / this.fadeInSamples;
                }
                // Apply fade out
                else if (this.samplePosition >= this.totalSamples - this.fadeOutSamples)
                {
                    var fadeOutPosition = this.samplePosition - (this.totalSamples - this.fadeOutSamples);
                    envelope = 1.0f - (float)fadeOutPosition / this.fadeOutSamples;
                }

                buffer[offset + i] = sample * envelope;
                this.samplePosition++;
                samplesRead++;
            }

            return samplesRead;
        }
    }
}

