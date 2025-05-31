using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    /// <summary>
    /// Provides methods for editing audio collections, including adding, removing, merging, and shortening audios.
    /// </summary>
    public class AudioEditorService
    {
        /// <summary>
        /// Represents a collection of audios that can be edited.
        /// </summary>
        public HashSet<Audio> AudioCollection { get; set; } = new();

        /// <summary>
        /// Adds the specified audio to the AudioCollection collection.
        /// </summary>
        /// <param name="audio"></param>
        public void AddAudio(Audio audio)
        {
            AudioCollection.Add(audio);
        }

        /// <summary>
        /// Removes the specified audios from the AudioCollection collection.
        /// </summary>
        /// <param name="audios"></param>
        public void RemoveAudio(HashSet<Audio> audios)
        {
            foreach (var audio in audios)
            {
                AudioCollection.Remove(audio);
            }
        }

        /// <summary>
        /// Removes all audios from the AudioCollection collection.
        /// </summary>
        public void RemoveAudio()
        {
            AudioCollection.Clear();
        }

        /// <summary>
        /// Removes the specified sounds from the audios in the provided dictionary.
        /// </summary>
        /// <param name="sounds"></param>
        public void RemoveSounds(Dictionary<Audio, HashSet<Sound>> sounds)
        {
            foreach (var audio in sounds)
            {
                audio.Key.Sounds.RemoveAll(sound => audio.Value.Contains(sound));
            }
        }

        /// <summary>
        /// Merges the selected audios into a single audio and adds it to the AudioCollection collection.
        /// </summary>
        /// <param name="selectedAudios"></param>
        public void MergeAudios(HashSet<Audio> selectedAudios)
        {
            if (selectedAudios.Count < 2) return;

            Audio mergedAudio = MergeAudios(selectedAudios.ToList());

            AudioCollection.Add(mergedAudio);

            foreach (var audio in selectedAudios)
            {
                AudioCollection.Remove(audio);
            }
        }

        /// <summary>
        /// Shortens the selected audios by either compressing or halving the duration of their sounds.
        /// </summary>
        /// <param name="selectedAudios"></param>
        /// <param name="compress"></param>
        public void ShortenAudios(HashSet<Audio> selectedAudios, bool compress)
        {
            if (selectedAudios.Count == 0) return;

            List<Audio> shortenedAudios = ShortenAudios(selectedAudios.ToList(), compress);

            shortenedAudios.ForEach(audio => AudioCollection.Add(audio));

            foreach (var audio in selectedAudios)
            {
                AudioCollection.Remove(audio);
            }
        }

        /// <summary>
        /// Updates the amplitude of the selected sounds to the specified amplitude.
        /// </summary>
        /// <param name="selectedSounds"></param>
        /// <param name="frequency"></param>
        public void UpdateSoundFrequencies(List<Sound> selectedSounds, double frequency)
        {
            ChangeFrequency(selectedSounds, frequency);
        }

        /// <summary>
        /// Merges a list of audios into a single audio by averaging the properties of their sounds.
        /// </summary>
        /// <param name="audios"></param>
        /// <returns></returns>
        private static Audio MergeAudios(List<Audio> audios)
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

        /// <summary>
        /// Shortens a list of audios by either compressing or halving the duration of their sounds.
        /// </summary>
        /// <param name="audios"></param>
        /// <param name="compress"></param>
        /// <returns></returns>
        private static List<Audio> ShortenAudios(List<Audio> audios, bool compress)
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

        /// <summary>
        /// Changes the frequency of the selected sounds to the specified frequency.
        /// </summary>
        /// <param name="selectedSounds"></param>
        /// <param name="frequency"></param>
        private static void ChangeFrequency(List<Sound> selectedSounds, double frequency)
        {
            for (int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Frequency = frequency;
            }
        }

        /// <summary>
        /// Changes the amplitude of the selected sounds to the specified amplitude.
        /// </summary>
        /// <param name="selectedSounds"></param>
        /// <param name="amplitude"></param>
        private static void ChangeAmplitude(List<Sound> selectedSounds, double amplitude)
        {
            for (int i = 0; i < selectedSounds.Count; i++)
            {
                Sound sound = selectedSounds[i];
                sound.Amplitude = amplitude;
            }
        }



    }
}
