
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

using AyBorg.Types;
using AyBorg.Types.Models;
using AyBorg.Types.Ports;

namespace AyBorg.Runtime.Devices;

public interface IDeviceProxy : IDisposable
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the manufacturer.
    /// </summary>
    string Manufacturer { get; }

    /// <summary>
    /// Gets a value indicating whether is active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Gets a value indicating whether is connected.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Gets the categories.
    /// </summary>
    IReadOnlyCollection<string> Categories { get; }

    /// <summary>
    /// Gets the ports.
    /// </summary>
    IEnumerable<IPort> Ports { get; }

    /// <summary>
    /// Gets or sets the meta information.
    /// </summary>
    PluginMetaInfo MetaInfo { get; }

    /// <summary>
    /// Gets or sets the provider meta information.
    /// </summary>
    PluginMetaInfo ProviderMetaInfo { get; }

    /// <summary>
    /// Gets the native device.
    /// </summary>
    IDevice Native { get; }

    /// <summary>
    /// Try to connect to the device.
    /// </summary>
    /// <returns>True if successful.</returns>
    ValueTask<bool> TryConnectAsync();

    /// <summary>
    /// Try to disconnect from the device.
    /// </summary>
    /// <returns>True if successful.</returns>
    ValueTask<bool> TryDisconnectAsync();
}
