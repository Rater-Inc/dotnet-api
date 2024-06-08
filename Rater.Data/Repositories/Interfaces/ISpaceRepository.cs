﻿using Rater.API;
using Rater.Domain.DataTransferObjects.SpaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Interfaces
{
    public interface ISpaceRepository
    {

        Task<List<SpaceResponseDto>> GetAllSpaces();


    }
}
