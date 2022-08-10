'use strict';
import * as du from "/js/dom_utils.js"

class TabCompleteInput extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            inputText: this.props.value,
            inputWidth: 20,
            suggestionText: "",
            suggestionWidth: 40
        };
        this.inputRef = React.createRef();
        this.suggestionRef = React.createRef();
        this.aspFor = this.props.aspFor;

        this.handleChange = this.handleChange.bind(this);
        this.handleInput = this.handleInput.bind(this);
        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.resize = this.resize.bind(this);
    }

    render() {
        return <div className="bgdb-tab-complete-input-group"
            onKeyDown={this.handleKeyDown}
            onClick={this.handleClick}>

            <input ref={this.inputRef}
                data-val="true"
                name={this.props.name}
                onChange={this.handleChange}
                onInput={this.handleInput}
                style={{ width: this.state.inputWidth + "px" }}
                value={this.state.inputText} /><span
                    ref={this.suggestionRef}
                    onClick={this.handleClick}
                    style={{ "width": this.state.suggestionWidth + "px" }}>{this.state.suggestionText}</span>
        </div>
    }

    componentDidMount() {
        this.resize();
    }

    handleInput(evt) {
        console.log("handleInput");
        this.setState({ inputText: evt.target.value }, this.updateSuggestion);
        this.updateSuggestion(evt.target.value);
    }

    handleChange(evt) {
        console.log("handleChange");
    }

    handleKeyDown(evt) {
        console.log("handleKeyDown");
        var code = evt.code || evt.which;
        if (code == '9' || code == "Tab") {
            evt.stopPropagation();
            evt.preventDefault();
            this.commitSuggestion();
        }
    }

    handleClick(evt) {
        console.log("handleClick");
        du.setCaretPositionToEnd(this.inputRef.current);
    }

    updateSuggestion() {
        console.log("updateSuggestion");

        var text = this.state.inputText;
        console.log(text);

        // Find suggestion
        var suggestion = this.findSuggestion(text);

        // Insert suggestion text
        this.setState({ suggestionText: suggestion }, this.resize);

    }

    commitSuggestion() {
        console.log("commitSuggestion");
        console.log(this.state.inputText);
        console.log(this.state.suggestionText);
        this.setState({
            inputText: this.state.inputText + this.state.suggestionText + this.props.sep + " ",
            suggestionText: ""
        }, this.resize);
    }

    findSuggestion(text) {
        console.log("findSuggestion");
        var suggestion = "";

        var inputValue = text;
        console.log(`Input: "${inputValue}"`);
        var inputList = inputValue.split(this.props.sep);
        inputList.forEach((value, index, array) => array[index] = value.trimStart());
        var lastInputValue = inputList[inputList.length - 1];
        console.log(`Last input value: "${lastInputValue}"`);

        if (lastInputValue.length < 1) {
            return suggestion;
        }

        // Find first matching suggestion from list
        var lastInputValueLower = lastInputValue.toLowerCase();
        for (var i = 0; i < this.props.suggestions.length; ++i) {
            if (this.props.suggestions[i].toLowerCase().startsWith(lastInputValueLower)) {
                suggestion = this.props.suggestions[i];
                break;
            }
        }
        console.log(`First suggestion: "${suggestion}"`);

        // Remove the part of the suggestion that is already input
        suggestion = suggestion.substring(lastInputValue.length);
        console.log(`Remaining suggestion text: "${suggestion}"`);

        return suggestion;
    }

    resize() {
        console.log("resize");
        console.log("input text", this.state.inputText);
        console.log(this.inputRef.current);
        var inputElementWidth = du.getTextWidth(this.state.inputText, du.getCanvasFont(this.inputRef.current));
        if (this.state.inputText.length == 0) {
            console.log("resizing to placeholder width");
            inputElementWidth = du.getTextWidth("Placeholder", du.getCanvasFont(this.inputRef.current));
        }
        var suggestionTextWidth = du.getTextWidth(this.suggestionText, du.getCanvasFont(this.suggestionRef.current));

        this.setState({
            inputWidth: inputElementWidth == 0 ? 1 : inputElementWidth,
            suggestionWidth: suggestionTextWidth
        });
    }
}

// export default function createTabCompleteInput(domContainer, suggestions, sep, aspFor) {
//     if (sep === undefined) {
//         sep = ",";
//     }
//     console.log("createTabCompleteInput");
//     const root = ReactDOM.createRoot(domContainer);
//     root.render(React.createElement(TabCompleteInput, {
//         suggestions: suggestions,
//         sep: sep,
//         aspFor: aspFor
//     }));
// }

// const domContainer = document.querySelector('.tab-complete');
// const root = ReactDOM.createRoot(domContainer);
// root.render(React.createElement(TabCompleteInput));

export { TabCompleteInput as default };
