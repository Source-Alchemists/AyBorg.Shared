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
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AyBorg.Authorization;

public sealed class JwtGenerator : ITokenGenerator
{
    private readonly byte[] _secretKey;

    public JwtGenerator(IOptions<SecurityConfiguration> configuration)
    {
        _secretKey = Encoding.ASCII.GetBytes(configuration.Value.PrimarySharedKey.KeyValue);
    }

    public string GenerateUserToken(string userName, IEnumerable<string> roles)
    {
        return GenerateToken(userName, roles);
    }

    public string GenerateServiceToken(string serviceName, IEnumerable<string> roles)
    {
        return GenerateToken(serviceName, roles);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GenerateToken(string name, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim> {
                new(ClaimTypes.Name, name),
            };
        if (roles.Any())
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
