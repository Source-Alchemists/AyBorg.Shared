/*
 * AyBorg - The new software generation for machine vision, automation and industrial IoT
 * Copyright (C) 2024  Source Alchemists
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the,
 * GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AyBorg.Authorization;

[Obsolete("Use AyBorg.SDK.Authorization.JwtTValidator instead.")]
public sealed class JwtConsumer : IJwtConsumer
{
    private readonly ILogger<JwtConsumer> _logger;
    private readonly byte[] _secretKey;

    public JwtConsumer(ILogger<JwtConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("AyBorg:Jwt:SecretKey"));
    }

    public JwtSecurityToken ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null!;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate token");
            return null!;
        }
    }
}
