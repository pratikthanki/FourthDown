import { observer } from "mobx-react";

import store from "../src/store";
import teamStore from "../src/stores/teamStore"

const PokemonInfo = () => {
  return teamStore.selectedPokemon ? (
    <div>
      <h2>{teamStore.selectedPokemon.name.english}</h2>
      <table>
        <tbody>
          {Object.keys(teamStore.selectedPokemon.base).map((key) => (
            <tr key={key}>
              <td>{key}</td>
              <td>{teamStore.selectedPokemon.base[key]}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  ) : null;
};

export default observer(PokemonInfo);
