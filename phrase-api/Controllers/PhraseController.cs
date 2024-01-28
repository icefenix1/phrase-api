using Microsoft.AspNetCore.Mvc;
using phrase_api.Contracts.Workers;
using phrase_api.Models;

[ApiController]
[Route("api/[controller]")]
public class PhraseController : ControllerBase
{
    private readonly IPhraseWorker _phraseWorker;

    public PhraseController(IPhraseWorker phraseWorker)
    {
        _phraseWorker = phraseWorker;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Phrase>>> GetAllPhrases()
    {
        var phrases = await _phraseWorker.GetAllPhrases();
        return Ok(phrases);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Phrase>> GetPhrase(Guid id)
    {
        var phrase = await _phraseWorker.GetPhraseById(id);
        if (phrase == null)
        {
            return NotFound();
        }
        return Ok(phrase);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePhrase([FromBody] string text)
    {
        await _phraseWorker.CreatePhrase(text);
        return CreatedAtAction(nameof(GetPhrase), new { id = text }, text);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePhrase(Guid id, [FromBody] string text)
    {
        try
        {
            await _phraseWorker.UpdatePhrase(id, text);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhrase(Guid id)
    {
        try
        {
            await _phraseWorker.DeletePhrase(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}
