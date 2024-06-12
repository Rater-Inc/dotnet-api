using Rater.API;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.SpaceDto
{
    public class GrandSpaceRequestDto
    {
        public string? creatorNickname { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsLocked { get; set; } = false!;

        public string? Password { get; set; }

        public virtual ICollection<MetricRequestDto> Metrics { get; set; } = new List<MetricRequestDto>();
        public virtual ICollection<ParticipantRequestDto> Participants { get; set; } = new List<ParticipantRequestDto>();
    }
}
