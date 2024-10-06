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

namespace AyBorg.Logging;

public sealed class AnalyticsLogger : ILogger
{
    private readonly IConfiguration _configuration;
    private readonly IAnalyticsCache _cache;

    public AnalyticsLogger(IConfiguration configuration,
                            IAnalyticsCache analyticsCache)
    {
        _configuration = configuration;
        _cache = analyticsCache;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        LogLevel configLogLevel = _configuration.GetValue("Logging:LogLevel:AyBorg.Log", LogLevel.Information);
        return logLevel >= configLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        _cache.Enqueue(new EventEntry
        {
            ServiceUniqueName = string.Empty,
            Timestamp = DateTime.UtcNow,
            LogLevel = (int)logLevel,
            EventId = eventId.Id,
            Message = $"{formatter(state, exception)}"
        });
    }
}
