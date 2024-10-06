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

namespace AyBorg.Logging;

public sealed record EventEntry
{
    /// <summary>
    /// Gets or sets the service unique name.
    /// </summary>
    public string ServiceUniqueName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the service type.
    /// </summary>
    public string ServiceType { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets or sets the log level.
    /// </summary>
    public int LogLevel { get; init; }

    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; init; } = string.Empty;
}
