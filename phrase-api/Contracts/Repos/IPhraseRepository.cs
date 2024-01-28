using phrase_api.Models;

namespace phrase_api.Contracts.Repos
{
    public interface IPhraseRepository
    {
        Task Create(Phrase phrase);
        Task Delete(Guid id);
        Task<List<Phrase>> GetAll();
        Task<Phrase> GetById(Guid id);
        Task Update(Guid id, Phrase phrase);
    }
}