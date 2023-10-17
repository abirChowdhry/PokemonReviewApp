using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepo : ICrudRepo<Review, int, bool, string>
    {
        List<Review> GetReviewsOfAPokemon(int pokeId);
        List<Review> GetReviewsByReviewer(int reviewerId);
    }
}
