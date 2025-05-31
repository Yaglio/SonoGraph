using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class SoundService
    {
        private Audio? audio = null;
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

        public void ProcessSound(double frequency, double amplitude)
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            DateTime newDateTime = DateTime.Now;

            var mappedFrequency = minFrequency * Math.Pow(maxFrequency / minFrequency, frequency);

            Sound sound = new Sound(mappedFrequency, amplitude, 100.0);

            asyncSoundStream.AddSound(sound);

            if (newDateTime.Subtract(dateTime).TotalMilliseconds > 100)
            {
                audio.Sounds.Add(sound);
                dateTime = newDateTime;
            }
        }

        public void EndSound()
        {
            if (asyncSoundStream == null || audio == null)
            {
                throw new InvalidOperationException("Sound has not started");
            }
            asyncSoundStream.Complete();

            _audioEditorService.AddAudio(audio);
        }
    }
}
