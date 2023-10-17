using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class CountryRepo : ICountryRepo
    {
        DataContext db;
        public CountryRepo(DataContext _db)
        {
            db = _db;
        }

        public List<Country> GetAll()
        {
            return db.Countries.ToList();
        }

        public Country Get(int id)
        {
            return db.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetByName(string name)
        {
            return db.Countries.Where(c => c.Name == name).FirstOrDefault();
        }

        public bool Exst(int id)
        {
            return db.Countries.Any(c => c.Id == id);
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return db.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public bool Create(Country country)
        {
            db.Countries.Add(country);
            return db.SaveChanges() > 0;
        }

        public bool Update(Country country)
        {
            db.Update(country);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = db.Countries.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
