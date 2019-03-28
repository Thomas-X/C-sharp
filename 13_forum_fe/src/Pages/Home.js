import React, {Component} from 'react';
import {Parallax, ParallaxLayer} from 'react-spring/renderprops-addons';
import "../Styling/Home.css";
import Chef from "../Assets/chef.svg";
import styled from "styled-components";

const HeroHeader = styled.h1`
    font-weight: bold;
    font-size: 3em;
    line-height: 1.6;
    margin: 0;
`;
const Body = styled.p`
      font-size: 2em;
      margin: 0;
`;

const FlexColumn = styled.div`
      display: flex;
      flex-direction: column;
      line-height: 1.6;
`;

const url = (name, wrap = false) => `${wrap ? 'url(' : ''}https://awv3node-homepage.surge.sh/build/assets/${name}.svg${wrap ? ')' : ''}`

// #db6732

class Home extends Component {
    render() {
        return (
            <Parallax ref={ref => (this.parallax = ref)} pages={3}>
                <ParallaxLayer offset={0} speed={0} style={{ backgroundColor: '#f4be34' }} />
                <ParallaxLayer offset={1} speed={0} style={{ backgroundColor: '#1d3f4b' }} />
                <ParallaxLayer offset={2} speed={0} style={{ backgroundColor: '#3da4b8' }} />
                <ParallaxLayer offset={0} speed={0} factor={3} style={{ backgroundImage: url('stars', true), backgroundSize: 'cover' }} />
                <ParallaxLayer offset={1.3} speed={-0.3} style={{ pointerEvents: 'none' }}>
                    <img src={url('satellite4')} style={{ width: '15%', marginLeft: '70%' }} />
                </ParallaxLayer>
                <ParallaxLayer
                    offset={0}
                    speed={0.5}
                    style={{display: 'flex', alignItems: 'center', justifyContent: 'center'}}>
                    <img src={Chef} style={{width: '20%', marginRight: "60%", marginTop: "40%"}}/>
                </ParallaxLayer>
                <ParallaxLayer
                    offset={0}
                    speed={0.2}
                    style={{display: 'flex', alignItems: 'center', justifyContent: 'center', flexDirection: "column"}}>
                    <FlexColumn style={{width: "60%"}}>
                        <HeroHeader style={{maxWidth: "700px"}}>Lorem ipsum</HeroHeader>
                        <Body>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aperiam at culpa debitis
                        distinctio</Body>
                    </FlexColumn>
                </ParallaxLayer>

                <ParallaxLayer
                    offset={1}
                    speed={0.1}
                    style={{display: 'flex', alignItems: 'center', justifyContent: 'center'}}>
                    <img src={url('bash')} style={{width: '40%'}}/>
                </ParallaxLayer>
                <ParallaxLayer
                    offset={2}
                    speed={-0}
                    style={{display: 'flex', alignItems: 'center', justifyContent: 'center'}}
                    onClick={() => this.parallax.scrollTo(0)}>
                    <img src={url('clients-main')} style={{width: '40%'}}/>
                </ParallaxLayer>
            </Parallax>
        )
    }
}

export default Home;
