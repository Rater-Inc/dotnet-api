using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.ParticipantDto
{
    public class ParticipantRequestDto
    {
        public int SpaceId { get; set; }

        public string ParticipantName { get; set; } = null!;

    }
}
