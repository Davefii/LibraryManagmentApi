using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IRefreshTokenService
    {
        Task<User?> ValidateRefreshToken(string refreshToken);
        Task RevokeToken(string refreshToken);
        Task AddAsync(RefreshToken refreshTokenEntity);
    }
}
