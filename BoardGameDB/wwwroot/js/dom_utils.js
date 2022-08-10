
/**
 * https://stackoverflow.com/questions/118241/calculate-text-width-with-javascript
 */
 function getTextWidth(text, font) {
    // re-use canvas object for better performance
    const canvas = getTextWidth.canvas || (getTextWidth.canvas = document.createElement("canvas"));
    const context = canvas.getContext("2d");
    context.font = font;
    const metrics = context.measureText(text);
    return metrics.width;
}

/**
 * https://stackoverflow.com/questions/118241/calculate-text-width-with-javascript
 */
function getCssStyle(element, prop) {
    return window.getComputedStyle(element, null).getPropertyValue(prop);
}

/**
 * https://stackoverflow.com/questions/118241/calculate-text-width-with-javascript
 */
function getCanvasFont(el = document.body) {
    const fontWeight = getCssStyle(el, 'font-weight') || 'normal';
    const fontSize = getCssStyle(el, 'font-size') || '16px';
    const fontFamily = getCssStyle(el, 'font-family') || 'Times New Roman';

    return `${fontWeight} ${fontSize} ${fontFamily}`;
}

// https://www.vishalon.net/blog/javascript-getting-and-setting-caret-position-in-textarea
function setCaretPosition(ctrl, start, end) {
    // IE >= 9 and other browsers
    if (ctrl.setSelectionRange) {
        ctrl.focus();
        ctrl.setSelectionRange(start, end);
    }
    // IE < 9 
    else if (ctrl.createTextRange) {
        var range = ctrl.createTextRange();
        range.collapse(true);
        range.moveEnd('character', end);
        range.moveStart('character', start);
        range.select();
    }
}

function setCaretPositionToEnd(ctrl) {
    var position = 0;
    if (ctrl.value) {
        position = ctrl.value.length;
    }
    else {
        position = ctrl.innerHTML.length;
    }
    setCaretPosition(ctrl, position, position);
}

export {
    getTextWidth,
    getCssStyle,
    getCanvasFont,
    setCaretPosition,
    setCaretPositionToEnd
}