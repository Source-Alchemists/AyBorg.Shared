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

namespace AyBorg.Runtime;

public enum EngineState
{
    /// <summary>
    /// The engine is idle.
    /// </summary>
    Idle = 0,
    /// <summary>
    /// The engine is starting.
    /// </summary>
    Starting = 1,
    /// <summary>
    /// The engine is running.
    /// </summary>
    Running = 2,
    /// <summary>
    /// The engine is stopping.
    /// </summary>
    Stopping = 3,
    /// <summary>
    /// The engine is stopped.
    /// </summary>
    Stopped = 4,
    /// <summary>
    /// The engine is aborting.
    Aborting = 5,
    /// <summary>
    /// The engine is aborted.
    /// </summary>
    Aborted = 6,
    /// <summary>
    /// The engined finished.
    /// </summary>
    Finished = 7
}
