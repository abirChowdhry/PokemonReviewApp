using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepo : ICrudRepo<Pokemon, int, bool, string>
    {
        List<Pokemon> GetPokemonByCategory(int categoryId);
        List<Pokemon> GetPokemonByOwner(int ownerId);
        bool Create(Pokemon pokemon, string categoryName);
    }
}
