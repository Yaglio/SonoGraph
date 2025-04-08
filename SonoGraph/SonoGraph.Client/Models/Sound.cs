namespace SonoGraph.Client.Models
{
    public class Sound
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }
        public TimeOnly Time { get; set; }
        
        public Sound (double Frequency, double Amplitude, TimeOnly Time)
        {
            this.Frequency = Frequency;
            this.Amplitude = Amplitude;
            this.Time = Time;
        }

    }
}
