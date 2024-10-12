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

namespace AyBorg.Authorization;

/// <summary>
/// Token generator interface
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// Generate user token
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="roles">Roles</param>
    /// <returns>Token</returns>
    string GenerateUserToken(string userName, IEnumerable<string> roles);

    /// <summary>
    /// Generate service token
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="serviceUniqueName">Service unique name</param>
    /// <param name="version">Version</param>
    /// <returns>Token</returns>
    string GenerateServiceToken(string serviceName, string serviceUniqueName, string version);
}
