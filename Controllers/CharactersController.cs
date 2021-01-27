using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RickAndMortyCharactersAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace RickAndMortyCharactersAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CharactersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetAllCharacters()
        {
            List<Character> characters = new List<Character>();
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://rickandmortyapi.com/api/character");
            
            if(response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                JsonElement data = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream);
                try
                {
                    var results = data.GetProperty("results").EnumerateArray();
                    foreach (var character in results)
                    {
                        characters.Add(new Character
                        {
                        Id = character.GetProperty("id").GetInt32(),
                        Name = character.GetProperty("name").GetString(),
                        Image = character.GetProperty("image").GetString(),
                        Status = character.GetProperty("status").GetString(),
                        Specie = character.GetProperty("species").GetString(),
                        Gender = character.GetProperty("gender").GetString(),
                        });
                    }
                    return Ok(characters);
                }
                catch (System.Exception error)
                {
                    return BadRequest(error.Message);
                }
            }

            return NotFound(response.StatusCode);

        }
    }
}