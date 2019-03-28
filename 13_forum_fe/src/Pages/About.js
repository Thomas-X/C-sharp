import React from 'react';
import {Link} from "react-router-dom";
import Routes from "../Constants/Routes";

const About = () => {
    return (
        <div>
            Hello from about
            <br/>
            <Link to={Routes.home.path}>
                go to home
            </Link>
        </div>
    );
};

export default About;