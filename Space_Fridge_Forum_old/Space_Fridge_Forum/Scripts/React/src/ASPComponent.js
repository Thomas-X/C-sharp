import React from "react";

export default class ASPComponent {
    key = -1;
    Component = null;
    AppId = "";

    constructor(key, Component) {

        this.key = key;
        this.Component = Component;
    }

    render() {
        const Component = this.Component;
        const obj = window.ServerState[this.AppId];
        return <Component {...obj !== null ? obj.state : {}}/>
    }
}