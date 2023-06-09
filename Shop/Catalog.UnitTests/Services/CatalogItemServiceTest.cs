using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly ICatalogItemService _catalogItemService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItem _testItem = new CatalogItem()
    {
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png"
    };

    public CatalogItemServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogItemService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Add(_testItem);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Add(_testItem);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.Update(
             It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Update(_testItem);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Update(
            It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Update(_testItem);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.Remove(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Remove(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Remove(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.Remove(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }
}