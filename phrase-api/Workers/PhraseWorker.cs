
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using phrase_api.Contracts.Repos;
using phrase_api.Contracts.Workers;
using phrase_api.Models;
using phrase_api.Repos;

namespace phrase_api.Workers
{

    public class PhraseWorker : IPhraseWorker
    {
        private readonly IPhraseRepository _phraseRepository;

        public PhraseWorker(IPhraseRepository phraseRepository)
        {
            _phraseRepository = phraseRepository;
        }

        public async Task<IEnumerable<Phrase>> GetAllPhrases()
        {
            return await _phraseRepository.GetAll();
        }

        public async Task<Phrase> GetPhraseById(Guid id)
        {
            return await _phraseRepository.GetById(id);
        }

        public async Task CreatePhrase(string text)
        {
            var phrase = new Phrase(text);
            await _phraseRepository.Create(phrase);
        }

        public async Task UpdatePhrase(Guid id, string text)
        {
            var existingPhrase = await _phraseRepository.GetById(id);
            if (existingPhrase != null)
            {
                existingPhrase.Text = text;
                await _phraseRepository.Update(id, existingPhrase);
            }
            else
            {
                throw new ArgumentException($"Phrase with ID {id} not found.");
            }
        }

        public async Task DeletePhrase(Guid id)
        {
            var existingPhrase = await _phraseRepository.GetById(id);
            if (existingPhrase != null)
            {
                await _phraseRepository.Delete(id);
            }
            else
            {
                throw new ArgumentException($"Phrase with ID {id} not found.");
            }
        }
    }
}