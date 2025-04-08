var audioContext;
var oscillators = new Map();
var oscillatorIdCounter = 0;

window.initializeAudioPlayer = () => {

    audioContext = new (window.AudioContext || window.webkitAudioContext)();

    if (!audioContext) {
        console.error("AudioContext element not found:", audioId);
        return;
    }

    console.log("AudioContext initialized successfully.");
}

window.startAudio = (waveForm) => {
    if (!audioContext) {
        console.error("AudioPlayer not initialized");
        return;
    }

    const oscillator = new Oszillator(waveForm, audioContext);
    oscillator.start();

    oscillatorIdCounter++;

    oscillators.set(oscillatorIdCounter, oscillator)

    return oscillatorIdCounter;
}

window.playAudio = (id, frequency, amplitude) => {
    if (!oscillators.has(id)) {
        console.error("Invalid oscillator id:", id);
        return;
    }

   
    oscillators.get(id).play(frequency, amplitude);
}

window.stopAudio = (id) => {
    if (!oscillators.has(id)) {
        console.error("Invalid oscillator id:", id);
        return;
    }

    oscillators.get(id).stop();

    oscillators.delete(id);
}

class Oszillator {
    constructor(waveForm, audioContext) {
        this.audioContext = audioContext;
        this.waveForm = waveForm;
        this.oscillator = null;
        this.gainNode = null;
    }

    start() {
        if (!this.audioContext) {
            console.error("AudioPlayer not initialized");
            return;
        }
  
        this.oscillator = this.audioContext.createOscillator();
        this.gainNode = this.audioContext.createGain();
        this.oscillator.type = this.waveForm;

        this.oscillator.connect(this.gainNode);
        this.gainNode.connect(this.audioContext.destination);

        this.oscillator.start();
    }


    play(frequency, amplitude) {
        if (!this.audioContext || !this.oscillator || !this.gainNode) {
            console.error("Oszillator not initialized");
            return;
        }
        this.oscillator.frequency.setValueAtTime(frequency, this.audioContext.currentTime); // value in hertz (Hz)
        this.gainNode.gain.setValueAtTime(amplitude, this.audioContext.currentTime); // amplitude (volume)
    }

    stop() {
        if (!this.audioContext || !this.oscillator || !this.gainNode) {
            console.error("Oszillator not initialized");
            return;
        }
        this.oscillator.stop();
        this.oscillator.disconnect();
        this.gainNode.disconnect();
        this.oscillator = null;
        this.gainNode = null;
    }
}

