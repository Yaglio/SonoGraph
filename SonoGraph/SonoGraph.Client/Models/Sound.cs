namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents a sound with a frequency and amplitude.
    /// </summary>
    public class Sound
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

        /// <summary>
        /// The duration of the sound in milliseconds.
        /// </summary>
        public double Duration { get; set; }

        public int Id { get; }
        private static int nextID = 0;

        public Sound(double frequency, double amplitude, double duration)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Duration = duration;
            Id = nextID++;
        }

        public static void ChangeFrequency(List<Sound> selectedSounds, double frequency)
        {
            for (int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Frequency = frequency;
            }
        }

        public static void ChangeAmplitude(List<Sound> selectedSounds, double amplitude)
        {
            for(int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Amplitude = amplitude;
            }
        }
    }
}