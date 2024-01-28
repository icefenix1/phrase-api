namespace phrase_api.Models
{
    public class Phrase
    {
        public Guid Id { get; }

        public string Text { get; set; }

        public Phrase()
        {
            Id = Guid.NewGuid();
            Text = "";
        }

        public Phrase(string phrase)
        {
            Id = Guid.NewGuid();
            Text = phrase;
        }
    }
}
