using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class OwnerRepo : IOwnerRepo
    {
        DataContext db;
        public OwnerRepo(DataContext _db)
        {
            db = _db;
        }

        public List<Owner> GetAll()
        {
            return db.Owners.ToList();
        }

        public Owner Get(int id)
        {
            return db.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public Owner GetByName(string name)
        {
            return db.Owners.Where(o => o.Name == name).FirstOrDefault();
        }

        public bool Exst(int id)
        {
            return db.Owners.Any(o => o.Id == id);
        }

        public List<Owner> GetOwnersByCountry(int countryId)
        {
            return db.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public List<Owner> GetOwnersByPokemon(int pokeId)
        {
            return db.PokemonOwners.Where(p => p.PokemonId == pokeId).Select(o => o.Owner).ToList();
        }

        public bool Create(Owner owner)
        {
            db.Owners.Add(owner);
            return db.SaveChanges() > 0;
        }

        public bool OwnPokemon(string ownerName, string pokemonName)
        {
            var pokemonOwner = new PokemonOwner()
            {
                Owner = db.Owners.Where(o => o.Name == ownerName).FirstOrDefault(),
                Pokemon = db.Pokemons.Where(p => p.Name == pokemonName).FirstOrDefault()
            };

            db.PokemonOwners.Add(pokemonOwner);
            return db.SaveChanges() > 0;
        }

        public bool AlreadyOwnedPokemon(string ownerName, string pokemonName)
        {
            return db.PokemonOwners.Any(po => po.Owner.Name == ownerName && po.Pokemon.Name == pokemonName);
        }

        public bool Update(Owner owner)
        {
            db.Update(owner);
            return db.SaveChanges()>0;
        }

        public bool Delete(int id)
        {
            var data = db.Owners.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
