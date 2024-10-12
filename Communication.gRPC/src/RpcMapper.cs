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

using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Ayborg.Gateway.Agent.V1;
using AyBorg.Types.Models;
using AyBorg.Types.Ports;

namespace AyBorg.Communication.gRPC;

public class RpcMapper : IRpcMapper
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

     public PluginMetaInfo FromRpc(PluginMetaDto rpc)
    {
        return new PluginMetaInfo
        {
            Id = Guid.Parse(rpc.Id),
            AssemblyName = rpc.AssemblyName,
            AssemblyVersion = rpc.AssemblyVersion,
            TypeName = rpc.TypeName
        };
    }

    public StepModel FromRpc(StepDto rpc)
    {
        var convertedPorts = new List<PortModel>();
        foreach (PortDto? port in rpc.Ports)
        {
            convertedPorts.Add(FromRpc(port));
        }

        var step = new StepModel
        {
            Id = Guid.Parse(rpc.Id),
            Name = rpc.Name,
            Categories = rpc.Categories,
            X = rpc.X,
            Y = rpc.Y,
            ExecutionTimeMs = rpc.ExecutionTimeMs,
            MetaInfo = FromRpc(rpc.MetaInfo),
            Ports = convertedPorts
        };

        return step;
    }

    public PortModel FromRpc(PortDto rpc)
    {
        return new PortModel
        {
            Id = Guid.Parse(rpc.Id),
            Name = rpc.Name,
            Direction = (PortDirection)rpc.Direction,
            Brand = (PortBrand)rpc.Brand,
            IsConnected = rpc.IsConnected,
            IsLinkConvertable = rpc.IsLinkConvertable,
            Value = UnpackPortValue(rpc)
        };
    }

    public LinkModel FromRpc(LinkDto rpc)
    {
        return new LinkModel
        {
            Id = Guid.Parse(rpc.Id),
            SourceId = Guid.Parse(rpc.SourceId),
            TargetId = Guid.Parse(rpc.TargetId)
        };
    }

    public PluginMetaDto ToRpc(PluginMetaInfo pluginMetaInfo)
    {
        return new PluginMetaDto
        {
            Id = pluginMetaInfo.Id.ToString(),
            AssemblyName = pluginMetaInfo.AssemblyName,
            AssemblyVersion = pluginMetaInfo.AssemblyVersion,
            TypeName = pluginMetaInfo.TypeName
        };
    }

    public StepDto ToRpc(StepModel step)
    {
        var convertedPorts = new List<PortDto>();
        foreach (PortModel port in step.Ports!)
        {
            convertedPorts.Add(ToRpc(port));
        }
        var rpc = new StepDto
        {
            Id = step.Id.ToString(),
            Name = step.Name,
            X = step.X,
            Y = step.Y,
            ExecutionTimeMs = step.ExecutionTimeMs,
            MetaInfo = ToRpc(step.MetaInfo),
        };

        rpc.Categories.AddRange(step.Categories);
        rpc.Ports.Add(convertedPorts);

        return rpc;
    }

    public PortDto ToRpc(PortModel port)
    {
        return new PortDto
        {
            Id = port.Id.ToString(),
            Name = port.Name,
            Direction = (int)port.Direction,
            Brand = (int)port.Brand,
            IsConnected = port.IsConnected,
            IsLinkConvertable = port.IsLinkConvertable,
            Value = PackPortValue(port)
        };
    }

    public LinkDto ToRpc(LinkModel link)
    {
        return new LinkDto
        {
            Id = link.Id.ToString(),
            SourceId = link.SourceId.ToString(),
            TargetId = link.TargetId.ToString()
        };
    }

    public LinkDto ToRpc(PortLink link)
    {
        return new LinkDto
        {
            Id = link.Id.ToString(),
            SourceId = link.SourceId.ToString(),
            TargetId = link.TargetId.ToString()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object UnpackPortValue(PortDto rpc)
    {
        return (PortBrand)rpc.Brand switch
        {
            PortBrand.String or PortBrand.Folder => rpc.Value,
            PortBrand.Boolean => bool.Parse(rpc.Value),
            PortBrand.Numeric => double.Parse(rpc.Value.Replace(',', '.'), CultureInfo.InvariantCulture),
            PortBrand.Enum => JsonSerializer.Deserialize<EnumModel>(rpc.Value, s_jsonSerializerOptions)!,
            PortBrand.Select => JsonSerializer.Deserialize<SelectPort.ValueContainer>(rpc.Value, s_jsonSerializerOptions)!,
            PortBrand.Rectangle => JsonSerializer.Deserialize<RectangleModel>(rpc.Value, s_jsonSerializerOptions)!,
            PortBrand.Image => JsonSerializer.Deserialize<CacheImage>(rpc.Value, s_jsonSerializerOptions)!,
            // Collections
            PortBrand.StringCollection => JsonSerializer.Deserialize<string[]>(rpc.Value, s_jsonSerializerOptions)?.ToImmutableList() ?? ImmutableList<string>.Empty,
            PortBrand.NumericCollection => JsonSerializer.Deserialize<double[]>(rpc.Value, s_jsonSerializerOptions)?.ToImmutableList() ?? ImmutableList<double>.Empty,
            PortBrand.RectangleCollection => JsonSerializer.Deserialize<RectangleModel[]>(rpc.Value, s_jsonSerializerOptions)?.ToImmutableList() ?? ImmutableList<RectangleModel>.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(rpc.Brand), rpc.Brand, null),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string PackPortValue(PortModel port)
    {
        return port.Brand switch
        {
            PortBrand.String or PortBrand.Folder => port.Value!.ToString()!,
            PortBrand.Boolean => port.Value!.ToString()!,
            PortBrand.Numeric => Convert.ToString(port.Value, CultureInfo.InvariantCulture)!,
            PortBrand.Enum => ConvertEnum(port.Value!),
            PortBrand.Select => JsonSerializer.Serialize(port.Value, s_jsonSerializerOptions),
            PortBrand.Rectangle => JsonSerializer.Serialize(port.Value, s_jsonSerializerOptions),
            PortBrand.Image => ConvertImage(port.Value!),
            // Collections
            PortBrand.StringCollection => ConvertCollection<string>(port.Value!),
            PortBrand.NumericCollection => ConvertCollection<double>(port.Value!),
            PortBrand.RectangleCollection => ConvertCollection<RectangleModel>(port.Value!),
            _ => throw new ArgumentOutOfRangeException(nameof(port.Brand), port.Brand, null),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string ConvertCollection<T>(object obj)
    {
        string result;
        if (obj is IEnumerable<T> collection)
        {
            result = JsonSerializer.Serialize(collection, s_jsonSerializerOptions);
        }
        else
        {
            result = obj.ToString()!;
        }

        // The JsonSerializer is providing us with a invalid json, we have to fix it.
        if (result.Equals("[\"\"]"))
        {
            result = result.Replace("\"\"", string.Empty);
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string ConvertEnum(object inEnum)
    {
        EnumModel resEnum;
        if (inEnum is Enum enumObject)
        {
            resEnum = new EnumModel
            {
                Name = enumObject.ToString(),
                Names = Enum.GetNames(enumObject.GetType())
            };
        }
        else
        {
            resEnum = (EnumModel)inEnum;
        }

        return JsonSerializer.Serialize(resEnum, s_jsonSerializerOptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string ConvertImage(object image)
    {
        ImageMeta imageMeta = ((CacheImage)image).Meta;

        return JsonSerializer.Serialize(imageMeta, s_jsonSerializerOptions);
    }
}
