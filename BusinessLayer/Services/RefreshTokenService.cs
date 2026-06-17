using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class RefreshTokenService
    {
        private readonly UserRepository _userRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(
                UserRepository userRepository,
                RefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<User?> ValidateRefreshToken(
            string email,
            string refreshToken)
        {
            var user =
                await _userRepository
                    .GetByEmailAsync(email);

            if (user == null)
                return null;

            var tokens =
                await _refreshTokenRepository
                    .GetByUserIdAsync(user.Id);

            var activeToken =
                tokens.FirstOrDefault(t =>
                    t.RevokedAt == null &&
                    t.ExpiresAt > DateTime.UtcNow &&
                    BCrypt.Net.BCrypt.Verify(
                        refreshToken,
                        t.Token));

            if (activeToken == null)
                return null;

            return user;
        }

        public async Task RevokeToken(
            string email,
            string refreshToken)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return;

            var tokens = await _refreshTokenRepository.GetByUserIdAsync(user.Id);

            var token = tokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(refreshToken,t.Token));

            if (token == null)
                return;

            token.RevokedAt =
                DateTime.UtcNow;

            await _refreshTokenRepository
                .UpdateAsync(token);
        }

        public async Task AddAsync(RefreshToken refreshTokenEntity)
        {
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        }


    }
}
