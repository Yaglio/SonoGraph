var canvas = null;
var context = null;

window.initializeDrawing = (canvasId) => {
    canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error("Canvas element not found:", canvasId);
        return;
    }

    context = canvas.getContext('2d');
    if (!context) {
        console.error("Canvas context could not be initialized.");
        return;
    }

    context.strokeStyle = 'black';
    context.lineWidth = 2;
    context.lineCap = 'round';
    console.log("Canvas initialized successfully.");
};

window.startDrawing = (x, y) => {
    if (!context) {
        console.error("Drawing context is not initialized.");
        return;
    }

    console.log("JS start draw");

    context.beginPath();
    context.moveTo(x, y);
};

window.draw = (x, y) => {
    if (!context) return;

    console.log("JS draw at:", x, y);
    context.lineTo(x, y);
    context.stroke();
};

window.stopDrawing = () => {
    console.log("JS stop draw");
    context.beginPath();
};

window.clearCanvas = () => {
    if (!context || !canvas) {
        console.error("Canvas or context is not initialized.");
        return;
    }

    console.log("JS clear canvas");
    context.clearRect(0, 0, canvas.width, canvas.height);
};