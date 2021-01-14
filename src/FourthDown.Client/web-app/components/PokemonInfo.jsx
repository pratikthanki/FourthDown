import { observer } from "mobx-react";

import store from "../src/store";

const PokemonInfo = () => {
  return store.selectedPokemon ? (
    <div>
      <h2>{store.selectedPokemon.name.english}</h2>
      <table>
        <tbody>
          {Object.keys(store.selectedPokemon.base).map((key) => (
            <tr key={key}>
              <td>{key}</td>
              <td>{store.selectedPokemon.base[key]}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  ) : null;
};

export default observer(PokemonInfo);
