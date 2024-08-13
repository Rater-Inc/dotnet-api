using FakeItEasy;
using FluentAssertions;
using Rater.API;
using Rater.Business.Services;
using Rater.Data.Repositories;
using Rater.Data.Repositories.Interfaces;

namespace Rater.Test.Unit_Tests.Service_Tests
{
    public class ParticipantServiceTest
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ParticipantService _sut;

        public ParticipantServiceTest()
        {
            _participantRepository = A.Fake<IParticipantRepository>();

            //SUT

            _sut = new ParticipantService(_participantRepository);
        }

        [Fact]
        public async Task PariticpantService_GetParticipants_WhenParticipantsAreRetrieved()
        {
            //Arrange

            int space_id = 1;

            var expectedParticipants = new List<Participant>
            {
                new Participant
                {
                    ParticipantId = 1,
                    ParticipantName = "participant 1",
                    Ratings = new List<Rating>(),
                    Space = null!,
                    SpaceId = space_id
                },
                new Participant
                {
                    ParticipantId = 2,
                    ParticipantName = "participant 2",
                    Ratings = new List<Rating>(),
                    Space = null!,
                    SpaceId = space_id
                }
            };

            A.CallTo(() => _participantRepository.GetAllParticipants(space_id)).Returns(expectedParticipants);

            //Act

            var result = await _sut.GetParticipants(space_id);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedParticipants, options => options
                           .Excluding(m => m.Space)
                           .Excluding(m => m.Ratings));
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(participant => participant.SpaceId.Should().Be(space_id));
        }

        [Fact]
        public async Task ParticipantService_GetParticipants_WhenParticipantsAreNull()
        {
            //Arrange

            int space_id = 99999;
            A.CallTo(() => _participantRepository.GetAllParticipants(space_id)).Returns(Task.FromResult(new List<Participant>()));

            //Act & Assert

            await _sut.Invoking(s => s.GetParticipants(space_id))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Couldn't retrieve participants.");
        }

        [Fact]
        public async Task ParticipantService_GetParticipantsGivenIds_GetParticipantsGivenIds()
        {
            //Arrange

            var list = new List<int> { 1,2,3 };

            var expectedParticipants = new List<Participant>
            {
                                new Participant
                {
                    ParticipantId = 1,
                    ParticipantName = "participant 1",
                    Ratings = new List<Rating>(),
                    Space = null!,
                    SpaceId = 1
                },
                new Participant
                {
                    ParticipantId = 2,
                    ParticipantName = "participant 2",
                    Ratings = new List<Rating>(),
                    Space = null!,
                    SpaceId = 1
                },
                new Participant
                {
                    ParticipantId = 3,
                    ParticipantName = "participant 3",
                    Ratings = new List<Rating>(),
                    Space = null!,
                    SpaceId = 1
                }
            };

            A.CallTo(() => _participantRepository.GetParticipantsGivenIds(list)).Returns(Task.FromResult(expectedParticipants));

            //Act

            var result = await _sut.GetParticipantsGivenIds(list);

            //Assert

            A.CallTo(() => _participantRepository.GetParticipantsGivenIds(list)).MustHaveHappenedOnceExactly();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedParticipants, options => options
                           .Excluding(m => m.Space)
                           .Excluding(m => m.Ratings));
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task MetricService_GetMetricsGivenIds_WhenParticipantsAreEmpty()
        {
            //Arrange

            var list = new List<int> { 1, 2, 3 };
            var expectedParticipants = new List<Participant>();


            A.CallTo(() => _participantRepository.GetParticipantsGivenIds(list)).Returns(Task.FromResult(expectedParticipants));

            //Act && Assert

            await _sut.Invoking(s => s.GetParticipantsGivenIds(list))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Couldn't retrieve participants.");
        }

    }
}
