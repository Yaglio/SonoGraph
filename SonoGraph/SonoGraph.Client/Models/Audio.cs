using System.IO;

namespace SonoGraph.Client.Models
{
    public class Audio
    {
        public double SamplingRate { get; set; }
        public WaveFormType WaveForm { get; set; }
        public List<Sound> Sounds { get; set; }
    }

    public static class AudioUtils
    {
        public static Audio CreateTestAudio()
        {
            double baseFrequency = 440; // A4 note
            double modulationDepth = 100; // How much the frequency shifts
            double modulationSpeed = 0.1; // Controls the oscillation rate
            

            return new Audio
            {
                SamplingRate = 600,
                WaveForm = WaveFormType.Sine,
                Sounds = Enumerable
                    .Range(0, 300)
                    .Select(i => new Sound(
                        baseFrequency + modulationDepth * Math.Sin(i * modulationSpeed), 
                        0.05))
                    .ToList()
            };
        }
    }

}
