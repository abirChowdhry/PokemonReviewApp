namespace PokemonReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //One to Many
        public ICollection<Owner> Owners { get; set;}
    }
}
