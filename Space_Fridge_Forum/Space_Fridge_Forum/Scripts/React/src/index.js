import ReactDOM from "react-dom";
import React from "react";
import Components from "./Constants/Components";
import ASPComponent from "./ASPComponent";
import NotFound from "./ASPComponents/NotFound";

const ServerState = window.ServerState;

class App extends React.Component {
    render() {
        const AspComponent = this.props.AspComponent;
        return (
            <div>
                {/* some global (i.e state/style providers) should go here */}
                {AspComponent.render()}
            </div>
        );
    }
}

// The keys are the App IDs
for(const AppId of Object.keys(ServerState)) {
    const Val = ServerState[AppId];
    const Content = Components.filter(x => Val.Component === x.key)[0];
    let AspComponent;
    if (Content == null) {
        AspComponent = new ASPComponent(-1, NotFound);
    } else {
        AspComponent = Content;
    }
    AspComponent.AppId = AppId;
    ReactDOM.render(<App AspComponent={AspComponent}/>, document.getElementById(AppId));
}