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

using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace AyBorg.Communication.Hub;

/// <summary>
/// Base class for hub clients.
/// </summary>
public class BaseHubClient : IBaseHubClient
{
    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger<BaseHubClient> Logger;

    /// <summary>
    /// The SignalR connection.
    /// </summary>
    protected HubConnection Connection = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseHubClient"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected BaseHubClient(ILogger<BaseHubClient> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Tries to connect to the hub.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the connection was successful.</returns>
    public async ValueTask<bool> TryConnectAsync(CancellationToken cancellationToken = default)
    {
       while (true)
        {
            try
            {
                await Connection.StartAsync(cancellationToken);
                return true;
            }
            catch when (cancellationToken.IsCancellationRequested)
            {
                return false;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogWarning(ex, "Failed to connect to the hub. Retrying in 5 seconds...");
                Debug.Assert(Connection!.State == HubConnectionState.Disconnected);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
