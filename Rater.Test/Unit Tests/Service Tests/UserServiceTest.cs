using FakeItEasy;
using FluentAssertions;
using Rater.Business.Services;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class UserServiceTest
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _sut;
        public UserServiceTest()
        {
            _userRepository = A.Fake<IUserRepository>();

            //SUT

            _sut = new UserService(_userRepository);
        }

        [Fact]
        public async Task UserService_CreateUser_ReturnsCorrectValue()
        {
            // Arrange 
            UserRequestDto request = new UserRequestDto()
            {
                NickName = "nickname"
            };
            var response = new UserResponseDto()
            {
                UserId = 1,
                Nickname = request.NickName,
                CreatedAt = DateTime.UtcNow
            };
            var expected = new UserResponseDto()
            {
                UserId = 1,
                Nickname = request.NickName,
                CreatedAt = DateTime.UtcNow
            };
            A.CallTo(() => _userRepository.AddUser(request)).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.CreateUser(request);

            // Assert
            result.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.CreatedAt));
        }

        [Fact]
        public async Task UserService_CreateUser_CalledOnce()
        {
            //Arrange

            UserRequestDto request = new UserRequestDto()
            {
                NickName = "nickname"
            };

            var response = new UserResponseDto()
            {
                UserId = 1,
                Nickname = request.NickName,
                CreatedAt = DateTime.UtcNow
            };
            A.CallTo(() => _userRepository.AddUser(request)).Returns(Task.FromResult(response));


            //Act

            var result = await _sut.CreateUser(request);

            //Assert

            A.CallTo(() => _userRepository.AddUser(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UserService_CreateUser_ThrowsExceptionWhenAddUserFails()
        {
            //Arrange

            UserRequestDto request = new UserRequestDto()
            {
                NickName = "nickname"
            };

            A.CallTo(() => _userRepository.AddUser(request)).Throws(new Exception("repository fail"));

            //Act

            var result = _sut.CreateUser(request);

            //Assert

            await _sut.Invoking(s => s.CreateUser(request))
                .Should().ThrowAsync<Exception>()
                .WithMessage("repository fail");
        }

        [Fact]
        public async Task UserService_CreateUser_WhenRequestIsNull()
        {

            //Arrange

            UserRequestDto request = new UserRequestDto()
            {
                NickName = string.Empty
            };

            var exception = new ArgumentException("Request is empty");
            A.CallTo(() => _userRepository.AddUser(request)).Throws(exception);

            // Act

            Func<Task> act = async () => await _sut.CreateUser(request);

            // Assert

            await _sut.Invoking(s => s.CreateUser(request)).Should().ThrowAsync<ArgumentException>();



        }


    }
}
