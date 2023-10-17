using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepo : ICrudRepo<Reviewer, int, bool, string>
    {
    }
}
