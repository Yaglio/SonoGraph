namespace SonoGraph.Client.Models
{
    public class WaveFormType : Enumeration
    {
        public static readonly WaveFormType Sine = new WaveFormType("sine");
        public static readonly WaveFormType Square = new WaveFormType("square");
        public static readonly WaveFormType Triangle = new WaveFormType("triangle");
        public static readonly WaveFormType Sawtooth = new WaveFormType("sawtooth");

        private WaveFormType(string name) : base(name) { }
    }

    public abstract class Enumeration : IComparable
    {
        public string Value { get; private set; }

        protected Enumeration(string value) => (Value) = (value);

        public int CompareTo(object? obj)
        {
            return Value.CompareTo(((Enumeration)obj).Value); ;
        }
    }
}
