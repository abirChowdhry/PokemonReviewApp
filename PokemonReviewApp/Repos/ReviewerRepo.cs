using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repos
{
    public class ReviewerRepo : IReviewerRepo
    {
        private DataContext db;
        public ReviewerRepo(DataContext _db)
        {
            db = _db;
        }

        public List<Reviewer> GetAll()
        {
            return db.Reviewers.ToList();
        }

        public Reviewer Get(int id)
        {
            return db.Reviewers.Where(r => r.Id == id).Include(e => e.Reviews).FirstOrDefault();
        }

        public Reviewer GetByName(string name)
        {
            return db.Reviewers.Where(r => r.FirstName + " " +r.LastName == name).FirstOrDefault();
        }

        public bool Exst(int id)
        {
            return db.Reviewers.Any(r => r.Id == id);
        }

        public bool Create(Reviewer reviewer)
        {
            db.Reviewers.Add(reviewer);
            return db.SaveChanges() > 0;
        }

        public bool Update(Reviewer reviewer)
        {
            db.Update(reviewer);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = db.Reviewers.Where(c => c.Id == id).FirstOrDefault();
            db.Remove(data);
            return db.SaveChanges() > 0;
        }
    }
}
