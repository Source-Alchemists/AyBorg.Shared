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

namespace AyBorg.Runtime;

public interface IStepProxy : IDisposable
{
    /// <summary>
    /// Called when the step is executed.
    /// </summary>
    event EventHandler<bool> Completed;

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    Guid Id { get; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets the categories.
    /// </summary>
    IReadOnlyCollection<string> Categories { get; }

    /// <summary>
    /// Gets or sets the meta information.
    /// </summary>
    /// <value>
    /// The meta information.
    /// </value>
    PluginMetaInfo MetaInfo { get; }

    /// <summary>
    /// Gets the ports.
    /// </summary>
    /// <value>
    /// The ports.
    /// </value>
    IEnumerable<IPort> Ports { get; }

    /// <summary>
    /// Gets the links.
    /// </summary>
    IList<PortLink> Links { get; }

    /// <summary>
    /// Gets the step body.
    /// </summary>
    /// <value>
    /// The step body.
    /// </value>
    IStepBody StepBody { get; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    int X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    int Y { get; set; }

    /// <summary>
    /// Gets the iteration identifier the step was execution last in.
    /// </summary>
    Guid IterationId { get; }

    /// <summary>
    /// Gets the execution time in milliseconds.
    /// </summary>
    long ExecutionTimeMs { get; }

    /// <summary>
    /// Executes the step.
    /// </summary>
    /// <param name="iterationId">The iteration identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<bool> TryRunAsync(Guid iterationId, CancellationToken cancellationToken);

    /// <summary>
    /// Initializes the step before running it.
    /// </summary>
    ValueTask<bool> TryBeforeStartAsync();

    /// <summary>
    /// Called after the step is created or loaded.
    /// </summary>
    ValueTask<bool> TryAfterInitializedAsync();
}
