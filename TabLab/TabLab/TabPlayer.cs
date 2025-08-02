using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace TabLab;

public static class TabPlayer
{
    public static List<List<Note>> ParseTab(string tab)
    {
        var moments = new List<List<Note>>();
        var lines = tab.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                       .Select(line => line.Trim())
                       .ToList();

        if (lines.Count != 6)
        {
            Console.WriteLine("Warning: Tab does not appear to have 6 strings.");
            return moments;
        }

        // Standard tuning frequencies for a 6-string guitar (EADGBe)
        var openStringFrequencies = new[] { 329.63f, 246.94f, 196.00f, 146.83f, 110.00f, 82.41f };
        var tabLength = lines[0].Length;

        for (var i = 0; i < tabLength; i++)
        {
            var currentMomentNotes = new List<Note>();
            for (var stringIndex = 0; stringIndex < 6; stringIndex++)
            {
                var fretChar = lines[stringIndex][i];
                if (Char.IsDigit(fretChar))
                {
                    // Simple parsing for single-digit frets.
                    // A more advanced parser would handle multi-digit frets.
                    var fret = fretChar - '0';
                    var noteFrequency = (float)(openStringFrequencies[stringIndex] * Math.Pow(2, fret / 12.0));
                    currentMomentNotes.Add(new Note(noteFrequency, 0.25f)); // Default duration
                }
            }

            if (currentMomentNotes.Count != 0)
            {
                moments.Add(currentMomentNotes);
            }
        }

        return moments;
    }

    // Plays the list of moments using NAudio
    public static async Task PlayMoments(List<List<Note>> moments)
    {
        // Use WaveOutEvent for audio playback
        using var waveOut = new WaveOutEvent();
        foreach (var moment in moments)
        {
            if (moment.Count == 0)
            {
                continue;
            }

            // Use a MixingSampleProvider to play multiple notes (chords) at once
            var mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            float maxDuration = 0;

            foreach (var note in moment)
            {
                if (note.Duration > maxDuration)
                {
                    maxDuration = note.Duration;
                }

                var oscillator = new SignalGenerator(mixer.WaveFormat.SampleRate, mixer.WaveFormat.Channels)
                {
                    Gain = 0.15, // Lower gain to prevent clipping when mixing
                    Frequency = note.Frequency,
                    Type = SignalGeneratorType.Sin
                };

                // Apply envelope to prevent pops and clicks
                var envelopedNote = new EnvelopedNote(oscillator, note.Duration, 0.01f, 0.05f);
                mixer.AddMixerInput(envelopedNote);
            }

            waveOut.Init(mixer);
            waveOut.Play();

            Console.Write($"Playing {moment.Count} note(s)... ");
            // Wait for the notes in the current moment to finish playing
            await Task.Delay((int)(maxDuration * 1000));
            Console.WriteLine("Done.");

            // Stop is needed to allow the next Init call
            waveOut.Stop();
        }
    }

    // Custom sample provider that applies an envelope (fade in/out) to prevent audio pops
    private class EnvelopedNote : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly int totalSamples;
        private readonly int fadeInSamples;
        private readonly int fadeOutSamples;
        private int samplePosition;

        public EnvelopedNote(ISampleProvider source, float duration, float fadeInTime, float fadeOutTime)
        {
            this.source = source;
            WaveFormat = source.WaveFormat;

            totalSamples = (int)(duration * WaveFormat.SampleRate);
            fadeInSamples = (int)(fadeInTime * WaveFormat.SampleRate);
            fadeOutSamples = (int)(fadeOutTime * WaveFormat.SampleRate);
            samplePosition = 0;
        }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = 0;

            for (var i = 0; i < count; i++)
            {
                if (samplePosition >= totalSamples)
                {
                    // End of note reached
                    break;
                }

                // Read one sample from the source
                var sourceSample = new float[1];
                var read = source.Read(sourceSample, 0, 1);
                if (read == 0)
                {
                    break;
                }

                var sample = sourceSample[0];
                var envelope = 1.0f;

                // Apply fade in
                if (samplePosition < fadeInSamples)
                {
                    envelope = (float)samplePosition / fadeInSamples;
                }
                // Apply fade out
                else if (samplePosition >= totalSamples - fadeOutSamples)
                {
                    var fadeOutPosition = samplePosition - (totalSamples - fadeOutSamples);
                    envelope = 1.0f - (float)fadeOutPosition / fadeOutSamples;
                }

                buffer[offset + i] = sample * envelope;
                samplePosition++;
                samplesRead++;
            }

            return samplesRead;
        }
    }
}
