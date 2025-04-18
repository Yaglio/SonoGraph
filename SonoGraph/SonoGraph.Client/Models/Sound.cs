﻿namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents a sound with a frequency and amplitude.
    /// </summary>
    public struct Sound
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

        /// <summary>
        /// The duration of the sound in milliseconds.
        /// </summary>
        public double Duration { get; set; }

        public Sound(double frequency, double amplitude, double duration)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Duration = duration;
        }
    }
}
