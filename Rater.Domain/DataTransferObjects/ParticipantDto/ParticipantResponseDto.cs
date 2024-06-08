using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.ParticipantDto
{
    public class ParticipantResponseDto
    {
        public int ParticipantId { get; set; }

        public string ParticipantName { get; set; } = null!;
    }
}
