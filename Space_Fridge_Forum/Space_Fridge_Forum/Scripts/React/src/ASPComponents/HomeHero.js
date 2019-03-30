import React, {Component} from 'react';
import styled from 'styled-components';

const Hero = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #f9a825;
  height: 100vh;
`;

const Title = styled.h1`
  font-size: 3em;
  font-family: "Roboto",sans-serif;
`;

class HomeHero extends Component {
    render() {
        return (
            <Hero>
                <Title>
                    !HELLO WORLD!
                </Title>
            </Hero>
        );
    }
}

export default HomeHero;