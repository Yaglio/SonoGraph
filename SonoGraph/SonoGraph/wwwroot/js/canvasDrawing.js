﻿var canvas = null;
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
    context.lineWidth = 4;
    context.lineCap = 'round';
    context.lineJoin = 'round';
    console.log("Canvas initialized successfully.");

    var rect = canvas.getBoundingClientRect();

    return { x: rect.left, y: rect.top };
};

window.startDrawing = (x, y, color) => {
    if (!context) {
        console.error("Drawing context is not initialized.");
        return;
    }

    context.strokeStyle = color;

    context.beginPath();
    context.moveTo(x, y);
};

window.draw = (x, y) => {
    if (!context) return;

    context.lineTo(x, y);
    context.stroke();
};

window.stopDrawing = () => {
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