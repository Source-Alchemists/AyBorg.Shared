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

using Ayborg.Gateway.Analytics.V1;
using AyBorg.Communication;
using Moq;

namespace AyBorg.Logging.Tests;

public class AnalyticsBackgroundServiceTests
{
    private readonly Mock<IServiceConfiguration> _mockConfiguration = new();
    private readonly Mock<IAnalyticsCache> _mockCache = new();
    private readonly Mock<EventLog.EventLogClient> _mockClient = new();
    private readonly AnalyticsBackgroundService _service;

    public AnalyticsBackgroundServiceTests()
    {
        _service = new AnalyticsBackgroundService(_mockConfiguration.Object, _mockCache.Object, _mockClient.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Test_BackgroundDequeue(bool canDequeue)
    {
        // Arrange
        CancellationTokenSource tokenSource = new();
        Ayborg.Gateway.Analytics.V1.EventEntry entry;
        _mockCache.Setup(m => m.TryDequeue(out entry)).Returns(canDequeue);

        // Act
        await _service.StartAsync(tokenSource.Token);
        await Task.Delay(100);
        await _service.StopAsync(tokenSource.Token);
        tokenSource.Cancel();

        // Assert
        _mockCache.Verify(m => m.TryDequeue(out entry), Times.AtLeastOnce);
    }
}
