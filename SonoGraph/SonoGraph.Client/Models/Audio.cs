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
        public static double GetDuration(Audio audio)
        {
            return audio.Sounds.Sum(sound => sound.Duration);
        }


    }
}
