using System.IO;

namespace SonoGraph.Client.Models
{
    public class Audio
    {
        public WaveFormType WaveForm { get; set; }
        public List<Sound> Sounds { get; set; }

        public Audio(WaveFormType waveForm, List<Sound> sounds)
        {
            WaveForm = waveForm;
            Sounds = sounds;
        }
    }

    public static class AudioUtils
    {
        public static Audio CreateTestAudio()
        {
            double baseFrequency = 440; // A4 note
            double modulationDepth = 100; // How much the frequency shifts
            double modulationSpeed = 0.1; // Controls the oscillation ratio

            return new Audio(WaveFormType.Sine, Enumerable
                    .Range(0, 300)
                    .Select(i => new Sound(
                        baseFrequency + modulationDepth * Math.Sin(i * modulationSpeed),
                        0.05,
                        100))
                    .ToList());
        }

        public static double GetDuration(Audio audio)
        {
            return audio.Sounds.Sum(sound => sound.Duration);
        }

        //ToDo
        public static Audio MergeAudios(List<Audio> audios)
        {
            
        }
    }

}
