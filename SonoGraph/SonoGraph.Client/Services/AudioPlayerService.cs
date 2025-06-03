using Microsoft.JSInterop;
using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    /// <summary>
    /// Provides methods for playing audio using JavaScript interop.
    /// </summary>
    public class AudioPlayerService
    {
        /// <summary>
        /// The JavaScript runtime used for interop calls.
        /// </summary>
        private readonly IJSRuntime _jSRuntime;

        /// <summary>
        /// The current audio playback session ID.
        /// </summary>
        private int? _currentId = null;

        /// <summary>
        /// Gets or sets the master volume for audio playback (0-100).
        /// </summary>
        public double MasterVolume { get; set; } = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayerService"/> class.
        /// </summary>
        /// <param name="jSRuntime">The JavaScript runtime for interop.</param>
        public AudioPlayerService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        /// <summary>
        /// Initializes the audio player by invoking the corresponding JavaScript function.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Initialize()
        {
            await _jSRuntime.InvokeVoidAsync("initializeAudioPlayer");
        }

        /// <summary>
        /// Plays a sequence of <see cref="Sound"/> objects using the specified wave form.
        /// </summary>
        /// <param name="sounds">An asynchronous sequence of <see cref="Sound"/> objects to play.</param>
        /// <param name="waveForm">The <see cref="WaveFormType"/> to use for playback.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Play(IAsyncEnumerable<Sound> sounds, WaveFormType waveForm, CancellationToken cancellationToken)
        {
            _currentId = await _jSRuntime.InvokeAsync<int>("startAudio", waveForm.Value);

            Console.WriteLine("Starting audio with ID: " + _currentId);

            try
            {
                await foreach (var sound in sounds.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await _jSRuntime.InvokeVoidAsync("playAudio", _currentId, sound.Frequency, sound.Amplitude * MasterVolume / 100);
                }
            }
            catch (OperationCanceledException)
            {
                // Playback was cancelled.
            }
            finally
            {
                await _jSRuntime.InvokeVoidAsync("stopAudio", _currentId);
            }
        }

        /// <summary>
        /// Plays a single <see cref="Sound"/> object using the specified wave form.
        /// </summary>
        /// <param name="sound">The <see cref="Sound"/> object to play.</param>
        /// <param name="waveForm">The <see cref="WaveFormType"/> to use for playback.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
        /// Plays an <see cref="Audio"/> object by streaming its sounds in sequence using the specified wave form.
        /// </summary>
        /// <param name="audio">The <see cref="Audio"/> object containing the sounds and wave form to play.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
