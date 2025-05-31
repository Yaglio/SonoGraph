using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class AudioEditorService
    {
        public List<Audio> Audios { get; set; } = [];

        public void addAudio(Audio audio)
        {
            Audios.Add(audio);
        }

        public void removeAudio(Audio audio)
        {
            Audios.Remove(audio);
        }

        public void removeAudio(HashSet<Audio> audios)
        {
            foreach (var audio in audios)
            {
                Audios.Remove(audio);
            }
        }

        public void removeAudio()
        {
            Audios.Clear();
        }

        public void removeSounds(Dictionary<Audio, HashSet<Sound>> sounds)
        {
            foreach (var audio in sounds)
            {
                audio.Key.Sounds.RemoveAll(sound => audio.Value.Contains(sound));
            }
        }

        public void mergeAudios(HashSet<Audio> selectedAudios)
        {
            if (selectedAudios.Count < 2) return;

            Audio mergedAudio = MergeAudios(selectedAudios.ToList());

            Audios.Add(mergedAudio);

            foreach (var audio in selectedAudios)
            {
                Audios.Remove(audio);
            }
        }

        public void shortenAudios(HashSet<Audio> selectedAudios, bool compress)
        {
            if (selectedAudios.Count == 0) return;

            List<Audio> shortenedAudios = ShortenAudios(selectedAudios.ToList(), compress);

            Audios.AddRange(shortenedAudios);

            foreach (var audio in selectedAudios)
            {
                Audios.Remove(audio);
            }
        }

        public void changeSoundFrequency(List<Sound> selectedSounds, double frequency)
        {
            ChangeFrequency(selectedSounds, frequency);
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
                    for (int i = 0; i < audio.Sounds.Count - 1; i += 2)
                    {
                        double newFrequency = (audio.Sounds[i].Frequency + audio.Sounds[i + 1].Frequency) / 2;
                        double newVolume = (audio.Sounds[i].Amplitude + audio.Sounds[i + 1].Amplitude) / 2;
                        double newDuration = (audio.Sounds[i].Duration + audio.Sounds[i + 1].Duration) / 2;
                        sounds.Add(new Sound(newFrequency, newVolume, newDuration));
                    }
                    if (audio.Sounds.Count % 2 != 0)
                    {
                        sounds.Add(sounds[sounds.Count - 1]);
                    }
                    shortened.Add(new Audio(audios[j].WaveForm, sounds));
                    j++;
                }
            }
            else
            {
                int j = 0;
                foreach (Audio audio in audios)
                {
                    List<Sound> sounds = new List<Sound>();
                    foreach (Sound sound in audio.Sounds)
                    {
                        double newDuration = sound.Duration / 2;
                        if (newDuration >= 50)
                        {
                            sounds.Add(new Sound(sound.Frequency, sound.Amplitude, newDuration));
                        }
                        else
                        {
                            sounds.Add(new Sound(sound.Frequency, sound.Amplitude, 50));
                        }
                    }
                    shortened.Add(new Audio(audios[j].WaveForm, sounds));
                    j++;
                }
            }

            return shortened;
        }

        public static void ChangeFrequency(List<Sound> selectedSounds, double frequency)
        {
            for (int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Frequency = frequency;
            }
        }

        public static void ChangeAmplitude(List<Sound> selectedSounds, double amplitude)
        {
            for (int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Amplitude = amplitude;
            }
        }



    }
}
