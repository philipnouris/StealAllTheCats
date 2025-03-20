using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Data;
using StealAllTheCats.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StealAllTheCats.Controllers
{
    [Route("api/cats")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly CatService _catService;


        public CatsController(StealAllTheCatsDbContext context, HttpClient httpClient, CatService catService)
        {
            _catService = catService;
        }


        //fetch 25 cat images and save them to the database
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchCats()
        {
            //Calls the service to fetch and store cat data
            var success = await _catService.FetchAndStoreCatsAsync();

            //Returns a 200 OK if successful, or 500 if there was an error
            return success ? Ok("Cats fetched and stored successfully!") : StatusCode(500, "Failed to fetch cats.");
        }

        //get cat by its Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatById(int id) 
        {
            //Validation: Ensure ID is greater than zero
            if (id <= 0)
            {
                return BadRequest(new { error = "Invalid ID provided." });
            }

            //call the service to find the cat on given id
            var cat = await _catService.GetCatByIdAsync(id);

            //if no cat is found return a 404 Not found response
            if(cat == null)
            {
                return NotFound(new { message = "Cat not found" });
            }

            //return a 200 OK response and the cat object 
            return Ok(cat);
            
        }

        //Retrieve all cats with pagination
        [HttpGet("getCatsPerPage")]
        public async Task<IActionResult> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            //Validation: Ensure that page and pageSize are greater than zero
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { error = "Page and pageSize must be greater than 0." });
            }

            var cats = await _catService.GetCatsAsync(page, pageSize);

            //if no cats found return 404 No cats found response
            if (cats == null || cats.Count == 0)
            {
                return NotFound(new { message = "No cats found" });
            }
            //200 and pagionated list of cats response
            return Ok(cats);
        }

        //Retrieve cats filtered by a tag with pagination
        [HttpGet("getCatsByTag")]
        public async Task<IActionResult> GetCatsByTag([FromQuery] string? tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            //Validation: Ensure tag is provided
            if (string.IsNullOrWhiteSpace(tag))
            {
                return BadRequest(new { error = "Tag parameter is required." });
            }

            //Pagination values check
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { error = "Page and pageSize must be greater than 0." });
            }

            var cats = await _catService.GetCatsAsync(tag, page, pageSize);
            //return 400 if no cats are found
            if (cats == null || cats.Count == 0)
            {
                return NotFound(new { message = "No cats found" });
            }

            //200 and filtered list of cats
            return Ok(cats);
        }
    }
}
