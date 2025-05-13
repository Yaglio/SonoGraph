using Microsoft.JSInterop;
using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class AudioPlayerService : IInitializableService
    {
        private readonly IJSRuntime jSRuntime;
        private int? currentId = null; // To store the current audio id
        public double MasterVolume { get; set; } = 50;

        public AudioPlayerService(IJSRuntime JSRuntime)
        {
            jSRuntime = JSRuntime;
        }

        /// <summary>
        /// Initializes the audio player with the js audio context.
        /// </summary>
        /// <param name="audioId"></param>
        /// <returns></returns>
        public async Task Initialize()
        {
            await jSRuntime.InvokeVoidAsync("initializeAudioPlayer");
        }

        /// <summary>
        /// Plays a stream of sounds with the specified wave form. 
        /// Use an <see cref="AsyncSoundStream"/> to dynamically add sounds.
        /// </summary>
        /// <param name="sounds"></param>
        /// <param name="waveForm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Play(IAsyncEnumerable<Sound> sounds, WaveFormType waveForm, CancellationToken cancellationToken)
        {
            currentId = await jSRuntime.InvokeAsync<int>("startAudio", waveForm.Value);

            try
            {
                await foreach (var sound in sounds.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await jSRuntime.InvokeVoidAsync("playAudio", currentId, sound.Frequency, sound.Amplitude * MasterVolume / 100, sound.Duration / 1000);
                }
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                await jSRuntime.InvokeVoidAsync("stopAudio", currentId);
            }
        }

        /// <summary>
        /// Plays a sound with the specified frequency, amplitude, and wave form for a duration in ms.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="waveForm"></param>
        /// <returns></returns>
        public async Task Play(Sound sound, WaveFormType waveForm, CancellationToken cancellationToken)
        {
            async IAsyncEnumerable<Sound> StreamSound()
            {
                yield return sound;
                await Task.Delay(TimeSpan.FromMilliseconds(sound.Duration), cancellationToken);
            }


            await Play(StreamSound(), waveForm, cancellationToken);
        }

        /// <summary>
        /// Plays a audio.
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Play(Audio audio, CancellationToken cancellationToken)
        {
            async IAsyncEnumerable<Sound> StreamSound()
            {
                foreach (var sound in audio.Sounds)
                {
                    yield return sound;
                    await Task.Delay(TimeSpan.FromMilliseconds(sound.Duration), cancellationToken);
                }

            }

            await Play(StreamSound(), audio.WaveForm, cancellationToken);
        }
    }
}
