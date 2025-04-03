namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents a sound with a frequency and amplitude.
    /// </summary>
    public struct Sound
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

        public Sound(double frequency, double amplitude)
        {
            Frequency = frequency;
            Amplitude = amplitude;
        }
    }
}
