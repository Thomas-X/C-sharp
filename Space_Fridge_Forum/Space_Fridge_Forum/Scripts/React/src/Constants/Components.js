import ASPComponent from "../ASPComponent";
import Home from "../ASPComponents/Home";
import Image from "../ASPComponents/Image";
import HomeHero from "../ASPComponents/HomeHero";
import Fridge from "../ASPComponents/Fridge";
import IngredientItemManager from "../ASPComponents/IngredientItemManager";

// These values should matchup with the enum in ASP
export default [
    new ASPComponent(0, Home),
    new ASPComponent(2, Image),
    new ASPComponent(3, HomeHero),
    new ASPComponent(4, Fridge),
    new ASPComponent(5, IngredientItemManager),
];