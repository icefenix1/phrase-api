namespace phrase_api.Contracts.Workers
{
    public interface IWords
    {
        Task<IEnumerable<string>> Search(string partOfSpeach, string search);
        IEnumerable<string> GetParts();
    }
}