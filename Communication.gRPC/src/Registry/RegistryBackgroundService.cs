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

using Ayborg.Gateway.V1;
using AyBorg.Types;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AyBorg.Communication.gRPC.Registry;

public sealed class RegistryBackgroundService : BackgroundService
{
    private readonly ILogger<RegistryBackgroundService> _logger;
    private readonly Register.RegisterClient _registerClient;
    private readonly IServiceConfiguration _serviceConfiguration;
    private readonly IConfiguration _configuration;
    private Guid _serviceId = Guid.Empty;

    public RegistryBackgroundService(ILogger<RegistryBackgroundService> logger,
                                        Register.RegisterClient registerClient,
                                        IConfiguration configuration,
                                        IServiceConfiguration serviceConfiguration)
    {
        _logger = logger;
        _registerClient = registerClient;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace(new EventId((int)EventLogType.Connect), "Registry service is starting.");
        try
        {
            await Register(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(new EventId((int)EventLogType.Connect), ex, "Failed to register at start");
        }

        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace(new EventId((int)EventLogType.Disconnect), "Registry service is stopping.");
        try
        {
            StatusResponse response = await _registerClient.UnregisterAsync(new UnregisterRequest
            {
                Id = _serviceId.ToString()
            }, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (!response.Success)
            {
                _logger.LogWarning(new EventId((int)EventLogType.Disconnect), "Failed to unregister service: {ErrorMessage}", response.ErrorMessage);
            }

            _serviceId = Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(new EventId((int)EventLogType.Disconnect), ex, "Failed to unregister");
        }

        await base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Factory.StartNew(async () =>
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_serviceId != Guid.Empty)
                {
                    StatusResponse response = await _registerClient.HeartbeatAsync(new HeartbeatRequest
                    {
                        Id = _serviceId.ToString()
                    }, cancellationToken: stoppingToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        _logger.LogWarning(new EventId((int)EventLogType.Connect), "Failed to send heartbeat: {ErrorMessage}", response.ErrorMessage);
                        _serviceId = Guid.Empty;
                    }
                }
                else
                {
                    await Register(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(new EventId((int)EventLogType.Connect), ex, "Failed to send heartbeat");
                _serviceId = Guid.Empty; // In the next iteration, the service should be registered again
            }

            await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);
        }
    }, TaskCreationOptions.LongRunning);

    private async ValueTask Register(CancellationToken cancellationToken)
    {
        StatusResponse response = await _registerClient.RegisterAsync(new RegisterRequest
        {
            Name = _serviceConfiguration.DisplayName,
            UniqueName = _serviceConfiguration.UniqueName,
            Type = _serviceConfiguration.TypeName,
            Url = DetermineServiceUrl(_configuration),
            Version = _serviceConfiguration.Version
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        if (!response.Success)
        {
            _logger.LogWarning(new EventId((int)EventLogType.Connect), "Failed to register service: {ErrorMessage}", response.ErrorMessage);
        }

        if (!Guid.TryParse(response.Id, out _serviceId))
        {
            _logger.LogWarning(new EventId((int)EventLogType.Connect), "Failed to parse service id: {Id}", response.Id);
        }
    }

    private static string DetermineServiceUrl(IConfiguration configuration)
    {
        string url = configuration.GetValue("Kestrel:Endpoints:gRPC:Url", string.Empty)!;

        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        url = configuration.GetValue("Kestrel:Endpoints:Https:Url", string.Empty)!;

        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        url = configuration.GetValue("Kestrel:Endpoints:Http:Url", string.Empty)!;

        if (string.IsNullOrEmpty(url))
        {
            throw new KeyNotFoundException("Service URL not set!");
        }

        return url;
    }
}
