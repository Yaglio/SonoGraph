﻿using Microsoft.JSInterop;
using SonoGraph.Client.Models;

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
        /// Plays a stream of sounds with the specified wave form.
        /// </summary>
        /// <param name="sounds"></param>
        /// <param name="waveForm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Play(IAsyncEnumerable<Sound> sounds, WaveFormType waveForm, CancellationToken cancellationToken)
        {
            var id = await jSRuntime.InvokeAsync<int>("startAudio", waveForm.Value);

            try
            {
                await foreach (var sound in sounds.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await jSRuntime.InvokeVoidAsync("playAudio", id, sound.Frequency, sound.Amplitude);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            finally
            {
                await jSRuntime.InvokeVoidAsync("stopAudio", id);
            }
        }

        /// <summary>
        /// Plays a sound with the specified frequency, amplitude, and wave form for a duration in ms.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="duration"></param>
        /// <param name="waveForm"></param>
        /// <returns></returns>
        public async Task Play(Sound sound, int duration, WaveFormType waveForm, CancellationToken cancellationToken)
        {
            async IAsyncEnumerable<Sound> StreamSound()
            {
                yield return sound;
                await Task.Delay(duration, cancellationToken);
            }


            await Play(StreamSound(),waveForm ,cancellationToken);
        }

        /// <summary>
        /// Plays a audio.
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Play(Audio audio, CancellationToken cancellationToken)
        {
            var duration = TimeSpan.FromSeconds(1 / audio.SamplingRate);

            async IAsyncEnumerable<Sound> StreamSound()
            {
                foreach (var sound in audio.Sounds)
                {
                    yield return sound;
                    await Task.Delay(duration, cancellationToken);
                }

            }

            await Play(StreamSound(), audio.WaveForm, cancellationToken);    
        }

    }
}
