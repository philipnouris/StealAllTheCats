using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Data;
using StealAllTheCats.Dto;
using StealAllTheCats.Models;

namespace StealAllTheCats.Services
{
    public class CatService
    {
        private readonly StealAllTheCatsDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string ApiUrl;
        private readonly string ApiKey;

        //constructor
        public CatService(StealAllTheCatsDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            ApiUrl = _configuration["CatApi:BaseUrl"];
            ApiKey = _configuration["CatApi:ApiKey"];


        }


        //fetch 25 cat images and save them to the database
        public async Task<bool> FetchAndStoreCatsAsync()
        {

            //create a GET request to fetch cat images from external API
            var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
            request.Headers.Add("x-api-key", ApiKey);

            var response = await _httpClient.SendAsync(request);
            //If response (API) fails, return false
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            //Deserialize API response into a list of cat objects
            var responseBody = await response.Content.ReadAsStringAsync();
            var cats = JsonSerializer.Deserialize<List<CatApiResponse>>(responseBody);

            foreach (var cat in cats)
            {
                //Prevent duplicate entries by checking if a cat with the same ID already exists.
                if (await _context.Cats.AnyAsync(c => c.CatId == cat.id))
                {
                    continue; //dont add this cat.
                }
                //if (!_context.Cats.Any(c => c.CatId == cat.id))
                //{
                var newCat = new Cat
                {
                    CatId = cat.id,
                    Width = cat.width,
                    Height = cat.height,
                    ImageUrl = cat.url
                };

                foreach (var breed in cat.breeds)
                {
                    var tags = breed.temperament.Split(",").Select(t => t.Trim());

                    foreach (var tag in tags)
                    {
                        //check if tag already exists
                        var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag);
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tag };
                            _context.Tags.Add(existingTag);
                            await _context.SaveChangesAsync();
                        }
                        
                        newCat.Tags.Add(existingTag);
                        
                    }
                }
                _context.Cats.Add(newCat);
                //}
            }
            await _context.SaveChangesAsync();
            return true;
        }

        //Retrieve Cat By id
        public async Task<Cat?> GetCatByIdAsync(int id)
        {
            return await _context.Cats
                .Include(c => c.Tags) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        //Retrieve list of Cats per page
        public async Task<List<CatDto>> GetCatsAsync(int page, int pageSize)
        {
            var cats = await _context.Cats
                .Include(c => c.Tags)
                .Skip((page - 1) * pageSize) //skip previous page
                .Take(pageSize) 
                .ToListAsync();

            return cats.Select(cat => new CatDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                ImageUrl = cat.ImageUrl,
                Tags = cat.Tags.Select(t => t.Name).ToList() 
            }).ToList();
        }

        //Retrieve cats filtered by tag per page
        public async Task<List<CatDto>> GetCatsAsync(string? tag, int page, int pageSize)
        {
            var query = _context.Cats
                .Include(c => c.Tags)
                .AsQueryable();

            //Apply tag filtering if provided using where
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(c => c.Tags.Any(t => t.Name.ToLower() == tag.ToLower()));
            }

            var cats = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return cats.Select(cat => new CatDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                ImageUrl = cat.ImageUrl,
                Tags = cat.Tags.Select(t => t.Name).ToList()
            }).ToList();
        }
    }
}
