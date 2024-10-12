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

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AyBorg.Authorization;

public sealed class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenValidator<JwtSecurityToken> tokenValidator)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ") is string[] parts && parts.Length > 0 ? parts[parts.Length - 1] : null;
        if (string.IsNullOrEmpty(token))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (tokenValidator.Validate(token, out JwtSecurityToken validatedToken))
        {
            validatedToken.Claims.ToList().ForEach(claim => context.User.AddIdentity(new ClaimsIdentity(new[] { claim }, "jwt")));
        }

        await _next(context).ConfigureAwait(false);
    }
}

public static class JwtMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}
