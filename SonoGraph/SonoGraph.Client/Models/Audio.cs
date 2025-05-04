using SonoGraph.Client.Pages;
using System.IO;

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
        public static Audio CreateTestAudio()
        {
            double baseFrequency = 440; // A4 note
            double modulationDepth = 100; // How much the frequency shifts
            double modulationSpeed = 0.1; // Controls the oscillation ratio

            return new Audio(WaveFormType.Sine, Enumerable
                    .Range(0, 300)
                    .Select(i => new Sound(
                        baseFrequency + modulationDepth * Math.Sin(i * modulationSpeed),
                        0.05,
                        100))
                    .ToList());
        }

        public static double GetDuration(Audio audio)
        {
            return audio.Sounds.Sum(sound => sound.Duration);
        }

        //Takes an Audio List of any length and creates a new Audio of the length of the shortest amongst the input Audios, with Frequncies and Amplitudes being the averages of the input.
        public static Audio MergeAudios(List<Audio> audios)
        {
            if (audios == null || audios.Count == 0)
                return null;

            int minLength = audios.Min(a => a.Sounds.Count);
            List<Sound> mergedSounds = new List<Sound>();

            for (int i = 0; i < minLength; i++)
            {
                double avgFrequency = audios.Average(a => a.Sounds[i].Frequency);
                double avgVolume = audios.Average(a => a.Sounds[i].Amplitude);
                double avgDuration = audios.Average(a => a.Sounds[i].Duration);
                mergedSounds.Add(new Sound(avgFrequency, avgVolume, avgDuration));
            }

            return new Audio(audios[0].WaveForm, mergedSounds);
            //@David H; siehe nächste Methode hier bitte auch noch die ursprünglichen Listen löschen, danki
        }

        //Shortens the Audio to half of its original length, either by uniting neighbouring Sounds (compress == true, changes the exact melody a little but gives a smaller and easier to edit list) or by simply halving the duration (compress == false)
        public static List<Audio> ShortenAudios(List<Audio> audios, bool compress)
        {
            List<Audio> shortened = new List<Audio>();

            if (compress)
            {
                int j = 0;
                foreach (Audio audio in audios)
                {
                    List<Sound> sounds = new List<Sound>();
                    for(int i = 0; i < audio.Sounds.Count-1; i+=2)
                    {
                        double newFrequency = (audio.Sounds[i].Frequency + audio.Sounds[i+1].Frequency) / 2;
                        double newVolume = (audio.Sounds[i].Amplitude + audio.Sounds[i+1].Amplitude) / 2;
                        double newDuration = (audio.Sounds[i].Duration + audio.Sounds[i+1].Duration) / 2;
                        sounds.Add(new Sound(newFrequency, newVolume, newDuration));
                    }
                    if(audio.Sounds.Count % 2!= 0){
                        sounds.Add(sounds[sounds.Count-1]);
                    }
                    shortened.Add(new Audio(audios[j].WaveForm, sounds));
                    j++;
                }
            }

            else
            {
                if (audios[0].Sounds[0].Duration < 100)
                {
                    return audios;
                }
                int j = 0;
                foreach(Audio audio in audios)
                {
                    List<Sound> sounds = new List<Sound>();
                    foreach(Sound sound in audio.Sounds)
                    {
                        double newDuration = sound.Duration / 2;
                        sounds.Add(new Sound(sound.Frequency, sound.Amplitude, newDuration));
                    }
                    shortened.Add(new Audio(audios[j].WaveForm, sounds));
                    j++;
                }
            }

            return shortened;
            //@David H: Du müsstest bei Funktionsaufruf dann noch die alten Audios rauslöschen, ich weiß nich, wie ich auf die Referenzen von deinen Audios von hier aus zugreifen kann, dankeeeee
        }
    }
}
