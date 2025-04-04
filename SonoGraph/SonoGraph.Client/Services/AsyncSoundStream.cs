using SonoGraph.Client.Models;
using System.Threading.Channels;

namespace SonoGraph.Client.Services
{
    public class AsyncSoundStream
    {
        private readonly Channel<Sound> _channel;

        private AsyncSoundStream(Channel<Sound> channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// Creates a new instance of the AsyncSoundStream class.
        /// </summary>
        /// <returns></returns>
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
        /// Asynchronously reads sounds from the channel.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<Sound> GetSoundsAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }

        /// <summary>
        /// Adds a sound to the channel.
        /// </summary>
        /// <param name="sound"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddSound(Sound sound)
        {
            if (!_channel.Writer.TryWrite(sound))
            {
                throw new InvalidOperationException("Channel is full");
            }
        }

        /// <summary>
        /// Completes the channel, indicating that no more sounds will be added.
        /// </summary>
        public void Complete()
        {
            _channel.Writer.Complete();
        }
    }
}
