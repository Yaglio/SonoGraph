﻿using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    /// <summary>
    /// class Soundservice is responsible for translating the Coordinates into Sounds while Drawing on the Canvas and saving those Sounds into one continous Audio
    /// This Audio is given to the Editor once the drawing process has finished
    /// </summary>
    public class SoundService
    {
        
        private Audio ? audio = null;
        private DateTime dateTime;
        private CancellationTokenSource cancellationTokenSource;
        private readonly AudioPlayerService audioPlayerService;
        private AsyncSoundStream? asyncSoundStream;
        private readonly AudioEditorService _audioEditorService;

        private readonly int minFrequency = 50;
        private readonly int maxFrequency = 8000;

        public SoundService(AudioPlayerService Service, AudioEditorService audioEditorService)
        {
            audioPlayerService = Service;
            cancellationTokenSource = new CancellationTokenSource();
            _audioEditorService = audioEditorService;

            audioPlayerService.Initialize();
        }
        /// <summary>
        /// Starts the Soundcollection process. Needs to be called before any other method
        /// </summary>
        /// <param name="waveForm"></param> the Type of Wave that the Audio should be played as (Sinus, sawtooth, square, triangle) not changeable in final Project
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception> if the Audio or the Soundstream could not be created or saved in the Variable
        public async Task StartSound(WaveFormType waveForm)
        {
            dateTime = DateTime.Now;

            audio = new Audio(waveForm, new List<Sound>());

            asyncSoundStream = AsyncSoundStream.Create();

            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            await audioPlayerService.Play(asyncSoundStream.GetSoundsAsync(cancellationTokenSource.Token), audio.WaveForm, cancellationTokenSource.Token);

        }
        /// <summary>
        /// Processes a Sound after 100ms and adds it to the Queue of Soundstream and Audio
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="amplitude"></param>
        /// <exception cref="InvalidOperationException"></exception> if method startSound was not successfully called
        public void ProcessSound(double frequency, double amplitude)
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            DateTime newDateTime = DateTime.Now;

            if (newDateTime.Subtract(dateTime).TotalMilliseconds > 100)
            {
                var mappedFrequency = minFrequency * Math.Pow(maxFrequency / minFrequency, frequency);

                Sound sound = new Sound(mappedFrequency, amplitude, 100.0);

                asyncSoundStream.AddSound(sound);

                audio.Sounds.Add(sound);

                dateTime = newDateTime;
            }
        }
        /// <summary>
        /// Stops the Soundstream and puts the Audio into storage
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception> if method startSound was not successfully called
        public void EndSound()
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            asyncSoundStream.Complete();

            _audioEditorService.AddAudio(audio);

            audio = null;
        }
    }
}
