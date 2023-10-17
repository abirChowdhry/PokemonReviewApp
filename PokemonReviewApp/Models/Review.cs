namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        //One to One
        public Reviewer Reviewer { get; set; }

        //One to One
        public Pokemon Pokemon { get; set; }
    }
}
