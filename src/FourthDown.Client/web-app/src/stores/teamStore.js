import { makeObservable, observable, computed } from "mobx"
import useSWR from 'swr'
import fetch from "node-fetch"

class Store {
  pokemon = [];
  filter = "";
  selectedPokemon = null;

  constructor() {
    makeObservable(this, {
      pokemon: observable,
      filter: observable,
      selectedPokemon: observable,
      filteredPokemon: computed
    });
  }

  get filteredPokemon() {
    return this.pokemon.filter(({ name: { english } }) =>
      english.toLocaleLowerCase().includes(this.filter.toLocaleLowerCase())
    );
  }

  setPokemon(pokemon) {
    this.pokemon = pokemon
  }

  setFilter(filter) {
    this.filter = filter
  }

  setSelectedPokemon(selectedPokemon) {
    this.selectedPokemon = selectedPokemon
  }
}

const teamStore = new Store();

const base_url = "http://localhost:5000/"
const endpoint = "api/teams"

const url = `${base_url}${endpoint}`

console.log(url)

fetch(url)
  .then((resp) => resp.json())
  .catch((error) => {
    assert.isNotOk(error, 'Promise error');
    done();
  })
  .then((pokemon) => store.setPokemon(pokemon));

export default teamStore;
