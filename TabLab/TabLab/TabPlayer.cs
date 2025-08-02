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

            if (currentMomentNotes.Any())
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
            if (!moment.Any())
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
                    Gain = 0.2, // Lower gain to prevent clipping when mixing
                    Frequency = note.Frequency,
                    Type = SignalGeneratorType.Sin
                };
                mixer.AddMixerInput(oscillator.Take(TimeSpan.FromSeconds(note.Duration)));
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
}
