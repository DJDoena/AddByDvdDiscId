using System.Collections.Generic;
using DoenaSoft.AbstractionLayer.IOServices;
using DoenaSoft.AbstractionLayer.UIServices;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

internal interface IServiceProvider
{
    IDVDProfilerAPI Api { get; set; }

    DefaultValues DefaultValues { get; set; }

    IIOServices IOServices { get; }

    IEnumerable<Locality> Localities { get; set; }

    IUIServices UIServices { get; }
}