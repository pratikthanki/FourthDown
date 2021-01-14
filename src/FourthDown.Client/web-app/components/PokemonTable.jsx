import { observer } from "mobx-react";

import PokemonRow from "./PokemonRow";
import store from "../src/store";

function PokemonTable() {
  return (
    <table width="100%">
      <tbody>
        {store.filteredPokemon.slice(0, 20).map((pokemon) => (
          <PokemonRow
            key={pokemon.id}
            pokemon={pokemon}
            onClick={(pokemon) => store.setSelectedPokemon(pokemon)}
          />
        ))}
      </tbody>
    </table>
  );
}

export default observer(PokemonTable);
