using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepo : ICrudRepo<Owner, int, bool, string>
    {
        List<Owner> GetOwnersByCountry(int countryId);
        List<Owner> GetOwnersByPokemon(int pokeId);
        bool OwnPokemon(string ownerName, string pokemonName);
        bool AlreadyOwnedPokemon(string ownerName, string pokemonName);
    }
}
