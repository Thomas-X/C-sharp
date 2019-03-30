import ASPComponent from "../ASPComponent";
import Home from "../ASPComponents/Home";
import Image from "../ASPComponents/Image";
import HomeHero from "../ASPComponents/HomeHero";

// These values should matchup with the enum in ASP
export default [
    new ASPComponent(0, Home),
    new ASPComponent(2, Image),
    new ASPComponent(3, HomeHero),
];