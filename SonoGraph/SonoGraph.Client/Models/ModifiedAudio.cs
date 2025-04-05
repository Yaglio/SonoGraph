namespace SonoGraph.Client.Models
{
    public class ModifiedAudio : Audio
    {
        public double DurationModifier { get; set; }
        public double VolumeModifier { get; set; }
        public double StartTime { get; set; }

        public new List<Sound> Sounds
        {
            get
            {
                return base.Sounds
                    .Select(sound => new Sound(
                        sound.Frequency,
                        sound.Amplitude * VolumeModifier,
                        sound.Duration * DurationModifier))
                    .ToList();
            }
            set => base.Sounds = value;
        }

        public ModifiedAudio(WaveFormType waveForm, List<Sound> sounds, double durationModifier, double volumeModifier, double startTime) : base(waveForm, sounds)
        {
            DurationModifier = durationModifier;
            VolumeModifier = volumeModifier;
            StartTime = startTime;
        }

        public ModifiedAudio(WaveFormType waveForm, List<Sound> sounds) : this(waveForm, sounds, 1.0, 1.0, 0.0)
        {
        }
    }
}
