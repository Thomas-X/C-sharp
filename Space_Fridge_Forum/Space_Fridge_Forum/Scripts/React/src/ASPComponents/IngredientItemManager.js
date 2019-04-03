import React, {Component} from 'react';
import styled from "styled-components";

const ButtonContainer = styled.div`
  display: flex;
  padding: 10px 0;
`;

// Honestly this whole class is a mess and should be re-written, it's state management is confusing
// And the things it does are weird in a react-sense.
// TODO rewrite this class
class IngredientItemManager extends Component {
    state = {
        currentIngredients: [0],
        hasCleared: false,
    };

    constructor(props) {
        super(props);
        // This is not clean.
        if (props.ingredients != null) {
            for (let i = 0; i < props.ingredients.length; i++) {
                this.state.currentIngredients[i] = i;
            }
        }
    }


    renderDefault = (o, i) => {
        const {units, types} = this.props;
        return (
            <div key={`newingredients-${i}`} className="form-group">
                <label className={"control-label col-md-2"}>Ingredient</label>
                <div className="col-md-2">
                    <select name={`type-${i}`} className={"form-control"}>
                        {types.map((x, a) => <option key={`${x.Type}-${a}`} value={x.Id}>{x.Type}</option>)}
                    </select>
                </div>
                <div className="col-md-2">
                    <input type={"number"} name={`ingredientvalue-${i}`} className={"form-control"}
                           placeholder={"value of unit"}/>
                </div>
                <div className="col-md-2">

                    <select name={`unit-${i}`} className={"form-control"}>
                        {units.map((x, p) => p < 2 ?
                            <option key={`${x.Unit}-${p}`} value={x.Id}>{x.Unit}</option> : null)}
                    </select>
                </div>
            </div>
        );
    };

    render() {
        const {units, types, ingredients} = this.props;
        const {currentIngredients} = this.state;

        return (
            <div>
                <ButtonContainer>
                    <button style={{marginRight: "1rem"}} type="button" onClick={(e) => {
                        e.preventDefault();
                        const ci = currentIngredients.slice(0);
                        ci.push(ci.length);
                        this.setState({
                            currentIngredients: ci
                        })
                    }} className={"btn btn-success col-md-2"}>add new ingredient
                    </button>
                    <button type="button" onClick={(e) => {
                        e.preventDefault();
                        const ci = currentIngredients.slice(0);
                        ci.splice(currentIngredients.length - 1, 1);
                        if (ci.length <= 0) {
                            this.setState({
                                hasCleared: true,
                            }, () => {
                                this.setState({
                                    currentIngredients: ci
                                })
                            })
                        } else {
                            this.setState({
                                currentIngredients: ci
                            })
                        }

                    }} className={"btn btn-danger col-md-2"}>remove the last ingredient
                    </button>
                </ButtonContainer>
                {/* TODO DRY */}
                {!!ingredients && !this.state.hasCleared && currentIngredients.map((t, idx) => {

                    const x = this.props.ingredients[idx];

                    if (x === null || x === undefined) {
                        return this.renderDefault(null, idx);
                    }

                    // TODO DRY
                    let typesIndex = 0;
                    for (let i = 0; i < types.length; i++) {
                        if (types[i].Type === x.IngredientType.Type) {
                            typesIndex = i;
                        }
                    }
                    let unitsIndex = 0;
                    for (let i = 0; i < units.length; i++) {
                        if (units[i].Unit === x.IngredientUnit.Unit) {
                            unitsIndex = i;
                        }
                    }
                    return (
                        <div key={`currentIngredient-${idx}`} className="form-group">
                            <label className={"control-label col-md-2"}>Ingredient</label>
                            <div className="col-md-2">
                                <select name={`type-${idx}`} className={"form-control"}>
                                    <option key={`${types[typesIndex].Type}-${idx}`}
                                            value={types[typesIndex].Id}>{types[typesIndex].Type}</option>
                                    {types.map((x, i) => i !== typesIndex ?
                                        <option key={`${x.Type}-${i}`} value={x.Id}>{x.Type}</option> : null)}
                                </select>
                            </div>
                            <div className="col-md-2">
                                <input type={"number"} name={`ingredientvalue-${idx}`} className={"form-control"}
                                       placeholder={"value of unit"} defaultValue={x.Value}/>
                            </div>
                            <div className="col-md-2">
                                <select name={`unit-${idx}`} className={"form-control"}>
                                    <option key={`${units[unitsIndex].Unit}-${idx}`}
                                            value={units[unitsIndex].Id}>{units[unitsIndex].Unit}</option>
                                    {units.map((x, p) => p < 2
                                        ? p !== unitsIndex
                                            ? <option key={`${x.Unit}-${p}`} value={x.Id}>{x.Unit}</option>
                                            : null
                                        : null)}
                                </select>
                            </div>
                        </div>

                    );
                })}

                {(ingredients == null || this.state.hasCleared) && currentIngredients.map((o, i) => {
                    return this.renderDefault(o, i);
                })}

            </div>
        );
    }
}

export default IngredientItemManager;