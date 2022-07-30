// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function setTabComplete(inputId, suggestionTextId, items) {
    console.log("setTabComplete");
    console.log(`inputId: ${inputId}`);
    console.log(`suggestionTextId: ${suggestionTextId}`);
    
    var inputElement = document.getElementById(inputId);
    var suggestionTextElement = document.getElementById(suggestionTextId);

    
    inputElement.addEventListener("input", evt => {
        console.log("input");
        console.log(evt);
        showSuggestion(inputElement, suggestionTextElement, items);
        resizeSuggestion(inputElement, suggestionTextElement);
    });
    
    inputElement.parentElement.addEventListener("click", () => inputElement.focus());
    suggestionTextElement.addEventListener("click", () => inputElement.focus());
    
    
    $("body").find("#" + inputId).keydown(evt => {
        var code = evt.code || evt.which;
        var inputElement = evt.currentTarget;
        if (code == '9' || code == "Tab") {
            completeSuggestion(inputElement, suggestionTextElement, items)
            resizeSuggestion(inputElement, suggestionTextElement);
            return false;
        }
    });

    resizeSuggestion(inputElement, suggestionTextElement);
}

function findSuggestion(inputText, items) {
    console.log("findSuggestion");
    var suggestion = "";

    var inputValue = inputText;
    console.log(`Input: "${inputValue}"`);
    var inputList = inputValue.split(",");
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

function showSuggestion(inputElement, suggestionTextElement, items) {
    console.log("showSuggestion");

    // Find suggestion
    var suggestion = findSuggestion(inputElement.value, items);

    // Insert suggestion text
    suggestionTextElement.innerHTML = suggestion;
}

function completeSuggestion(inputElement, suggestionTextElement, items) {
    console.log("completeSuggestion");
    console.log(inputElement.value);
    console.log(suggestionTextElement.innerHTML);
    inputElement.value += suggestionTextElement.innerHTML;
    suggestionTextElement.innerHTML = "";
}

function resizeSuggestion(inputElement, suggestionTextElement) {
    var inputElementWidth = getTextWidth(inputElement.value, getCanvasFont(inputElement));
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