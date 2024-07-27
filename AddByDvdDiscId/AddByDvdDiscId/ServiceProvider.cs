using System.Collections.Generic;
using System.Linq;
using DoenaSoft.AbstractionLayer.IOServices;
using DoenaSoft.AbstractionLayer.UIServices;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

internal sealed class ServiceProvider : IServiceProvider
{
    private IEnumerable<Locality> _localities;

    public IIOServices IOServices { get; }

    public IUIServices UIServices { get; }

    public IDVDProfilerAPI Api { get; set; }

    public DefaultValues DefaultValues { get; set; }

    public IEnumerable<Locality> Localities
    {
        get => _localities ?? Enumerable.Empty<Locality>();
        set => _localities = value;
    }

    public ServiceProvider()
    {
        this.IOServices = new IOServices();

        this.UIServices = new FormUIServices();
    }
}