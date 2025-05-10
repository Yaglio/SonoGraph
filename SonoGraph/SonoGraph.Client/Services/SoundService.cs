using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using SonoGraph.Client.Models;
using SonoGraph.Client.Pages;
using SonoGraph.Client.Services;

namespace SonoGraph.Client.Services
{
    public class SoundService
    {
        Audio audio {  get; set; }
        DateTime dateTime { get; set; }
        CancellationTokenSource cancellationTokenSource { get; set; }
        AudioPlayerService audioPlayerService { get; set; }
        AsyncSoundStream? asyncSoundStream;
        public SoundService (AudioPlayerService Service) {
            audioPlayerService = Service;
            audio = new Audio(WaveFormType.Sine, new List<Sound> { });
            dateTime = DateTime.Now;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartSound(Coordinate coordinate, WaveFormType waveForm)
        {
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            dateTime = DateTime.Now;
            audio.Sounds.Add(sound);
            asyncSoundStream = AsyncSoundStream.Create();
            asyncSoundStream.AddSound(sound);
            await PlaySound(sound);

        }

        public async Task ProcessSound(Coordinate coordinate)
        {
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            DateTime newDateTime = DateTime.Now; 
            if (audio.Sounds.Count > 0)
            {
                audio.Sounds.Last().Duration = (newDateTime.Subtract(dateTime)).TotalMilliseconds;
            }
            dateTime = newDateTime;
            audio.Sounds.Add(sound);
            asyncSoundStream.AddSound(sound);
            await PlaySound(sound);
        }

        public async Task EndSound()
        {
            DateTime newDateTime = DateTime.Now;
            audio.Sounds.Last().Duration = (newDateTime.Subtract(dateTime)).TotalMilliseconds;
            asyncSoundStream.Complete();
            StorageService.Audios.Add(audio);
        }

        public async Task PlaySound(Sound sound)
        {
            await audioPlayerService.Play(asyncSoundStream.GetSoundsAsync(cancellationTokenSource.Token), audio.WaveForm, cancellationTokenSource.Token);
        }
    }
}
