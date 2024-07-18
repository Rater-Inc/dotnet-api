namespace Rater.Business.Services.Interfaces
{
    public interface IJwtTokenService
    {
        public int GetSpaceIdFromToken();
        Task<bool> CreateToken(string token);
        Task<bool> ValidateToken();
    }
}
