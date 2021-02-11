using System.Diagnostics;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    [DebuggerDisplay("{Description}")]
    internal sealed class LocalityViewModel
    {
        private readonly Locality _locality;

        public int Id => _locality.ID;

        public string Description => _locality.Description;

        public LocalityViewModel(Locality locality)
        {
            _locality = locality;
        }
    }
}
