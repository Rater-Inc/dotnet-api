﻿using AutoMapper;
using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.Data.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly DBBContext _context;
        private readonly IMapper _mapper;
        public ParticipantRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ParticipantResponseDto>> CreateParticipants(List<ParticipantRequestDto> request)
        {
            var participants = request.Select(e => _mapper.Map<Participant>(e)).ToList();
            await _context.AddRangeAsync(participants);
            await _context.SaveChangesAsync();

            var result = participants.Select(e => _mapper.Map<ParticipantResponseDto>(e)).ToList();
            return result;


        }

        public async Task<List<Participant>?> GetParticipants(int space_id)
        {
            var participants = await _context.Participants.Where(e => e.SpaceId == space_id).ToListAsync();
            return participants.Any() ? participants : null;
        }

        public async Task<List<Participant>> GetParticipantsGivenIds(List<int> participantIds)
        {
            var value = await _context.Participants
                            .Where(p => participantIds.Contains(p.ParticipantId))
                            .ToListAsync();
            return value;
        }

    }
}
