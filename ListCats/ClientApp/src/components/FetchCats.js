import React, { Component, useState } from 'react';

export class FetchCats extends Component {
    static displayName = "Fetch Cat List";

    constructor(props) {
        super(props);
        this.state = { cats: [], loading: true, errorMessage: "" };

    }

    componentDidMount() {
        this.getCats();
    }

    static renderCatList(cats) {
        return (
            <div>
                <div>
                    <h3>Male</h3>
                    {cats["Male"].sort().map((group, idx) => {
                        return (
                            <ul key={idx}>
                                <li>{group}</li>
                            </ul>
                        )
                    })}
                </div>
                <div>
                    <h3>Female</h3>
                    {cats["Female"].sort().map((group, idx) => {
                        return (
                            <ul key={idx}>
                                <li>{group}</li>
                            </ul>
                        )
                    })}
                </div>
            </div>
        );
    }

    static renderError(errMsg) {
        return (            
            <h3 className="error"> {errMsg} </h3>
        );
    }
    render() {      
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : (this.state.errorMessage != "" ? FetchCats.renderError(this.state.errorMessage) : FetchCats.renderCatList(this.state.cats));

        return (
            <div>
                <h1 id="tabelLabel" >Cats</h1>
                <p>This is a list of cats.</p>
                {contents}
            </div>
        );
    }

    async getCats() {
        const response = await fetch('fetchcat');
        if (!response.ok) {
            const message = `An error occured. ${response.status}`;;
            this.setState({ cats: [], loading: false, errorMessage: message });
            throw new Error(message);
        }
        const data = await response.json();
        this.setState({ cats: data, loading: false, errorMessage: "" });
    }
}
