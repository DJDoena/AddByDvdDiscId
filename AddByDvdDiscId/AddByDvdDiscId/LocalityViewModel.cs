using System.Diagnostics;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

[DebuggerDisplay("{Description}")]
internal sealed class LocalityViewModel
{
    public Locality Locality { get; }

    public int Id
        => this.Locality.ID;

    public string Description
        => this.Locality.Description;

    public LocalityViewModel(Locality locality)
    {
        this.Locality = locality;
    }
}