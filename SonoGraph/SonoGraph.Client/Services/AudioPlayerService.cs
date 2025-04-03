using Microsoft.JSInterop;
using SonoGraph.Client.Models;
using System.Threading;

namespace SonoGraph.Client.Services
{
    public class AudioPlayerService
    {
        private readonly IJSRuntime jSRuntime;

        public AudioPlayerService(IJSRuntime JSRuntime)
        {
            jSRuntime = JSRuntime;
        }

        public async Task Initialize(string audioId)
        {
            await jSRuntime.InvokeVoidAsync("initializeAudioPlayer");
        }

        /// <summary>
        /// Plays a sound with the specified frequency, amplitude, and wave form for a duration in ms.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="duration"></param>
        /// <param name="waveForm"></param>
        /// <returns></returns>
        public async Task Play(Sound sound, int duration, WaveFormType waveForm)
        {
            var id = await jSRuntime.InvokeAsync<int>("startAudio", waveForm.Value);

            await jSRuntime.InvokeVoidAsync("playAudio",id, sound.Frequency, sound.Amplitude);

            await Task.Delay(TimeSpan.FromMilliseconds(duration));

            await jSRuntime.InvokeVoidAsync("stopAudio", id);
        }

        public async Task Play(Audio audio, CancellationToken cancellationToken)
        {
            var duration = 1 / audio.SamplingRate;

            var id = await jSRuntime.InvokeAsync<int>("startAudio", audio.WaveForm.Value);

            foreach (var sound in audio.Sounds)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await jSRuntime.InvokeVoidAsync("playAudio",id, sound.Frequency, sound.Amplitude);

                await Task.Delay(TimeSpan.FromSeconds(duration), cancellationToken);
            }

            await jSRuntime.InvokeVoidAsync("stopAudio", id);
        }
    }
}
