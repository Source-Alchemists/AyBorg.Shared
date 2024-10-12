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

using Microsoft.Extensions.Logging;

namespace AyBorg.Communication.MQTT;

/// <summary>
/// Factory for creating MQTT client provider.
/// </summary>
public interface IMqttClientProviderFactory
{
    /// <summary>
    /// Creates the MQTT client provider.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <returns>New mqtt client provider.</returns>
    IMqttClientProvider Create(ILogger logger, string clientId, string host, int port);
}
