using AutoMapper;
using DotnetRpg.AutoMapper;
using DotnetRpg.Services.CharacterService;
using DotnetRpg.Services.UserProvider;

namespace DotnetRpg.Tests;

public class CharacterServiceTests
{
    private CharacterService _sut = default!;
    private DataContext _dbContext = default!;
    private IMapper _mapper = default!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
    }

    [SetUp]
    public void Setup()
    {
        var mockUserProvider = new Mock<IUserProvider>();
        var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new DataContext(dbContextOptions, mockUserProvider.Object);
        _sut = new CharacterService(_mapper, _dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
}
