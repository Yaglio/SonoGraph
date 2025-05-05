using System.Security.Cryptography.X509Certificates;
using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class SoundService
    {
        Audio audio {  get; set; }
        DateTime DateTime { get; set; }
        CancellationTokenSource CancellationTokenSource { get; set; }
        AudioPlayerService audioPlayerService { get; set; }
        public SoundService( WaveFormType WaveForm) {
            audio = new Audio(WaveForm, new List<Sound> { });
            DateTime = DateTime.Now;
            CancellationTokenSource = new CancellationTokenSource();
            //audioPlayerService = new AudioPlayerService();
        }

        public void processSound(Coordinate coordinate1, Coordinate coordinate2)
        {
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
            playSound(sound);
        }

        public void playSound(Sound sound)
        {
            audioPlayerService.Play(sound, audio.WaveForm, CancellationTokenSource.Token);
        }
    }
}
