import React, {Component} from 'react';

class NotFound extends Component {

    render() {
        console.error("Component not found, this is most likely due to Enums/Components.cs and Constants/Component.js mismatch");
        debugger;
        return (
            <h1>
                Component not found
            </h1>
        );
    }
}

export default NotFound;