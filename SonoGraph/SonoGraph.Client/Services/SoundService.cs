using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using SonoGraph.Client.Models;
using SonoGraph.Client.Pages;
using SonoGraph.Client.Services;

namespace SonoGraph.Client.Services
{
    public class SoundService
    {
        private Audio ? audio = null;
        private DateTime dateTime;
        private CancellationTokenSource cancellationTokenSource;
        private readonly AudioPlayerService audioPlayerService;
        private AsyncSoundStream? asyncSoundStream;
        private readonly StorageService storageService;
        public SoundService (AudioPlayerService Service, StorageService storage) {
            audioPlayerService = Service;
            cancellationTokenSource = new CancellationTokenSource();
            storageService = storage;
        }

        public async Task StartSound(Coordinate coordinate, WaveFormType waveForm)
        {
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            dateTime = DateTime.Now;
            audio = new Audio(waveForm, new List<Sound>());
            audio.Sounds.Add(sound);
            asyncSoundStream = AsyncSoundStream.Create();
            asyncSoundStream.AddSound(sound);
            await PlaySound(sound);

        }
        public async Task ProcessSound(Coordinate coordinate)
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            DateTime newDateTime = DateTime.Now;
            if (newDateTime.Subtract(dateTime).TotalMilliseconds < 100.0)
            {
                return;
            }
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            dateTime = newDateTime;
            audio.Sounds.Add(sound);
            asyncSoundStream.AddSound(sound);
            await PlaySound(sound);
        }

        public void EndSound()
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            asyncSoundStream.Complete();
            storageService.Audios.Add(audio);
        }

        private async Task PlaySound(Sound sound)
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            await audioPlayerService.Play(asyncSoundStream.GetSoundsAsync(cancellationTokenSource.Token), audio.WaveForm, cancellationTokenSource.Token);
        }
    }
}
