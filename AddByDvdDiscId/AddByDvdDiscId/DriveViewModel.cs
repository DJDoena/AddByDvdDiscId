using System.Diagnostics;
using DoenaSoft.AbstractionLayer.IOServices;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

[DebuggerDisplay("{Description}")]
internal sealed class DriveViewModel
{
    public IDriveInfo Drive { get; }

    public string Id
        => this.Drive.DriveLetter;

    public bool IsReady
        => this.Drive.IsReady;

    public string Description
        => this.Drive.DriveLabel;

    public DriveViewModel(IDriveInfo drive)
    {
        this.Drive = drive;
    }
}