using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class ReviewRepo : IReviewRepo
    {
        DataContext db;

        public ReviewRepo(DataContext _db)
        {
            db = _db;
        }

        public List<Review> GetAll()
        {
            return db.Reviews.ToList(); 
        }

        public Review Get(int id)
        {
            return db.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public Review GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Exst(int id)
        {
            return db.Reviews.Any(r => r.Id == id);
        }

        public List<Review> GetReviewsOfAPokemon(int pokeId)
        {    
            return db.Reviews.Where(p => p.Pokemon.Id == pokeId).ToList();
        }

        public List<Review> GetReviewsByReviewer(int reviewerId)
        {
            return db.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool Create(Review review)
        {
            db.Reviews.Add(review);
            return db.SaveChanges() > 0;
        }

        public bool Update(Review review)
        {
            db.Update(review);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = db.Reviews.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
