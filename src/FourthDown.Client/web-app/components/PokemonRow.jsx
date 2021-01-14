import Link from "next/link";
import PropTypes from "prop-types";
import { Button } from "@material-ui/core";
import { observer } from "mobx-react";

import PokemonType from "../src/pokemonType";

const PokemonRow = ({ pokemon, onClick }) => (
  <>
    <tr key={pokemon.id}>
      <td>
        <Link href={`/pokemon/${pokemon.id}`}>
          <a>{pokemon.name.english}</a>
        </Link>
      </td>
      <td>{pokemon.type.join(", ")}</td>
      <td>
        <Button
          variant="contained"
          color="primary"
          onClick={() => onClick(pokemon)}
        >
          More Information
        </Button>
      </td>
    </tr>
  </>
);

PokemonRow.propTypes = {
  pokemon: PropTypes.arrayOf(PokemonType),
};

export default observer(PokemonRow);
