using Rater.API;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        Task<List<ParticipantResponseDto>> CreateParticipants(List<ParticipantRequestDto> request);
        Task<List<Participant>> GetParticipants(int space_id);
    }
}
