﻿using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricResponseForResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public double Score { get; set; }

        public ParticipantResponseDto? Winner { get; set; }

    }
}
