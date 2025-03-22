using Microsoft.EntityFrameworkCore;
using Moq;
using StealAllTheCats.Data;
using StealAllTheCats.Models;
using Microsoft.Extensions.Configuration;

namespace StealAllTheCats.Services
{
    public class CatServiceTests
    {
        private readonly StealAllTheCatsDbContext _dbContext;
        private readonly CatService _catService;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configMock;

        public CatServiceTests()
        {
            // Unique DB for each test
            var options = new DbContextOptionsBuilder<StealAllTheCatsDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dbContext = new StealAllTheCatsDbContext(options);
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configMock = new Mock<IConfiguration>();

            _catService = new CatService(_dbContext, new HttpClient(), _configMock.Object);
        }

        [Fact]
        public async Task GetCatByIdAsync_ReturnsCorrectCat()
        {
            // Arrange
            var testCat = new Cat { Id = 1, CatId = "test123", ImageUrl = "https://test.com/cat.jpg" };
            await _dbContext.Cats.AddAsync(testCat);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _catService.GetCatByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test123", result.CatId);
        }

        [Fact]
        public async Task GetCatsAsync_ReturnsCatsWithPagination()
        {
            // Arrange
            for (int i = 1; i <= 20; i++)
            {
                await _dbContext.Cats.AddAsync(new Cat { Id = i, CatId = $"cat{i}", ImageUrl = $"https://test.com/cat{i}.jpg" });
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _catService.GetCatsAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public async Task GetCatsByTagAsync_ReturnsCorrectFilteredResults()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Playful" };
            var cat1 = new Cat { Id = 1, CatId = "cat1", ImageUrl = "https://test.com/cat1.jpg", Tags = new List<Tag> { tag } };
            var cat2 = new Cat { Id = 2, CatId = "cat2", ImageUrl = "https://test.com/cat2.jpg" };

            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.Cats.AddAsync(cat1);
            await _dbContext.Cats.AddAsync(cat2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _catService.GetCatsAsync("Playful", 1, 10);

            // Assert
            Assert.Single(result);
            Assert.Equal("cat1", result.First().CatId);
        }
    }
}
