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

using AyBorg.Communication;
using Microsoft.Extensions.Hosting;

namespace AyBorg.Logging;

public sealed class AnalyticsBackgroundService : BackgroundService
{
    private readonly IServiceConfiguration _serviceConfiguration;
    private readonly IAnalyticsCache _cache;
    private readonly IEventLogClient _eventLogClient;


    /// <summary>
    /// Initializes a new instance of the <see cref="AnalyticsBackgroundService"/> class.
    /// </summary>
    /// <param name="serviceConfiguration">Service configuration</param>
    /// <param name="analyticsCache">Analytics cache</param>
    /// <param name="eventLogClient">Event log client</param>
    public AnalyticsBackgroundService(IServiceConfiguration serviceConfiguration,
                                        IAnalyticsCache analyticsCache,
                                        IEventLogClient eventLogClient)
    {
        _serviceConfiguration = serviceConfiguration;
        _cache = analyticsCache;
        _eventLogClient = eventLogClient;
    }

    /// <summary>
    /// Start the background service
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task StartAsync(CancellationToken cancellationToken) => base.StartAsync(cancellationToken);

    /// <summary>
    /// Stop the background service
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task StopAsync(CancellationToken cancellationToken) => base.StopAsync(cancellationToken);

    /// <summary>
    /// Execute the background service
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Factory.StartNew(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_cache.TryDequeue(out EventEntry? request))
                    {
                        request = request with { ServiceType = _serviceConfiguration.TypeName, ServiceUniqueName = _serviceConfiguration.UniqueName };
                        await _eventLogClient.LogEventAsync(request).ConfigureAwait(false);
                    }
                    else
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                    }
                }
                catch (Exception)
                {
                    await Task.Delay(100).ConfigureAwait(false);
                }
            }
        });
    }
}
