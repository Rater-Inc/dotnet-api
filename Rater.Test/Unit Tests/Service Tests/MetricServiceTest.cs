using FakeItEasy;
using FluentAssertions;
using Rater.API;
using Rater.Business.Services;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories;
using Rater.Data.Repositories.Interfaces;
using System.Xml.Linq;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class MetricServiceTest
    {
        private readonly IMetricRepository _metricRepository;
        private readonly MetricService _sut;
        public MetricServiceTest()
        {
            _metricRepository = A.Fake<IMetricRepository>();

            //SUT

            _sut = new MetricService(_metricRepository);
        }

        [Fact]
        public async Task MetricService_GetMetrics_WhenMetricsAreRetrieved()
        {
            //Arrange

            int space_id = 1;

            var expectedMetrics = new List<Metric>
            {
                new Metric
                {
                MetricId = 1,
                Name = "metric 1",
                Description = "desc1",
                Ratings = new List<Rating>(),
                SpaceId = space_id,
                Space = null!
                },
                new Metric
                {
                MetricId = 2,
                Name = "metric 2",
                Description = "desc2",
                Ratings = new List<Rating>(),
                SpaceId = space_id,
                Space = null!
                }
                
            };

            A.CallTo(() => _metricRepository.GetAllMetrics(space_id)).Returns(Task.FromResult(expectedMetrics));
            //Act

            var result = await _sut.GetMetrics(space_id);


            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedMetrics, options => options
                           .Excluding(m => m.Space)
                           .Excluding(m => m.Ratings));
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(metric => metric.SpaceId.Should().Be(space_id));

        }


        [Fact]
        public async Task MetricService_GetMetrics_WhenMetricsAreNull()
        {
            //Arrange

            int space_id = 99999;
            A.CallTo(() => _metricRepository.GetAllMetrics(space_id)).Returns(Task.FromResult(new List<Metric>()));

            //Act & Assert

            await _sut.Invoking(s => s.GetMetrics(space_id))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Couldn't retrieve metrics.");

        }

        [Fact]
        public async Task MetricService_GetMetricsGivenIds_GetMetricsGivenIds_ReturnsThreeCorrectMetricsWithThreeValidIds()
        {
            //Arrange

            var list = new List<int> { 1, 2, 3 };

            var expectedMetrics = new List<Metric>
            {
                new Metric
                {
                MetricId = 1,
                Name = "metric 1",
                Description = "desc1",
                Ratings = new List<Rating>(),
                SpaceId = 1,
                Space = null!
                },
                new Metric
                {
                MetricId = 2,
                Name = "metric 2",
                Description = "desc2",
                Ratings = new List<Rating>(),
                SpaceId = 1,
                Space = null!
                },
                new Metric
                {
                MetricId = 3,
                Name = "metric 2",
                Description = "desc2",
                Ratings = new List<Rating>(),
                SpaceId = 1,
                Space = null!
                }
            };

            A.CallTo(() => _metricRepository.GetMetricsGivenIds(list)).Returns(Task.FromResult(expectedMetrics));

            //Act

            var result = await _sut.GetMetricsGivenIds(list);

            //Assert

            A.CallTo(() => _metricRepository.GetMetricsGivenIds(list)).MustHaveHappenedOnceExactly();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedMetrics, options => options
                           .Excluding(m => m.Space)
                           .Excluding(m => m.Ratings));
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task MetricService_GetMetricsGivenIds_WhenMetricsAreEmpty()
        {
            //Arrange

            var list = new List<int> { 1,2,3 };
            var expectedMetrics = new List<Metric>();
                

            A.CallTo(() => _metricRepository.GetMetricsGivenIds(list)).Returns(Task.FromResult(expectedMetrics));

            //Act && Assert

            await _sut.Invoking(s => s.GetMetricsGivenIds(list))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Couldn't retrieve metrics.");
        }
    }
}
