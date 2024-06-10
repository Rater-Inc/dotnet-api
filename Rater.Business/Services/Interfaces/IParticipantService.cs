using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<List<ParticipantResponseDto>> CreateParticipants(List<ParticipantRequestDto> request);
        Task<List<ParticipantResponseDto>> GetParticipants(int space_id);
    }
}
