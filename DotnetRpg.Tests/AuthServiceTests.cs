using DotnetRpg.Models.Exceptions;
using DotnetRpg.Services.AuthService;
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
        
        mockSection.Setup(section => section["TokenSettings:Secret"]).Returns("SomeValue");
        mockConfiguration.Setup(config => config.GetSection("SectionKey")).Returns(mockSection.Object);

        var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new DataContext(dbContextOptions);
        _sut = new AuthService(_dbContext, mockConfiguration.Object);
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

        var result = await _sut.Register(userName, password);

        var users = await _dbContext.Users.ToListAsync();

        var newUser = users.First();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data, Is.GreaterThan(0));
        Assert.That(newUser, Is.Not.Null);
    }

    [Test]
    public async Task Register_ExistingUser_ThrowsConflictException()
    {
        var userName = GetStringWithLength(AuthService.userNameMinLength);
        var password = GetStringWithLength(AuthService.passwordMinLength);

        await _sut.Register(userName, password);

        Assert.ThrowsAsync<ConflictException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_UserNameTooShort_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.userNameMinLength - 1); ;
        var password = GetStringWithLength(AuthService.passwordMinLength);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_UserNameTooLong_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.userNameMaxLength + 1);
        var password = GetStringWithLength(AuthService.passwordMinLength);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_PasswordTooShort_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.userNameMinLength);
        var password = GetStringWithLength(AuthService.passwordMinLength - 1);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void Register_PasswordTooLong_ThrowsBadRequestException()
    {
        var userName = GetStringWithLength(AuthService.userNameMinLength);
        var password = GetStringWithLength(AuthService.passwordMaxLength + 1);

        Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Register(userName, password));
    }

    [Test]
    public void CredentialsLengthConstraints_AreValid()
    {
        Assert.Multiple(() =>
        {
            Assert.That(AuthService.userNameMinLength, Is.LessThanOrEqualTo(AuthService.userNameMaxLength));
            Assert.That(AuthService.passwordMinLength, Is.LessThanOrEqualTo(AuthService.passwordMaxLength));
        });
    }

    private static string GetStringWithLength(int length) => string.Join("", Enumerable.Repeat("a", length));
}
