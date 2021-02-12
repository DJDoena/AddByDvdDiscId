namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System.Diagnostics;
    using DVDProfilerXML.Version400.Localities;

    [DebuggerDisplay("{Description}")]
    internal sealed class LocalityViewModel
    {
        public Locality Locality { get; }

        public int Id => Locality.ID;

        public string Description => Locality.Description;

        public LocalityViewModel(Locality locality)
        {
            Locality = locality;
        }
    }
}
