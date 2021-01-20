import styled from "@emotion/styled";
import { observer } from "mobx-react";

import teamStore from "../src/stores/teamStore"

const Input = styled.input`
  width: 100%;
  padding: 0.2rem;
  font-size: large;
`;

const PokemonFilter = () => {
  return (
    <Input
      type="text"
      value={teamStore.filter}
      onChange={(evt) => teamStore.setFilter(evt.target.value)}
    />
  );
};

export default observer(PokemonFilter);
