import React, {Component} from 'react';

class IngredientItemManager extends Component {
    state = {
        currentIngredients: [null]
    };

    render() {
        const {units, types} = this.props;
        const { currentIngredients } = this.state;

        return (
            <div>
                <button type="button" onClick={(e) => {
                    e.preventDefault();
                    const ci = currentIngredients.slice(0);
                    ci.push(null);
                    this.setState({
                        currentIngredients: ci
                    })
                }} className={"btn btn-success"}>add new ingredient</button>
                {currentIngredients.map((o, i) => {
                    return (
                        <div key={i} className="form-group">
                            <label className={"control-label col-md-2"}>Ingredient</label>
                            <div className="col-md-2">
                                <select name={`type-${i}`} className={"form-control"}>
                                    {types.map(x=><option key={`${x.Type}`} value={x.Id}>{x.Type}</option>)}
                                </select>
                            </div>
                            <div className="col-md-2">
                                <input type={"number"} name={`ingredientvalue-${i}`} className={"form-control"} placeholder={"value of unit"}/>
                            </div>
                            <div className="col-md-2">

                                <select name={`unit-${i}`} className={"form-control"}>
                                    {units.map((x, i)=> i < 2 ? <option key={`${x.Unit}`} value={x.Id}>{x.Unit}</option> : null)}
                                </select>
                            </div>
                            <button type="button" onClick={(e) => {
                                e.preventDefault();
                                const ci = currentIngredients.slice(0);
                                ci.splice(i, 1);
                                this.setState({
                                    currentIngredients: ci
                                })
                            }} className={"btn btn-danger col-md-2"}>remove this ingredient</button>
                        </div>
                    );
                })}
            </div>
        );
    }
}

export default IngredientItemManager;