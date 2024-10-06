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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AyBorg.Communication;

public record ServiceConfiguration : GatewayConfiguration, IServiceConfiguration
{
    private readonly ILogger<ServiceConfiguration> _logger;

    public string GatewayUrl { get; }

    public ServiceConfiguration(ILogger<ServiceConfiguration> logger, IConfiguration configuration) : base(logger, configuration)
    {
        _logger = logger;
        string? registryUrl = configuration.GetValue<string>("AyBorg:Gateway:Url");
        if (string.IsNullOrEmpty(registryUrl))
        {
            if (configuration.GetValue<string>("AyBorg:Service:Type")?.Equals(ServiceTypes.Gateway) != true)
            {
                _logger.LogWarning("Registry url is not set in configuration. Using default value. (Hint: AyBorg:Gateway:Url)");
            }
            GatewayUrl = "http://localhost:5000";
        }
        else
        {
            GatewayUrl = registryUrl;
        }
    }
}
