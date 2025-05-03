namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents a sound with a frequency and amplitude.
    /// </summary>
    public struct Sound
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

        public static void ChangeFrequency(Sound sound, double frequency)
        {
            sound.Frequency = frequency;
        }

        public static void ChangeAmplitude(Sound sound, double amplitude)
        {
            sound.Amplitude = amplitude;
        }

        public static void ChangeDuration(Sound sound, double duration)
        {
            sound.Duration = duration;
        }
    }
}
