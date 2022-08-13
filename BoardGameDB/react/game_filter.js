'use strict';

class SelectList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            value: props.value
        };
        this.options = new Set([""].concat(this.props.options));
        this.handleChange = this.handleChange.bind(this);
    }

    render() {
        var label = (this.props.label !== undefined) && <label className="input-group-text">{this.props.label}</label>;
        var options = Array.from(this.options).map(o => <option key={o} value={o}>{o}</option>);

        return <div className={this.props.className}>
            <div className="input-group ">
                {label}
                <select className="form-select bgdb-min"
                    data-val={this.props.dataVal}
                    name={this.props.name}
                    value={this.state.value}
                    onChange={this.handleChange}>{options}</select>
            </div>
        </div>
    }

    handleChange(evt) {
        var value = evt.target.value;
        this.setState({ value: value }, () => {
            if (this.props.onSelected !== undefined) {
                this.props.onSelected({ selectList: this, value: value });
            }
        });

    }
}


class VariableList extends React.Component {
    constructor(props) {
        super(props);

        this.sep = props.sep || ";";
        this.state = {
            value: props.value || "",
            entries: new Set(),
        };

        this.remove = this.remove.bind(this);
        this.handleSelected = this.handleSelected.bind(this);
    }

    render() {
        var index = 0;
        var entries = Array.from(this.state.entries).sort().map(e =>
            <this.Entry
                key={e}
                value={e}
                onClick={evt => this.remove(e)}
            ></this.Entry>);
        var label = (this.props.label !== undefined) && <label className="input-group-text">{this.props.label}</label>

        return <div className={this.props.className}>
            <div className="input-group">
                <input hidden
                    name={this.props.name}
                    data-val="true"
                    value={this.state.value} />
                {label}
                <div className="form-control bgdb-min">
                    <div>
                        <SelectList
                            value={""}
                            onSelected={this.handleSelected}
                            options={this.props.options}>
                        </SelectList>
                    </div>
                    <div>
                        {entries}
                    </div>
                </div>
            </div>
        </div>
    }

    componentDidMount() {
        this.setState({
            entries: new Set(this.props.value
                .split(this.sep)
                .map(v => v.trim())
                .filter(v => v != "")
                .sort())
        }, this.updateValue);
    }

    handleSelected(evt) {
        this.add(evt.value);
        evt.selectList.setState({ value: "" });
    }

    add(entry) {
        if (entry.length == 0) {
            return;
        }
        var newEntries = this.state.entries;
        newEntries.add(entry);
        this.setState({ entries: newEntries }, this.updateValue);
    }

    remove(entry) {
        var newEntries = this.state.entries;
        newEntries.delete(entry);
        this.setState({ entries: newEntries }, this.updateValue);
    }

    updateValue() {
        // Join entries into a single string
        this.setState({
            value: Array.from(this.state.entries).join(this.sep)
        });
    }

    Entry(props) {
        return <div className="bgdb-variable-list-entry form-control">
            <span>{props.value}</span>
            <span className="bi bi-x"
                style={{ float: "right" }}
                onClick={evt => props.onClick(evt, props.value)}></span>
        </div>
    }
}

class BasicInput extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            value: props.value
        }

        this.handleChange = this.handleChange.bind(this);
    }

    render() {
        var label = (this.props.label !== undefined) && <label className="input-group-text">{this.props.label}</label>
        return <div className="input-group mb-3">
            {label}
            <input className="form-control"
                type={this.props.type}
                name={this.props.name}
                value={this.state.value}
                data-val="true"
                onChange={this.handleChange} />
        </div>;
    }

    handleChange(evt) {
        this.setState({ value: evt.target.value });
    }
}


function GameFilter(props) {
    return <div className="card card-body">
        <form>

            <BasicInput
                label="Title"
                name={props.titleProps.name}
                value={props.titleProps.value}
                type="text"
            ></BasicInput>

            <SelectList
                className="mb-3"
                label="Complexity"
                name={props.complexityProps.name}
                dataVal="true"
                options={["", "Low", "Medium", "Medium/Heavy", "Heavy", "Extremely Heavy"]}
                value={props.complexityProps.value}
            ></SelectList>

            <BasicInput
                label="Player Count"
                name={props.playerCountProps.name}
                value={props.playerCountProps.value}
                type="number"></BasicInput>

            <SelectList
                className="mb-3"
                label="Play Time"
                name={props.playTimeProps.name}
                dataVal="true"
                options={["", "Less than 30 minutes", "30-60 minutes", "1-2 hours", "More than 2 hours"]}
                value={props.playTimeProps.value}
            ></SelectList>
            <div className="mb-3">

            </div>
            <VariableList
                className="mb-3"
                label="Mechanics"
                name={props.mechanicsProps.name}
                value={props.mechanicsProps.value}
                options={props.mechanicsProps.options}
            ></VariableList>


            <VariableList
                className="mb-3"
                label="Play Styles"
                name={props.playStyleProps.name}
                value={props.playStyleProps.value}
                options={props.playStyleProps.options}
            ></VariableList>

            <div className="input-group mb-3">
                <input className="form-control btn btn-outline-dark" type="submit" value="Filter" />
            </div>
        </form>
        <form>
            <button className="form-control btn btn-outline-secondary"
                formAction={props.clearProps.formAction}>Clear</button>
        </form>
    </div>
}

export {
    GameFilter as default
};