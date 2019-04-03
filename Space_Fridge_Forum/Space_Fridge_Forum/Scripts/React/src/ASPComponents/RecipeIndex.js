import React, {Component} from 'react';
import styled from "styled-components";

const Recipe = styled.div`
  cursor: pointer;
  padding: 5px;
  transition: background-color 0.3s;
  &:hover {
    background-color: lightgray;
  }
`;

class RecipeIndex extends Component {
    state = {
        recipes: this.props.Recipes,
        filterBasedOnFridgeIngredients: false,
        search: "",
    };

    onFridgeFilter = (e) => {
        this.setState({
            filterBasedOnFridgeIngredients: e.target.checked
        }, () => {
            this.updateList();
        });
    };

    recipeIngredientsMatchFridge = (recipe) => {
        let pass = false;
        const passedItems = [];
        for (const recipeIngredient of recipe.Ingredients) {
            for (const fridgeIngredient of this.props.FridgeIngredients) {
                const percentile = fridgeIngredient.Value * 0.25;
                const fridgeVal = fridgeIngredient.Value;
                const recipeVal = recipeIngredient.Value;
                const min = fridgeVal - percentile;
                const max = fridgeVal + percentile;

                if (
                    // Check if ingredient unit is the same
                    recipeIngredient.IngredientUnit.Unit === fridgeIngredient.IngredientUnit.Unit &&
                    // Next up, check if the type is the same
                    recipeIngredient.IngredientType.Type === fridgeIngredient.IngredientType.Type &&
                    // Then, check if the value stored in the fridge is around 35% the weight of the recipe
                    recipeVal > min && recipeVal < max
                ) {
                    passedItems.push(fridgeIngredient);
                    // Only see a recipe as passed when we have ALL the ingredients with a margin of 25% in our space-fridge.
                    if (passedItems.length === recipe.Ingredients.length) {
                        pass = true;
                    }
                }
            }
        }
        return pass;
    };

    onSearch = (e) => {
        this.setState({
            search: e.target.value
        }, () => {
            this.updateList();
        });
    };

    updateList = () => {
        if (this.state.search.length > 0 || !!this.state.filterBasedOnFridgeIngredients === true) {
            this.setState({
                recipes: this.props.Recipes.filter(x => {
                    const b = x.Name.includes(this.state.search);
                    return this.state.filterBasedOnFridgeIngredients
                        ? this.state.search.length > 0
                            ? b && this.recipeIngredientsMatchFridge(x)
                            : this.recipeIngredientsMatchFridge(x)
                        : this.state.search.length > 0
                            ? b
                            : true;
                })
            });
        } else {
            this.setState({
                recipes: this.props.Recipes
            });
        }
    };

    render() {
        console.log(this.props);
        return (
            <div>
                <h1>Recipes</h1>
                <label>Search</label>
                <input style={{marginBottom: "1rem"}}
                       placeholder={"search recipes.."} className={"form-control full-width"} value={this.state.search}
                       onChange={this.onSearch}/>
                <div className="form-check">
                    <input onChange={this.onFridgeFilter} className="form-check-input" type="checkbox"
                           checked={this.state.filterBasedOnFridgeIngredients}/>
                    <label className="form-check-label">
                        Filter based on fridge ingredients
                    </label>
                </div>
                {this.state.recipes.map((x, i) => {
                    return (
                        <Recipe key={`recept-${i}`} onClick={() => window.location.href = `/Recipes/Details/${x.Id}`}>
                            <h2>{x.Name}</h2>
                            <h3><i>
                                by {x.User.Email.split("@")[0]}
                            </i>
                            </h3>
                            <p>
                                {/* max 50 chars */}
                                {x.Description.substring(0, 50)}
                            </p>
                        </Recipe>
                    );
                })}
            </div>
        );
    }
}

export default RecipeIndex;