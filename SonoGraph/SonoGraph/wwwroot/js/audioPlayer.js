var audioContext;
var oscillators = new Map();
var oscillatorIdCounter = 0;

window.initializeAudioPlayer = () => {
    audioContext = new (window.AudioContext || window.webkitAudioContext)();

    if (!audioContext) {
        console.error("AudioContext not available.");
        return;
    }

    console.log("AudioContext initialized.");
};

window.startAudio = (waveForm) => {
    if (!audioContext) {
        console.error("AudioPlayer not initialized");
        return;
    }

    const oscillator = new Oszillator(waveForm, audioContext);
    oscillatorIdCounter++;
    oscillators.set(oscillatorIdCounter, oscillator);

    return oscillatorIdCounter;
};

window.playAudio = (id, frequency, amplitude, duration) => {
    console.log("Playing Sound with:", frequency, amplitude, duration);
    const osc = oscillators.get(id);
    if (!osc) {
        console.error("Invalid oscillator id:", id);
        return;
    }

    osc.play(frequency, amplitude, duration);
};

window.stopAudio = (id) => {
    const osc = oscillators.get(id);
    if (!osc) {
        console.error("Invalid oscillator id:", id);
        return;
    }

    osc.stopAll();
    oscillators.delete(id);
};

window.stopAllAudio = () => {
    for (const [id, osc] of oscillators) {
        osc.stopAll();
    }
    oscillators.clear();
    console.log("All audio stopped.");
};


// Oszillator ist jetzt ein polyphoner Voice-Manager
class Oszillator {
    constructor(waveForm, audioContext) {
        this.audioContext = audioContext;
        this.waveForm = waveForm;
        this.voices = new Set(); // alle aktiven Mini-Oszillatoren
    }

    play(frequency, amplitude, duration) {
        const now = this.audioContext.currentTime;
        const osc = this.audioContext.createOscillator();
        const gain = this.audioContext.createGain();

        osc.type = this.waveForm;
        osc.frequency.setValueAtTime(frequency, now);

        gain.gain.setValueAtTime(0, now); // Start bei 0
        gain.gain.linearRampToValueAtTime(amplitude, now + 0.05); // sanft rein in 10ms

        gain.gain.setValueAtTime(amplitude, now + duration - 0.05); // kurz vor Ende
        gain.gain.linearRampToValueAtTime(0, now + duration); // weich ausblenden

        osc.connect(gain).connect(this.audioContext.destination);
        osc.start(now);
        osc.stop(now + duration);

        osc.onended = () => {
            osc.disconnect();
            gain.disconnect();
            this.voices.delete(osc);
        };

        this.voices.add(osc);
    }

    stopAll() {
        for (const osc of this.voices) {
            try {
                osc.stop();
                osc.disconnect();
            } catch { }
        }
        this.voices.clear();
    }
}
