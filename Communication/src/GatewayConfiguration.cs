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

using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AyBorg.Communication;

public record GatewayConfiguration : IGatewayConfiguration
{
    private readonly ILogger<GatewayConfiguration> _logger;

    public string DisplayName { get; }

    public string UniqueName { get; }

    public string TypeName { get; }

    public string Version { get; }

    public bool IsAuditRequired { get; }

    public GatewayConfiguration(ILogger<GatewayConfiguration> logger, IConfiguration configuration)
    {
        _logger = logger;
        AssemblyName assemblyName = Assembly.GetEntryAssembly()!.GetName();
        string? serviceUniqueName = configuration.GetValue<string>("AyBorg:Service:UniqueName");
        if (string.IsNullOrEmpty(serviceUniqueName))
        {
            _logger.LogWarning("Service unique name is not set in configuration. Using default value. (Hint: AyBorg:Service:UniqueName)");
            UniqueName = assemblyName.Name!;
        }
        else
        {
            UniqueName = serviceUniqueName;
        }

        string? serviceTypeName = configuration.GetValue<string>("AyBorg:Service:Type");
        if (string.IsNullOrEmpty(serviceTypeName))
        {
            _logger.LogWarning("Service type name is not set in configuration. Using default value. (Hint: AyBorg:Service:Type)");
            TypeName = assemblyName.Name!;
        }
        else
        {
            TypeName = serviceTypeName;
        }

        string? serviceDisplayName = configuration.GetValue<string>("AyBorg:Service:DisplayName");
        if (string.IsNullOrEmpty(serviceDisplayName))
        {
            _logger.LogWarning("Service display name is not set in configuration. Using default value. (Hint: AyBorg:Service:DisplayName)");
            DisplayName = assemblyName.Name!;
        }
        else
        {
            DisplayName = serviceDisplayName;
        }

        IsAuditRequired = configuration.GetValue<bool>("AyBorg:Service:Audit:Required", true);

        Version = assemblyName.Version!.ToString();
    }
}
