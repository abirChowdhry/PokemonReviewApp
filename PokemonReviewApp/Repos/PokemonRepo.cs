using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class PokemonRepo : IPokemonRepo
    {
        private DataContext db;

        public PokemonRepo(DataContext _db) 
        {
            db = _db;
        }

        public List<Pokemon> GetAll()
        {
            return db.Pokemons.ToList();
        }

        public Pokemon Get(int id)
        {
            return db.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetByName(string name)
        {
            return db.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public bool Exst(int id)
        {
            return db.Pokemons.Any(p => p.Id == id);
        }

        public List<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return db.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(p => p.Pokemon).ToList();
        }

        public List<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return db.PokemonOwners.Where(o => o.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool Create(Pokemon pokemon, string categoryName)
        {
            var category = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            db.PokemonCategories.Add(pokemonCategory);
            db.Pokemons.Add(pokemon);
            return db.SaveChanges() > 0;
        } 

        public bool Create(Pokemon type)
        {
            throw new NotImplementedException();
        }

        public bool Update(Pokemon pokemon)
        {
            db.Update(pokemon);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = db.Pokemons.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
