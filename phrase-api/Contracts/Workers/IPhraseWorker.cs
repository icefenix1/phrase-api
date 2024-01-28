using phrase_api.Models;

namespace phrase_api.Contracts.Workers
{
    public interface IPhraseWorker
    {
        Task CreatePhrase(string text);
        Task DeletePhrase(Guid id);
        Task<IEnumerable<Phrase>> GetAllPhrases();
        Task<Phrase> GetPhraseById(Guid id);
        Task UpdatePhrase(Guid id, string text);
    }
}