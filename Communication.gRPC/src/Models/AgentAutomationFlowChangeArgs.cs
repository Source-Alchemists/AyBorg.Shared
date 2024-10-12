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

namespace AyBorg.Communication.gRPC.Models;

public record AgentAutomationFlowChangeArgs
{
    public string[] AddedSteps { get; set; } = Array.Empty<string>();
    public string[] RemovedSteps { get; set; } = Array.Empty<string>();
    public string[] AddedLinks { get; set; } = Array.Empty<string>();
    public string[] RemovedLinks { get; set; } = Array.Empty<string>();
    public string[] ChangedSteps { get; set; } = Array.Empty<string>();
    public string[] ChangedPorts { get; set; } = Array.Empty<string>();
}
