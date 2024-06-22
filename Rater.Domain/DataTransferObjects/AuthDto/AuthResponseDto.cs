using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.AuthDto
{
    public class AuthResponseDto
    {
        public bool Success { get; set; } = false;
        public int spaceId { get; set; }
        public string jwtToken { get; set; } = string.Empty;
    }
}
