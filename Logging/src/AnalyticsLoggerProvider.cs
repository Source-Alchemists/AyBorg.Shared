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

[ProviderAlias("AyBorg.Log")]
public sealed class AnalyticsLoggerProvider : ILoggerProvider
{
    private readonly IConfiguration _configuration;
    private readonly IAnalyticsCache _cache;
    private AnalyticsLogger _logger = null!;
    private bool _isDisposed = false;

    public AnalyticsLoggerProvider(IConfiguration configuration,
                                    IAnalyticsCache analyticsCache)
    {
        _configuration = configuration;
        _cache = analyticsCache;
    }

    public ILogger CreateLogger(string categoryName)
    {
        _logger ??= new AnalyticsLogger(_configuration, _cache);
        return _logger;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (isDisposing && !_isDisposed)
        {
            _logger = null!;
            _isDisposed = true;
        }
    }
}
