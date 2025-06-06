namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents an Audio consisting of a Waveform and a List of Sounds.
    /// </summary>
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
        /// <summary>
        /// Returns the Duration in milliseconds for a given Audio
        /// </summary>
        /// <param name="audio"></param> the audio which you want the duration of
        /// <returns></returns> Sum of the Duration of all Sounds in milliseconds
        public static double GetDuration(Audio audio)
        {
            return audio.Sounds.Sum(sound => sound.Duration);
        }


    }
}
