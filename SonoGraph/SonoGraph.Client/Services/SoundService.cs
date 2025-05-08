using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using SonoGraph.Client.Models;
using SonoGraph.Client.Pages;
using SonoGraph.Client.Services;

namespace SonoGraph.Client.Services
{
    public class SoundService
    {
        static Audio audio {  get; set; }
        static DateTime DateTime { get; set; }
        static CancellationTokenSource CancellationTokenSource { get; set; }
        public SoundService( WaveFormType WaveForm) {
            audio = new Audio(WaveForm, new List<Sound> { });
            DateTime = DateTime.Now;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public static async Task ProcessSound(Coordinate coordinate1)
        {
            AudioPlayerService.Initialize();
            Sound sound = new Sound(coordinate1.Y, coordinate1.X, 100.0);
            DateTime newDateTime = DateTime.Now;
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

        public static void PlaySound(Sound sound)
        {
            audioPlayerService.Play(sound, audio.WaveForm, CancellationTokenSource.Token);
        }
    }
}
