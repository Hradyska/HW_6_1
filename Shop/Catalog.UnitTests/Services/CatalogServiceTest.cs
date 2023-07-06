using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();

        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _catalogBrandRepository.Object, _catalogTypeRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
      }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        PaginatedItems<CatalogItem> item = null!;
        _catalogItemRepository.Setup(s => s.GetByPageAsync(
             It.Is<int>(i => i == testPageIndex),
             It.Is<int>(i => i == testPageSize),
             It.IsAny<int?>(),
             It.IsAny<int?>())).ReturnsAsync(item);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByIdAsync_Success()
    {
        var testId = 1;

        var catalogItemsListSuccess = new GetItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                 new CatalogItem()
                 {
                     Name = "TestName"
                 },
            },
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(catalogItemsListSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByIdAsync_Failed()
    {
        // arrange
        var testId = 1000;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testId))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByBrandAsync_Success()
    {
        var testBrandId = 1;

        var catalogItemsListSuccess = new GetItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                 new CatalogItem()
                 {
                     Name = "TestName",
                     CatalogBrandId = testBrandId,
                 },
            },
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testBrandId))).ReturnsAsync(catalogItemsListSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsByBrandAsync(testBrandId);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByBrandAsync_Failed()
    {
        // arrange
        var testBrandId = 1000;

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testBrandId))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsByBrandAsync(testBrandId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByTypeAsync_Success()
    {
        var testTypeId = 1;

        var catalogItemsListSuccess = new GetItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                 new CatalogItem()
                 {
                     Name = "TestName",
                     CatalogBrandId = testTypeId,
                 },
            },
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testTypeId))).ReturnsAsync(catalogItemsListSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsByTypeAsync(testTypeId);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogItemsByTypeAsync_Failed()
    {
        // arrange
        var testTypeId = 1000;

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testTypeId))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsByTypeAsync(testTypeId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Success()
    {
        // arrange
        var pagingPaginatedBrandsSuccess = new GetBrands<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Brand = "Brand1",
                },
            },
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = "Brand1"
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "Brand1"
        };

        _catalogBrandRepository.Setup(s => s.GetAsync()).ReturnsAsync(pagingPaginatedBrandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Failed()
    {
        _catalogBrandRepository.Setup(s => s.GetAsync()).Returns((Func<BrandsResponse<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Success()
    {
        // arrange
        var pagingPaginatedTypesSuccess = new GetTypes<CatalogType>()
        {
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Type = "Type1",
                },
            },
        };

        var catalogTypesSuccess = new CatalogType()
        {
            Type = "Type1"
        };

        var catalogTypesDtoSuccess = new CatalogTypeDto()
        {
            Type = "Type1"
        };

        _catalogTypeRepository.Setup(s => s.GetAsync()).ReturnsAsync(pagingPaginatedTypesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypesSuccess)))).Returns(catalogTypesDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Failed()
    {
        _catalogTypeRepository.Setup(s => s.GetAsync()).Returns((Func<TypesResponse<CatalogTypeDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().BeNull();
    }
}