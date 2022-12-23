// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function colorStringToRgba(s) {
    const reLongHex = /#([a-f0-9]{2})([a-f0-9]{2})([a-f0-9]{2})([a-f0-9]{2})/i;
    const reLongHexNoAlpha = /#([a-f0-9]{2})([a-f0-9]{2})([a-f0-9]{2})/i;
    const reShortHex = /#([a-f0-9])([a-f0-9])([a-f0-9])([a-f0-9])/i;
    const reShortHexNoAlpha = /#([a-f0-9])([a-f0-9])([a-f0-9])/i;
    const reRgba = /rgba\s*\(\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+(\.[0-9]*)?)\s*\)/;
    const reRgb = /rgb\s*\(\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*\)/;

    var matches;
    var r = g = b = 0;
    var a = 1.0;

    if (matches = s.match(reLongHex)) {
        console.log(`"${s} is long hex`);
        var [_, rs, gs, bs, as] = matches;
        r = parseInt(rs, 16);
        g = parseInt(gs, 16); 
        b = parseInt(bs, 16);
        a = parseInt(as, 16) / 255.0;
    }
    else if (matches = s.match(reLongHexNoAlpha)) {
        console.log(`"${s} is long hex (no alpha)`);
        var [_, rs, gs, bs] = matches;
        r = parseInt(rs, 16);
        g = parseInt(gs, 16); 
        b = parseInt(bs, 16);
    }
    else if (matches = s.match(reShortHex)) {
        var [_, rs, gs, bs, as] = matches;
        console.log(`"${s}" is short hex`);
        r = parseInt(rs, 16) / 15.0 * 255.0;
        g = parseInt(gs, 16) / 15.0 * 255.0; 
        b = parseInt(bs, 16) / 15.0 * 255.0;
        a = parseInt(as, 16) / 15.0;
    }
    else if (matches = s.match(reShortHexNoAlpha)) {
        console.log(`"${s}" is short hex (no alpha)`);
        var [_, rs, gs, bs] = matches;
        r = parseInt(rs, 16) / 15.0 * 255.0;
        g = parseInt(gs, 16) / 15.0 * 255.0; 
        b = parseInt(bs, 16) / 15.0 * 255.0;
    }
    else if (matches = s.match(reRgba)) {
        console.log(`"${s}" is rgba`);
        var [_, rs, gs, bs, as] = matches;
        r = parseFloat(rs);
        g = parseFloat(gs); 
        b = parseFloat(bs);
        a = parseFloat(as);
    }
    else if (matches = s.match(reRgb)) {
        console.log(`"${s}" is rgb`);
        var [_, rs, gs, bs] = matches;
        r = parseFloat(rs);
        g = parseFloat(gs); 
        b = parseFloat(bs);
    }
    else {
        console.log(`Could not convert string "${s}" to RGBA`);
    }
    return `rgba(${r}, ${g}, ${b}, ${a})`;
}

function setTabComplete(inputId, suggestionTextId, items, sep) {
    console.log("setTabComplete");
    console.log(`inputId: ${inputId}`);
    console.log(`suggestionTextId: ${suggestionTextId}`);

    var inputElement = document.getElementById(inputId);
    var suggestionTextElement = document.getElementById(suggestionTextId);


    inputElement.addEventListener("input", evt => {
        console.log("input");
        console.log(evt);
        showSuggestion(inputElement, suggestionTextElement, items, sep);
        resizeSuggestion(inputElement, suggestionTextElement);
    });

    inputElement.parentElement.addEventListener("click", () => setCaretPositionToEnd(inputElement));
    suggestionTextElement.addEventListener("click", () => setCaretPositionToEnd(inputElement));


    $("body").find("#" + inputId).keydown(evt => {
        var code = evt.code || evt.which;
        // var inputElement = evt.currentTarget;
        if (code == '9' || code == "Tab") {
            completeSuggestion(inputElement, suggestionTextElement, items, sep)
            resizeSuggestion(inputElement, suggestionTextElement);
            return false;
        }
    });

    resizeSuggestion(inputElement, suggestionTextElement);

}

function findSuggestion(inputText, items, sep) {
    console.log("findSuggestion");
    var suggestion = "";
    if (sep === undefined) {
        sep = ",";
    }

    var inputValue = inputText;
    console.log(`Input: "${inputValue}"`);
    var inputList = inputValue.split(sep);
    inputList.forEach((value, index, array) => array[index] = value.trimLeft());
    var lastInputValue = inputList[inputList.length - 1];
    console.log(`Last input value: "${lastInputValue}"`);

    if (lastInputValue.length < 1) {
        return suggestion;
    }

    // Find first matching suggestion from list
    var lastInputValueLower = lastInputValue.toLowerCase();
    console.log("Items:");
    console.log(items);
    for (var i = 0; i < items.length; ++i) {
        if (items[i].toLowerCase().startsWith(lastInputValueLower)) {
            suggestion = items[i];
            break;
        }
    }
    console.log(`First suggestion: "${suggestion}"`);

    // Remove the part of the suggestion that is already input
    suggestion = suggestion.substring(lastInputValue.length);
    console.log(`Remaining suggestion text: "${suggestion}"`);

    return suggestion;
}

function showSuggestion(inputElement, suggestionTextElement, items, sep) {
    console.log("showSuggestion");

    // Find suggestion
    var suggestion = findSuggestion(inputElement.value, items, sep);

    // Insert suggestion text
    suggestionTextElement.innerHTML = suggestion;
}

function completeSuggestion(inputElement, suggestionTextElement, items, sep) {
    console.log("completeSuggestion");
    console.log(inputElement.value);
    console.log(suggestionTextElement.innerHTML);
    inputElement.value += suggestionTextElement.innerHTML + sep + " ";
    suggestionTextElement.innerHTML = "";
}

function resizeSuggestion(inputElement, suggestionTextElement) {
    var inputElementWidth = getTextWidth(inputElement.value, getCanvasFont(inputElement));
    if (inputElement.value.length == 0) {
        inputElementWidth = getTextWidth(inputElement.placeholder, getCanvasFont(inputElement));
    }
    var suggestionTextWidth = getTextWidth(suggestionTextElement.innerHTML, getCanvasFont(suggestionTextElement));

    inputElementWidth = inputElementWidth == 0 ? "1px" : inputElementWidth;
    inputElement.style.width = inputElementWidth + "px";
    suggestionTextElement.style.width = suggestionTextWidth + "px";
}



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


/**
 * Sets up a filter list where the list elements whose text does not contain 
 * the input text are hidden.
 * 
 * @param {*} inputId The ID of the input element
 * @param {*} nodes An array-like list of nodes to filter
 * @param {*} nodeTextGetter A function to call on each node to retreive its text
 */
function setFilterList(inputId, nodes, nodeTextGetter) {
    var inputElement = document.getElementById(inputId);
    inputElement.addEventListener("keyup", () => filterList(inputElement.value, nodes, nodeTextGetter));
}

/**
 * Performs filtering on a list (see setFilterList).
 * 
 * @param {*} text The text to use for filtering
 * @param {*} nodes An array-like list of nodes to filter
 * @param {*} nodeTextGetter A function to call on each node to retreive its text
 */
function filterList(text, nodes, nodeTextGetter) {
    var text = text.toLowerCase();
    Array.from(nodes).forEach(child => {
        var childText = nodeTextGetter(child).toLowerCase();
        if (childText.includes(text)) {
            child.classList.remove("bgdb-hidden");
        }
        else {
            child.classList.add("bgdb-hidden");
        }
    });
}