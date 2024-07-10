using DotnetRpg.Models.Exceptions;
using DotnetRpg.Services.AuthService;
using DotnetRpg.Services.UserProvider;
using Microsoft.Extensions.Configuration;

namespace DotnetRpg.Tests;

[TestFixture]
public class AuthServiceTests
{
    private AuthService _sut = default!;
    private DataContext _dbContext = default!;

    [SetUp]
    public void Setup()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        var mockUserProvider = new Mock<IUserProvider>();
        
        mockSection.Setup(section => section["TokenSettings:Secret"]).Returns("SomeValue");
        mockConfiguration.Setup(config => config.GetSection("SectionKey")).Returns(mockSection.Object);

        var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new DataContext(dbContextOptions, mockUserProvider.Object);
        _sut = new AuthService(_dbContext, mockConfiguration.Object, mockUserProvider.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task Register_NewUser_Success()
    {
        var userName = "newUser";
        var password = "password";

        await _sut.Register(userName, password);

        var users = await _dbContext.Users.ToListAsync();

        var newUser = users.First();
        
        Assert.That(newUser, Is.Not.Null);
    }

    [Test]
    public async Task Register_ExistingUser_ThrowsConflictException()
    {
        var userName = GetStringWithLength(AuthService.UserNameMinLength);
        var password = GetStringWithLength(AuthService.PasswordMinLength);

        await _sut.Register(userName, password);

        Assert.ThrowsAsync<ConflictException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_UserNameTooShort_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.UserNameMinLength - 1);
        var password = GetStringWithLength(AuthService.PasswordMinLength);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_UserNameTooLong_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.UserNameMaxLength + 1);
        var password = GetStringWithLength(AuthService.PasswordMinLength);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_PasswordTooShort_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.UserNameMinLength);
        var password = GetStringWithLength(AuthService.PasswordMinLength - 1);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_PasswordTooLong_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.UserNameMinLength);
        var password = GetStringWithLength(AuthService.PasswordMaxLength + 1);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void CredentialsLengthConstraints_AreValid()
    {
        Assert.Multiple(() =>
        {
            Assert.That(AuthService.UserNameMinLength, Is.LessThanOrEqualTo(AuthService.UserNameMaxLength));
            Assert.That(AuthService.PasswordMinLength, Is.LessThanOrEqualTo(AuthService.PasswordMaxLength));
        });
    }

    private static string GetStringWithLength(int length) => string.Join("", Enumerable.Repeat("a", length));
}
