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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AyBorg.Logging;

public static class AnalyticsLoggerExtension
{
    public static ILoggingBuilder AddAyBorgAnalyticsLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AnalyticsLoggerProvider>());
        return builder;
    }

    public static WebApplicationBuilder AddAyBorgAnalyticsLogger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IAnalyticsCache, AnalyticsCache>();
        builder.Services.AddHostedService<AnalyticsBackgroundService>();
        builder.Logging.AddAyBorgAnalyticsLogger();
        return builder;
    }
}
