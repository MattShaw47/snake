
using System.Reflection.Metadata;
using System;

var framecount = 0;
var animationFrameId = 0;
var animating = false;

export function ToggleAnimation(on) {
    console.log("N: Toggle Animation " + on);

    animating = on;
    if (on) {
        window.requestAnimationFrame(AnimationLoopJS);
    }
    else {
        window.cancelAnimationFrame(animationFrameId);
    }

}

/**
 * This code tells the C# side to draw the scene.
 * It then "sleeps" and recalls itself X frames a second
 * where X is usually 60, or whatever the browser is optimized
 * for.  
 * 
 * Additionally, it only calls this method if the windows is
 * showing. If you switch to a different tab, the animation stops.
 * 
 * @param {any} timeStamp
 */
function AnimationLoopJS(timeStamp) {
    framecount++;

    DotNetSide.invokeMethodAsync('Draw', timeStamp);
    if (animating) {
        animationFrameId = window.requestAnimationFrame(AnimationLoopJS);
    }
}

/**
 * This is a resize event handler that allows the screen
 * to be resized and gives C# the opportunity to do something
 * about it (if you wish);
 */
function resizeCanvasToFitWindow() {
    var holder = document.getElementById('myCanvas');
    var canvas = holder.querySelector('canvas');
    if (canvas) {
        var sideCar = document.getElementsByClassName('sidebar');
        var main = document.getElementsByTagName('main');

        //var width = window.innerWidth - sideCar[0].getBoundingClientRect().width;
        let width = document.getElementsByTagName('main')[0].offsetWidth;
        width = Math.min(width - 100, 1000);
        canvas.width = width;

        DotNetSide.invokeMethodAsync('ResizeInBlazor', width, width);
    }
}

/**
 * Setup the JavaScript side so:
 * 1) It knows about the C# side --> DotNetSide
 * 2) It has a resize window handler
 * 
 * @param {any} DotNetSide - the C# instance from the razor code.
 */
export function initJS(DotNetSide) {
    window.DotNetSide = DotNetSide;
    window.addEventListener("resize", resizeCanvasToFitWindow);
    resizeCanvasToFitWindow();
}

document.addEventListener('keydown', function (event) {
    const key = event.key.toLowerCase();

    const validKeys = ["w", "a", "s", "d", "arrowup", "arrowdown", "arrowleft", "arrowright"];

    // Check if the key is valid
    if (validKeys.includes(key)) {
        // Send the key to the C# method
        DotNetSide.invokeMethodAsync('SendMessage', key)
            .then(() => console.log(`Key sent to C#: ${key}`))
            .catch(err => console.error(`Error sending key to C#: ${err}`));
    }
    else {
        console.log(`Ignored invalid key: ${key}`);
    }
});

/**
 * Stop the animation when we leave the page.
 */
window.addEventListener("unload", () => {
    console.log("Networked JS: leaving page.");
    animating = false;
});