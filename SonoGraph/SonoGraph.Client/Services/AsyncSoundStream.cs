using SonoGraph.Client.Models;
using System.Threading.Channels;

namespace SonoGraph.Client.Services
{

    /// <summary>
    /// Provides an asynchronous stream for <see cref="Sound"/> objects using a channel.
    /// </summary>
    public class AsyncSoundStream
    {
        /// <summary>
        /// The underlying channel used for streaming <see cref="Sound"/> objects.
        /// </summary>
        private readonly Channel<Sound> _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSoundStream"/> class with the specified channel.
        /// </summary>
        /// <param name="channel">The channel to use for streaming sounds.</param>
        private AsyncSoundStream(Channel<Sound> channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// Creates a new <see cref="AsyncSoundStream"/> instance with an unbounded channel.
        /// </summary>
        /// <returns>A new <see cref="AsyncSoundStream"/> instance.</returns>
        public static AsyncSoundStream Create()
        {
            var channel = Channel.CreateUnbounded<Sound>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true,
                AllowSynchronousContinuations = false
            });

            return new AsyncSoundStream(channel);
        }

        /// <summary>
        /// Gets an asynchronous enumerable of <see cref="Sound"/> objects from the stream.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for sounds.</param>
        /// <returns>An <see cref="IAsyncEnumerable{Sound}"/> representing the stream of sounds.</returns>
        public IAsyncEnumerable<Sound> GetSoundsAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }

        /// <summary>
        /// Adds a <see cref="Sound"/> object to the stream.
        /// </summary>
        /// <param name="sound">The <see cref="Sound"/> to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if the channel is full.</exception>
        public void AddSound(Sound sound)
        {
            if (!_channel.Writer.TryWrite(sound))
            {
                throw new InvalidOperationException("Channel is full");
            }
        }

        /// <summary>
        /// Marks the stream as complete, indicating that no more sounds will be added.
        /// </summary>
        public void Complete()
        {
            _channel.Writer.Complete();
        }
    }
}
