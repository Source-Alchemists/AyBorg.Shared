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

namespace AyBorg.Communication;

public static class ServiceTypes
{
    public const string Gateway = nameof(Gateway);
    public const string Frontend = nameof(Frontend);
    public const string Dashboard = nameof(Dashboard);
    public const string Result = nameof(Result);
    public const string Agent = nameof(Agent);
    public const string Log = nameof(Log);
    public const string Audit = nameof(Audit);
    public const string Cognitive = nameof(Cognitive);
    public const string CognitiveAgent = "Cognitive.Agent";
}
