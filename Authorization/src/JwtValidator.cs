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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AyBorg.Authorization;

/// <summary>
/// JWT validator
/// </summary>
public class JwtValidator : ITokenValidator<JwtSecurityToken>
{
    private readonly ILogger<JwtValidator> _logger;
    private readonly IOptions<SecurityConfiguration> _securityConfiguration;
    private readonly byte[] _secretKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtValidator"/> class.
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="securityConfiguration">Security configuration</param>
    public JwtValidator(ILogger<JwtValidator> logger, IOptions<SecurityConfiguration> securityConfiguration)
    {
        _logger = logger;
        _securityConfiguration = securityConfiguration;
        _secretKey = Encoding.ASCII.GetBytes(_securityConfiguration.Value.PrimarySharedKey.KeyValue);
    }

    /// <inheritdoc/>
    public bool Validate(string token, out JwtSecurityToken tokenObject)
    {
        if (string.IsNullOrEmpty(token))
        {
            tokenObject = null!;
            return false;
        }


        if (_securityConfiguration.Value.PrimarySharedKey.Enabled)
        {
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

                tokenObject = (JwtSecurityToken)validatedToken;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to validate token");
                tokenObject = null!;
                return false;
            }
        }
        else
        {
            _logger.LogWarning("Primary shared key is disabled");
            tokenObject = null!;
            return false;
        }
    }
}
