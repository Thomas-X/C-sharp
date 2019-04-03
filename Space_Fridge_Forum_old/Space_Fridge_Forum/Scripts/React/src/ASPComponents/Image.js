import React, {Component} from 'react';
import styled from 'styled-components';

const InternalImage = styled.img`
  max-width: 500px;
  max-height: 500px;
`;

class Image extends Component {
    render() {
        return <InternalImage src={this.props.imageUrl} alt={"image"}/>
    }
}

export default Image;