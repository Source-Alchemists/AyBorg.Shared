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

using AyBorg.Types.Models;

namespace AyBorg.Runtime.Devices;

public interface IDeviceProviderProxy : IDisposable {

    /// <summary>
    /// Gets the device provider name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the device provider prefix.
    /// </summary>
    string Prefix { get; }

    /// <summary>
    /// Gets a value indicating whether can add.
    /// </summary>
    bool CanAdd { get; }

    /// <summary>
    /// Gets the plugin meta information.
    /// </summary>
    PluginMetaInfo MetaInfo { get; }

    /// <summary>
    /// Gets the devices.
    /// </summary>
    IReadOnlyCollection<IDeviceProxy> Devices { get; }

    /// <summary>
    /// Try to initialize.
    /// </summary>
    /// <returns>True if initialized.</returns>
    ValueTask<bool> TryInitializeAsync();

    /// <summary>
    /// Try to add.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>The device.</returns>
    ValueTask<IDeviceProxy> AddAsync(AddDeviceOptions options);

    /// <summary>
    /// Try to remove.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>The device.</returns>
    ValueTask<IDeviceProxy> RemoveAsync(string id);
}
