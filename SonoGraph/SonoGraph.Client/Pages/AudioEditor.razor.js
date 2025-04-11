var slider = document.getElementById("myRange");
var output = document.getElementById("currVal");
output.textContent = slider.value; // Display the default slider value

// Update the current slider value (each time you drag the slider handle)
slider.oninput = function () {
    output.textContent = this.value;
}