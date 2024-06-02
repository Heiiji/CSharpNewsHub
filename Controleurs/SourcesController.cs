using SourcesStoreApi.Models;
using SourcesStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace SourcesStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SourcesController : ControllerBase
{
    private readonly SourcesService _sourcesService;

    public SourcesController(SourcesService newsService) =>
        _sourcesService = newsService;

    [HttpGet]
    public async Task<List<Source>> Get() =>
        await _sourcesService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Source>> Get(string id)
    {
        var source = await _sourcesService.GetAsync(id);

        if (source is null)
        {
            return NotFound();
        }

        return source;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Source newNews)
    {
        await _sourcesService.CreateAsync(newNews);

        return CreatedAtAction(nameof(Get), new { id = newNews.Id }, newNews);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Source updatedNews)
    {
        var news = await _sourcesService.GetAsync(id);

        if (news is null)
        {
            return NotFound();
        }

        updatedNews.Id = news.Id;

        await _sourcesService.UpdateAsync(id, updatedNews);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var news = await _sourcesService.GetAsync(id);

        if (news is null)
        {
            return NotFound();
        }

        await _sourcesService.RemoveAsync(id);

        return NoContent();
    }
}