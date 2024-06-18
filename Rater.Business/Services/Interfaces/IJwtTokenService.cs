using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface IJwtTokenService
    {
        int GetSpaceIdFromToken();
    }
}
