using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepo : ICrudRepo<Country, int, bool, string> 
    {
        Country GetCountryByOwner(int countryId);
    }
}
