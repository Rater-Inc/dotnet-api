using Rater.Business.Services;
using FluentAssertions;
using FakeItEasy;
using Rater.Data.Repositories.Interfaces;
using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class RatingServiceTest
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly RatingService _sut;
        public RatingServiceTest()
        {
            _ratingRepository = A.Fake<IRatingRepository>();

            // SUT
            _sut = new RatingService(_ratingRepository);
        }


        [Fact]
        public async Task RatingService_AddRatings_WhenRepositoryAddsRatingsSuccessfully()
        {
            var requestRatings = new List<Rating>
            {

                //Arrange

                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 1,
                    Score = 5
                },
                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 2,
                    Score = 5
                }
            };

            var expectedResponse = new RatingResponseDto
            {
                ratingCount = 2,
                spaceId = 1,
                success = true
            };

            A.CallTo(() => _ratingRepository.AddRatings(requestRatings)).Returns(Task.FromResult(expectedResponse));

            //Act

            var result = await _sut.AddRatings(requestRatings);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task RatingService_AddRatings_WhenRepositoryThrowsInvalidOperationException()
        {
            //Arrange

            var requestRatings = new List<Rating>
            {

                //Arrange

                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 1,
                    Score = 5
                },
                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 2,
                    Score = 5
                }
            };

            A.CallTo(() => _ratingRepository.AddRatings(requestRatings)).Throws<InvalidOperationException>();

            //Act && Assert

            await _sut.Invoking(s => s.AddRatings(requestRatings))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task RatingService_AddRatings_WhenRepositoryThrowsUnAuthorizedAccessException()
        {
            //Arrange

            var requestRatings = new List<Rating>
            {

                //Arrange

                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 1,
                    Score = 5
                },
                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 2,
                    Score = 5
                }
            };

            A.CallTo(() => _ratingRepository.AddRatings(requestRatings)).Throws<UnauthorizedAccessException>();

            //Act && Assert

            await _sut.Invoking(s => s.AddRatings(requestRatings))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task RatingService_AddRatings_WhenRepositoryThrowsGenericException()
        {
            //Arrange

            var requestRatings = new List<Rating>
            {

                //Arrange

                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 1,
                    Score = 5
                },
                new Rating
                {
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 2,
                    Score = 5
                }
            };

            A.CallTo(() => _ratingRepository.AddRatings(requestRatings)).Throws<Exception>();

            //Act && Assert

            await _sut.Invoking(s => s.AddRatings(requestRatings))
                .Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RatingService_GetRatings_WhenRepositoryReturnsRatings()
        {
            //Arrange
            var space_id = 35;

            var expectedResult = new List<Rating>
            {

                //Arrange

                new Rating
                {
                    RatingId = 1,
                    SpaceId = space_id,
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 1,
                    Score = 5
                },
                new Rating
                {
                    RatingId = 2,
                    SpaceId= space_id,
                    RaterId = 1,
                    MetricId = 1,
                    RateeId = 2,
                    Score = 5
                }
            };

            A.CallTo(() => _ratingRepository.GetRatings(space_id)).Returns(Task.FromResult(expectedResult));


            //Act

            var result = await _sut.GetRatings(space_id);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(rating => rating.SpaceId.Should().Be(space_id));
        }


        [Fact]
        public async Task RatingService_GetRatings_WhenRepositoryReturnsEmpty()
        {
            //Arrange

            var space_id = 1;
            var expectedResult = new List<Rating>();

            A.CallTo(() => _ratingRepository.GetRatings(space_id)).Returns(Task.FromResult(expectedResult));

            //Act

            var result = await _sut.GetRatings(space_id);

            //Assert

            result.Should().BeEmpty();
        }
    }
}
