using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FakeItEasy;
using FluentAssertions;
using Rater.API;
using Rater.Business.Services;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class SpaceServiceTest
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly IUserService _userService;
        private readonly IMetricService _metricService;
        private readonly IRatingService _ratingService;
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        private readonly SpaceService _sut;
        public SpaceServiceTest()
        {
            _spaceRepository = A.Fake<ISpaceRepository>();
            _userService = A.Fake<IUserService>();
            _metricService = A.Fake<IMetricService>();
            _ratingService = A.Fake<IRatingService>();
            _participantService = A.Fake<IParticipantService>();
            _mapper = A.Fake<IMapper>();

            //SUT 

            _sut = new SpaceService(_spaceRepository, _userService, _metricService, _ratingService, _participantService, _mapper);
        }



        [Fact]
        public async Task SpaceService_AddSpace_CreatesSpaceSuccessfully()
        {

            //Arrange

            var request = new GrandSpaceRequestDto
            {
                creatorNickname = "creator nick",
                Name = "Test space",
                Description = "Test space description",
                IsLocked = false,
                Password = "password",
                Metrics = new List<MetricRequestDto>
                {
                    new MetricRequestDto { Name = "metric1", Description = "metric 1 desc" },
                    new MetricRequestDto { Name = "metric2", Description = "metric 2 desc" }
                },
                Participants = new List<ParticipantRequestDto>
                {
                    new ParticipantRequestDto { ParticipantName = "participant1"}
                }
            };

            var mappedSpace = new Space
            {
                Name = request.Name,
                Description = request.Description,
                Creator = new User
                {
                    Nickname = request.creatorNickname
                },
                CreatorId = 1,
                IsLocked = false,
                Password = request.Password,
                Metrics = new List<Metric>
                {
                    new Metric { Name = "metric1" , Description = "metric 1 desc" },
                    new Metric { Name = "metric2" , Description = "metric 2 desc" }
                },
                Participants = new List<Participant>
                {
                    new Participant { ParticipantName = "participant1"}
                }
            };

            var expectedResult = new SpaceResponseDto
            {
                SpaceId = 1,
                CreatorId = 1,
                Name = request.Name,
                Description = request.Description,
                IsLocked = false,
                CreatedAt = DateTime.Now,
                Metrics = new List<MetricResponseDto>
                {
                    new MetricResponseDto { MetricId = 1 , Name = "metric1" , Description = "metric 1 desc" },
                    new MetricResponseDto { MetricId = 2 , Name = "metric2" , Description = "metric 2 desc" }
                },
                Participants = new List<ParticipantResponseDto>
                {
                    new ParticipantResponseDto { ParticipantId = 1, ParticipantName = "participant1"}
                }

            };

            var responseNick = request.creatorNickname;
            var justCreatedUser = new UserResponseDto { Nickname = responseNick, UserId = 1, CreatedAt = DateTime.Now };

            A.CallTo(() => _userService.CreateUser(A<UserRequestDto>._)).Returns(Task.FromResult(justCreatedUser));
            A.CallTo(() => _mapper.Map<Space>(request)).Returns(mappedSpace);

            Space? capturedSpace = null;
            A.CallTo(() => _spaceRepository.CreateSpace(A<Space>._))
                                            .Invokes((Space s) =>
                                            {
                                                capturedSpace = s;
                                                expectedResult.Link = s.Link;
                                            })
                                            .Returns(expectedResult);

            //Act
            var result = await _sut.AddSpace(request);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

            BCrypt.Net.BCrypt.Verify(request.Password, capturedSpace!.Password).Should().BeTrue();

            capturedSpace.Should().NotBeNull();
            capturedSpace!.CreatorId.Should().Be(justCreatedUser.UserId);
            capturedSpace.Password.Should().NotBe(request.Password); // Check if password was hashed
            capturedSpace.Link.Should().NotBeNullOrEmpty();

            A.CallTo(() => _userService.CreateUser(A<UserRequestDto>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<Space>(A<GrandSpaceRequestDto>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _spaceRepository.CreateSpace(A<Space>._)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task SpaceService_GetSpace_ReturnsCorrectValue()
        {
            //Arrange
            var requestLink = "9gxm7191y29";

            var space = new Space
            {
                SpaceId = 1,
                Name = "Space 1",
                Description = "Space 1 Desc",
                Link = requestLink,
                Creator = new User
                {
                    Nickname = "creator nick"
                },
                CreatorId = 1,
                IsLocked = false,
                Password = "password",
                Metrics = new List<Metric>
                {
                    new Metric { Name = "metric1" , Description = "metric 1 desc" },
                    new Metric { Name = "metric2" , Description = "metric 2 desc" }
                },
                Participants = new List<Participant>
                {
                    new Participant { ParticipantName = "participant1"}
                },
                CreatedAt = DateTime.UtcNow,
            };

            var expectedResult = new SpaceResponseDto
            {
                SpaceId = space.SpaceId,
                CreatorId = space.CreatorId,
                Name = space.Name,
                Description = space.Description,
                IsLocked = space.IsLocked,
                Link = space.Link,
                Metrics = new List<MetricResponseDto>
                {
                    new MetricResponseDto {Name = space.Metrics.First().Name , Description = space.Metrics.First().Description},
                    new MetricResponseDto {Name = space.Metrics.Skip(1).First().Name , Description = space.Metrics.Skip(1).First().Description}
                },
                Participants = new List<ParticipantResponseDto>
                {
                    new ParticipantResponseDto {ParticipantName = space.Participants.First().ParticipantName}
                },
                CreatedAt = space.CreatedAt
            };

            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).Returns(space);
            A.CallTo(() => _mapper.Map<SpaceResponseDto>(space)).Returns(expectedResult);

            //Act
            var result = await _sut.GetSpace(requestLink);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

            result.Metrics.Should().HaveCount(2);
            result.Metrics.Should().BeEquivalentTo(expectedResult.Metrics);

            result.Participants.Should().HaveCount(1);
            result.Participants.Should().BeEquivalentTo(expectedResult.Participants);

            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<SpaceResponseDto>(space)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_GetSpace_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var requestLink = "invalid_link";
            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).Throws(new UnauthorizedAccessException("Access denied"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.GetSpace(requestLink));
            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_GetSpace_ThrowsGenericException()
        {
            // Arrange
            var requestLink = "error_link";
            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).Throws(new Exception("Generic error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _sut.GetSpace(requestLink));
            A.CallTo(() => _spaceRepository.GetSpaceByLink(requestLink)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_SpaceExist_ReturnsTrue_WhenSpaceExists()
        {
            // Arrange
            int spaceId = 1;
            A.CallTo(() => _spaceRepository.SpaceExist(spaceId)).Returns(true);

            // Act
            var result = await _sut.SpaceExist(spaceId);

            // Assert
            result.Should().BeTrue();
            A.CallTo(() => _spaceRepository.SpaceExist(spaceId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_SpaceExist_ReturnsFalse_WhenSpaceDoesNotExist()
        {
            // Arrange
            int spaceId = 999;
            A.CallTo(() => _spaceRepository.SpaceExist(spaceId)).Returns(false);

            // Act
            var result = await _sut.SpaceExist(spaceId);

            // Assert
            result.Should().BeFalse();
            A.CallTo(() => _spaceRepository.SpaceExist(spaceId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_GetSpaceByLink_ReturnsSpace_WhenLinkIsValid()
        {
            // Arrange
            string link = "valid_link";
            var expectedSpace = new Space
            {
                SpaceId = 1,
                Name = "Space 1",
                Description = "Space 1 Desc",
                Link = "dasda123sd",
                Creator = new User
                {
                    Nickname = "creator nick"
                },
                CreatorId = 1,
                IsLocked = false,
                Password = "password",
                Metrics = new List<Metric>
                {
                    new Metric { Name = "metric1" , Description = "metric 1 desc" },
                    new Metric { Name = "metric2" , Description = "metric 2 desc" }
                },
                Participants = new List<Participant>
                {
                    new Participant { ParticipantName = "participant1"}
                },
                CreatedAt = DateTime.UtcNow,
            };
            A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Returns(expectedSpace);

            // Act
            var result = await _sut.GetSpaceByLink(link);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedSpace);
            A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SpaceService_GetSpaceByLink_ReturnsNull_WhenLinkIsInvalid()
        {
            // Arrange
            string link = "invalid_link";
            A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Returns((Space?)null);

            // Act
            var result = await _sut.GetSpaceByLink(link);

            // Assert
            result.Should().BeNull();
            A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).MustHaveHappenedOnceExactly();
        }

    [Fact]
    public async Task GetSpaceResults_ValidLink_ReturnsGrandResultResponseDto()
    {
        // Arrange
        var link = "valid_link";
        var space = new Space { SpaceId = 1, Name = "Test Space" };
        var metrics = new List<Metric> 
        { 
            new Metric { MetricId = 1, Name = "Metric 1" },
            new Metric { MetricId = 2, Name = "Metric 2" }
        };
        var participants = new List<Participant>
        {
            new Participant { ParticipantId = 1, ParticipantName = "Participant 1" },
            new Participant { ParticipantId = 2, ParticipantName = "Participant 2" }
        };
        var ratings = new List<Rating>
        {
            new Rating { MetricId = 1, RateeId = 1, Score = 4 },
            new Rating { MetricId = 1, RateeId = 2, Score = 5 },
            new Rating { MetricId = 2, RateeId = 1, Score = 3 },
            new Rating { MetricId = 2, RateeId = 2, Score = 4 }
        };

        A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Returns(space);
        A.CallTo(() => _metricService.GetMetrics(space.SpaceId)).Returns(metrics);
        A.CallTo(() => _participantService.GetParticipants(space.SpaceId)).Returns(participants);
        A.CallTo(() => _ratingService.GetRatings(space.SpaceId)).Returns(ratings);
        A.CallTo(() => _mapper.Map<MetricLeaderDto>(A<Metric>._))
            .Returns(new MetricLeaderDto { Id = 1, Name = "Metric 1" });
        A.CallTo(() => _mapper.Map<PariticipantResultDto>(A<Participant>._))
            .Returns(new PariticipantResultDto { ParticipantId = 1, ParticipantName = "Participant 1" });
        A.CallTo(() => _mapper.Map<ParticipantResponseDto>(A<Participant>._))
            .Returns(new ParticipantResponseDto { ParticipantId = 1, ParticipantName = "Participant 1" });
        A.CallTo(() => _mapper.Map<ParticipantResultMetricDto>(A<Metric>._))
            .Returns(new ParticipantResultMetricDto { MetricId = 1, Name = "Metric 1" });

        // Act
        var result = await _sut.GetSpaceResults(link);

        // Assert
        result.Should().NotBeNull();
        result.SpaceId.Should().Be(space.SpaceId);
        result.Name.Should().Be(space.Name);
        result.MetricLeaders.Should().HaveCount(2);
        result.ParticipantResults.Should().HaveCount(2);
        A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _metricService.GetMetrics(space.SpaceId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _participantService.GetParticipants(space.SpaceId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _ratingService.GetRatings(space.SpaceId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetSpaceResults_InvalidLink_ThrowsInvalidOperationException()
    {
        // Arrange
        var link = "invalid_link";
            A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Returns(Task.FromResult<Space?>(null));

            // Act & Assert
            await _sut.Invoking(s => s.GetSpaceResults(link))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Space not found for the given link.");
    }

    [Fact]
    public async Task GetSpaceResults_UnauthorizedAccess_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var link = "valid_link";
        A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Throws(new UnauthorizedAccessException());

        // Act & Assert
        await _sut.Invoking(s => s.GetSpaceResults(link))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetSpaceResults_GeneralException_ThrowsException()
    {
        // Arrange
        var link = "valid_link";
        A.CallTo(() => _spaceRepository.GetSpaceByLink(link)).Throws(new Exception("Test exception"));

        // Act & Assert
        await _sut.Invoking(s => s.GetSpaceResults(link))
            .Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }


    [Fact]
    public async Task AddRatings_ValidRequest_ReturnsRatingResponseDto()
    {
        // Arrange
        var request = new RatingRequestDto
        {
            SpaceId = 1,
            RaterNickName = "TestRater",
            RatingDetails = new List<RatingDetailDto>
            {
                new RatingDetailDto { MetricId = 1, RateeId = 1, Score = 5 },
                new RatingDetailDto { MetricId = 2, RateeId = 2, Score = 4 }
            }
        };

        var metrics = new List<Metric>
        {
            new Metric { MetricId = 1, SpaceId = 1 },
            new Metric { MetricId = 2, SpaceId = 1 }
        };

        var participants = new List<Participant>
        {
            new Participant { ParticipantId = 1, SpaceId = 1 },
            new Participant { ParticipantId = 2, SpaceId = 1 }
        };

        var user = new User { UserId = 1, Nickname = "TestRater" };

        var expectedResponse = new RatingResponseDto();

        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>.That.IsSameSequenceAs(new List<int> { 1, 2 }))).Returns(metrics);
        A.CallTo(() => _participantService.GetParticipantsGivenIds(A<List<int>>.That.IsSameSequenceAs(new List<int> { 1, 2 }))).Returns(participants);
        A.CallTo(() => _userService.CreateUser(A<UserRequestDto>.That.Matches(u => u.NickName == "TestRater")))
            .Returns(Task.FromResult(new UserResponseDto { UserId = user.UserId, Nickname = user.Nickname }));
        A.CallTo(() => _ratingService.AddRatings(A<List<Rating>>.That.Matches(r => r.Count == 2))).Returns(expectedResponse);

        // Act
        var result = await _sut.AddRatings(request);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _participantService.GetParticipantsGivenIds(A<List<int>>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _userService.CreateUser(A<UserRequestDto>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _ratingService.AddRatings(A<List<Rating>>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddRatings_InvalidMetric_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RatingRequestDto
        {
            SpaceId = 1,
            RaterNickName = "TestRater",
            RatingDetails = new List<RatingDetailDto>
            {
                new RatingDetailDto { MetricId = 1, RateeId = 1, Score = 5 }
            }
        };

        var metrics = new List<Metric>
        {
            new Metric { MetricId = 1, SpaceId = 2 } // Different SpaceId
        };

        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>._)).Returns(metrics);

        // Act & Assert
        await _sut.Invoking(s => s.AddRatings(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The request payload does not match the provided space ID for metric.");
    }

    [Fact]
    public async Task AddRatings_InvalidParticipant_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RatingRequestDto
        {
            SpaceId = 1,
            RaterNickName = "TestRater",
            RatingDetails = new List<RatingDetailDto>
            {
                new RatingDetailDto { MetricId = 1, RateeId = 1, Score = 5 }
            }
        };

        var metrics = new List<Metric>
        {
            new Metric { MetricId = 1, SpaceId = 1 }
        };

        var participants = new List<Participant>
        {
            new Participant { ParticipantId = 1, SpaceId = 2 } // Different SpaceId
        };

        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>._)).Returns(metrics);
        A.CallTo(() => _participantService.GetParticipantsGivenIds(A<List<int>>._)).Returns(participants);

        // Act & Assert
        await _sut.Invoking(s => s.AddRatings(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The request payload does not match the provided space ID for participant.");
    }

    [Fact]
    public async Task AddRatings_UnauthorizedAccess_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var request = new RatingRequestDto
        {
            SpaceId = 1,
            RaterNickName = "TestRater",
            RatingDetails = new List<RatingDetailDto>
            {
                new RatingDetailDto { MetricId = 1, RateeId = 1, Score = 5 }
            }
        };

        var metrics = new List<Metric>
        {
            new Metric { MetricId = 1, SpaceId = 1 }
        };

        var participants = new List<Participant>
        {
            new Participant { ParticipantId = 1, SpaceId = 1 }
        };

        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>._)).Returns(metrics);
        A.CallTo(() => _participantService.GetParticipantsGivenIds(A<List<int>>._)).Returns(participants);
        A.CallTo(() => _userService.CreateUser(A<UserRequestDto>._)).Throws(new UnauthorizedAccessException("Unauthorized"));

        // Act & Assert
        await _sut.Invoking(s => s.AddRatings(request))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Unauthorized");
    }

    [Fact]
    public async Task AddRatings_GeneralException_ThrowsException()
    {
        // Arrange
        var request = new RatingRequestDto
        {
            SpaceId = 1,
            RaterNickName = "TestRater",
            RatingDetails = new List<RatingDetailDto>
            {
                new RatingDetailDto { MetricId = 1, RateeId = 1, Score = 5 }
            }
        };

        A.CallTo(() => _metricService.GetMetricsGivenIds(A<List<int>>._)).Throws(new Exception("Unexpected error"));

        // Act & Assert
        await _sut.Invoking(s => s.AddRatings(request))
            .Should().ThrowAsync<Exception>()
            .WithMessage("Unexpected error");
    }
    }
}
