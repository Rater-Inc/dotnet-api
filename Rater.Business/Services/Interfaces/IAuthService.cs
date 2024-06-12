using Rater.Domain.DataTransferObjects.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthLobby(string link, string password);
        string CreateToken();
    }
}
