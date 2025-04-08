namespace SonoGraph.Client.Models
{
    public class SoundService
    {
        public Audio Audio { get; set; }
        public SoundService(WaveForm WaveForm)
        {
            Audio = new Audio();
        }

        public void processSound(double x, double y, TimeOnly Time)
        {
            Sound sound = new Sound(x, y, Time);
            //SoundOutput.playSound(sound);
            Audio.Sounds.Add(sound);
            Audio.Duration = Audio.Sounds.First().Time - Time;
        }

    }
}
