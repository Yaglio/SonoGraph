namespace SonoGraph.Client.Models
{
    public class Audio
    {
        public double SamplingRate { get; set; }
        public WaveForm WaveForm { get; set; }
        public List<Sound> Sounds { get; set; }
    }

    public enum WaveForm
    {
        Sine,
        Square,
        Triangle,
        Sawtooth
    }
}
