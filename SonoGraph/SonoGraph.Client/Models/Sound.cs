namespace SonoGraph.Client.Models
{
    public class Sound
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
