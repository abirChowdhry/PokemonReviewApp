using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private DataContext db;
        public CategoryRepo(DataContext _db) 
        { 
            db = _db;  
        }

        public List<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category Get(int id)
        {
            return db.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetByName(string name)
        {
            return db.Categories.Where(c => c.Name == name).FirstOrDefault();
        }

        public bool Exst(int id)
        {
           return db.Categories.Any(c => c.Id == id);
        }

        public bool Create(Category category)
        {
            db.Categories.Add(category);
            return db.SaveChanges() > 0;
        }

        public bool Update(Category category)
        {
            db.Update(category);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = db.Categories.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
