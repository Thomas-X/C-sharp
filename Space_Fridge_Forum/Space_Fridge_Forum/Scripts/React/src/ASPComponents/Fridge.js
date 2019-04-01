import React, {Component} from 'react';
import Color from "color";

class Fridge extends Component {
    render() {
        const { color } = this.props;
        const darkColor = Color(color).darken(0.4);
        return (
            <div>
                <svg width={"100%"} height={"124pt"} viewBox="-128 0 512 512" xmlns="http://www.w3.org/2000/svg">
                    <path
                        d="m224 480h-192c-17.679688 0-32-14.320312-32-32v-416c0-17.679688 14.320312-32 32-32h192c17.679688 0 32 14.320312 32 32v416c0 17.679688-14.320312 32-32 32zm0 0"
                        fill={color}/>
                    <path d="m32 464h32v48h-32zm0 0" fill={color}/>
                    <path d="m192 464h32v48h-32zm0 0" fill={color}/>
                    <path d="m0 304v144c0 17.679688 14.320312 32 32 32h192c17.679688 0 32-14.320312 32-32v-144zm0 0"
                          fill="#bfc9d1"/>
                    <path d="m32 144h32v64h-32zm0 0" fill={darkColor}/>
                    <path d="m32 352h32v64h-32zm0 0" fill={darkColor}/>
                    <path d="m32 80h32v32h-32zm0 0" fill="#fff"/>
                </svg>
            </div>
        );
    }
}

export default Fridge;