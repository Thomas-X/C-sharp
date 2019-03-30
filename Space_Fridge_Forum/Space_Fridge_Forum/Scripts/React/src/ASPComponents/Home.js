import React, {Component} from 'react';

class Home extends Component {
    render() {
        return (
            <h1>
                This is a message from C# backend: {this.props.message}
            </h1>
        );
    }
}

export default Home;