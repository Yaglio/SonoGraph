using System.Security.Cryptography.X509Certificates;

namespace SonoGraph.Client.Models
{
    public class SoundService
    {
        Audio audio {  get; set; }
        public SoundService( WaveFormType WaveForm) {
            audio = new Audio(WaveForm, new List<Sound> { });

        }

        public void processSound(Coordinate coordinate1, Coordinate coordinate2)
        {
          Sound sound = new Sound(coordinate1.Y, coordinate1.X, 0.0);
          playSound(sound);
          audio.Sounds.Add(sound);
        }

        public void playSound(Sound sound)
        {
            //AudioPlayer.PlayAudio
        }
    }
}
