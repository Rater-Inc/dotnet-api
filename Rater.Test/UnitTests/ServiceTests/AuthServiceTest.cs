using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Rater.API;
using Rater.Business.Services;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.AuthDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class AuthServiceTest
    {
        private readonly AuthService _sut;
        private readonly ISpaceService _spaceService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public AuthServiceTest()
        {
            _spaceService = A.Fake<ISpaceService>();

            //var inMemorySettings = new Dictionary<string, string?>
            //{
            //    {"jwt:Token", "75e3d0b0db6c6a616472953fc4a63921484644e92f34f0867f4500be96576c803ed2405828712c4ed8be43eec1a246915682e0e96b6b0b231113c4772530a761"},
            //    {"jwt:jwtTokenTTL", "24"}
            //};

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();


            //SUT
            _sut = new AuthService(_spaceService, _configuration);
        }

        [Fact]
        public async Task AuthService_AuthLobby_ValidRequests()
        {
            //Arrange

            var link = "1l7n91ssyi";
            var password = "string";

            Space expectedResult = new Space
            {
                SpaceId = 1,
                Name = "space 1",
                Description = "space description",
                Creator = new User { UserId = 1, Nickname = "Creator", CreatedAt = DateTime.UtcNow },
                Link = link,
                Password = "$2a$11$TTheC6LG3HOwRy1TguQ2o.2L3M0zizScdPw/AG1sn3NqNGPRVHxte",
                IsLocked = true,
                CreatedAt = DateTime.UtcNow
            };


            A.CallTo(() => _spaceService.GetSpaceByLink(link)).Returns(expectedResult);

            //Act

            var result = await _sut.AuthLobby(link, password);

            result.Should().NotBeNull();
            result.Should().BeOfType<AuthResponseDto>();
            result.Success.Should().BeTrue();
            result.spaceId.Should().Be(expectedResult.SpaceId);
            result.jwtToken.Should().NotBeNullOrEmpty();
            result.jwtToken.Should().StartWith("eyJ");
            result.jwtToken.Split('.').Should().HaveCount(3);


        }

        [Fact]
        public async Task AuthService_AuthLobby_InvalidLink()
        {
            //Arrange

            var link = "1l7n91ssyi";
            var password = "string";

            var expectedResult = new AuthResponseDto();

            A.CallTo(() => _spaceService.GetSpaceByLink(link)).Returns(Task.FromResult<Space?>(null));

            //Act

            var result = await _sut.AuthLobby(link, password);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

        }

        [Fact]
        public async Task AuthService_AuthLobby_InvalidPassword()
        {
            //Arrange

            var link = "askodkx123";
            var password = "string";

            Space space = new Space
            {
                SpaceId = 1,
                Name = "space 1",
                Description = "space description",
                Creator = new User { UserId = 1, Nickname = "Creator", CreatedAt = DateTime.UtcNow },
                Link = link,
                Password = "$2a$11$TTheC6LG3HOwRysd1TguQ2o.2L3M0zizScdPw/AG1sn3NqNGPRVHxte",
                IsLocked = true,
                CreatedAt = DateTime.UtcNow
            };

            var expectedResult = new AuthResponseDto();

            A.CallTo(() => _spaceService.GetSpaceByLink(link)).Returns(space);

            //Act

            var result = await _sut.AuthLobby(link, password);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

        }

        [Fact]
        public void AuthService_CreateToken_WhenCalledWithValidSpaceId()
        {
            //Arrange

            int space_id = 1;

            //Act

            var result  = _sut.CreateToken(space_id);

            //Assert

            result.Should().NotBeNullOrEmpty();
            result.Should().StartWith("eyJ");
            result.Split('.').Should().HaveCount(3);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result);

            jwtToken.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.NameIdentifier && c.Value == space_id.ToString());
            jwtToken.Claims.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Jti).Which.Value.Should().NotBeNullOrEmpty();

            jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow, "the token should not be expired");
            jwtToken.ValidTo.Hour.Should().Be(DateTime.UtcNow.AddHours(24).Hour, "the token's expiration hour should match the TTL value");

        }
    }
}
