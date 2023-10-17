namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gym { get; set; }

        //One to One
        public Country Country { get; set; }

        //Many to Many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}
