import React, {Component} from 'react';
import './Styling/App.css';
import {BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Routes from "./Constants/Routes";

class App extends Component {

    render() {
        return (
            <Router>
                <div>
                    {/* Some navigation here*/}
                    {/*<nav>*/}
                        {/*<a href={"https://placekitten.com/640/380"}>*/}
                            {/*see a kitty*/}
                        {/*</a>*/}
                    {/*</nav>*/}
                    <Switch>
                        {Object.keys(Routes).map((x, i) => <Route key={i} {...Routes[x]}/>)}
                    </Switch>
                </div>
            </Router>
        );
    }
}

export default App;
