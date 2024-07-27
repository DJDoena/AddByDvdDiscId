namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System.Collections.Generic;
    using System.Linq;
    using AbstractionLayer.IOServices;
    using AbstractionLayer.UIServices;
    using DVDProfilerXML.Version400.Localities;
    using Invelos.DVDProfilerPlugin;

    internal sealed class ServiceProvider
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
            IOServices = new IOServices();

            UIServices = new FormUIServices();
        }
    }
}
