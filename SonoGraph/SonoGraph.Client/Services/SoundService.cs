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
        DateTime DateTime { get; set; }
        CancellationTokenSource CancellationTokenSource { get; set; }
        AudioPlayerService AudioPlayerService { get; set; }
        public SoundService (AudioPlayerService Service) {
            AudioPlayerService = Service;
            audio = new Audio(WaveFormType.Sine, new List<Sound> { });
            DateTime = DateTime.Now;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartSound(Coordinate coordinate, WaveFormType waveForm)
        {
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            DateTime = DateTime.Now;

        }

        public async Task ProcessSound(Coordinate coordinate)
        {
            Sound sound = new Sound(coordinate.Y, coordinate.X, 100.0);
            if (audio.Sounds.Count > 0)
            {
                audio.Sounds.Last().Duration = (newDateTime.Subtract(DateTime)).TotalMilliseconds;
                CancellationTokenSource.Cancel();
                CancellationTokenSource = new CancellationTokenSource();
            }
            DateTime = newDateTime;
            audio.Sounds.Add(sound);
            //playSound(sound);
        }

        public async Task EndSound()
        {

        }

        public void PlaySound(Sound sound)
        {
            AudioPlayerService.Play(sound, audio.WaveForm, CancellationTokenSource.Token);
        }
    }
}
