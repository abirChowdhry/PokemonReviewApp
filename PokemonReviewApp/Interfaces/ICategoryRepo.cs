using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepo : ICrudRepo<Category, int, bool, string>
    {
    }
}
