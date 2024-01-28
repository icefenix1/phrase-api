using MongoDB.Driver;
using phrase_api.Contracts.Repos;
using phrase_api.Models;

namespace phrase_api.Repos;

public class PhraseRepository : IPhraseRepository
{
    private readonly IMongoCollection<Phrase> _phraseCollection;

    public PhraseRepository(IMongoClient client, IConfiguration configuration)
    {
        var database = client.GetDatabase(configuration["MongoDBName"]);
        _phraseCollection = database.GetCollection<Phrase>("Phrases");
    }

    // Create
    public async Task Create(Phrase phrase)
    {
        await _phraseCollection.InsertOneAsync(phrase);
    }

    // Read
    public async Task<Phrase> GetById(Guid id)
    {
        var filter = Builders<Phrase>.Filter.Eq(p => p.Id, id);
        return await _phraseCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Phrase>> GetAll()
    {
        return await _phraseCollection.Find(_ => true).ToListAsync();
    }

    // Update
    public async Task Update(Guid id, Phrase phrase)
    {
        var filter = Builders<Phrase>.Filter.Eq(p => p.Id, id);
        var updateDefinition = Builders<Phrase>.Update
            .Set(p => p.Text, phrase.Text);
        await _phraseCollection.UpdateOneAsync(filter, updateDefinition);
    }

    // Delete
    public async Task Delete(Guid id)
    {
        var filter = Builders<Phrase>.Filter.Eq(p => p.Id, id);
        await _phraseCollection.DeleteOneAsync(filter);
    }
}