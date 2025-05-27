window.audioContext = null;
window.oscillators = new Map();
window.oscillatorIdCounter = 0;

window.initializeAudioPlayer = () => {
    window.audioContext = new (window.AudioContext || window.webkitAudioContext)();

    if (!window.audioContext) {
        console.error("AudioContext not available.");
        return;
    }

    console.log("AudioContext initialized.");
};

class PersistentOscillator {
    constructor(waveForm, audioContext) {
        this.audioContext = audioContext;

        this.oscillator = audioContext.createOscillator();
        this.gainNode = audioContext.createGain();

        this.oscillator.type = waveForm;
        this.oscillator.connect(this.gainNode);
        this.gainNode.connect(audioContext.destination);

        this.gainNode.gain.setValueAtTime(0, audioContext.currentTime); // start muted

        this.oscillator.start();
    }

    play(frequency, amplitude) {
        const now = this.audioContext.currentTime;

        // Update frequency immediately
        this.oscillator.frequency.setValueAtTime(frequency, now);

        this.gainNode.gain.cancelScheduledValues(now);
        this.gainNode.gain.setValueAtTime(amplitude, now);

    }

    stop() {
        // Stop oscillator after fade out
        this.oscillator.stop();

        // Disconnect after stopping
        this.oscillator.onended = () => {
            this.oscillator.disconnect();
            this.gainNode.disconnect();
        };
    }
}

window.startAudio = (waveForm) => {
    if (!window.audioContext) {
        console.error("AudioPlayer not initialized");
        return null;
    }

    const osc = new PersistentOscillator(waveForm, window.audioContext);
    window.oscillatorIdCounter++;
    window.oscillators.set(window.oscillatorIdCounter, osc);

    return window.oscillatorIdCounter;
};

window.playAudio = (id, frequency, amplitude) => {
    const osc = window.oscillators.get(id);
    if (!osc) {
        console.error("Invalid oscillator id:", id);
        return;
    }

    console.log(`Playing audio on oscillator ${id} with frequency ${frequency} and amplitude ${amplitude}`);

    osc.play(frequency, amplitude);
};

window.stopAudio = (id) => {
    const osc = window.oscillators.get(id);
    if (!osc) {
        console.error("Invalid oscillator id:", id);
        return;
    }

    osc.stop();
    window.oscillators.delete(id);
};

window.stopAllAudio = () => {
    for (const osc of window.oscillators.values()) {
        osc.stop();
    }
    window.oscillators.clear();
    console.log("All audio stopped.");
};
