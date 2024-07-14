using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.Models;
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
        Task<List<ParticipantModel>> GetParticipants(int space_id);
    }
}
