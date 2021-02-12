namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System.Diagnostics;
    using AbstractionLayer.IOServices;

    [DebuggerDisplay("{Description}")]
    internal sealed class DriveViewModel
    {
        public IDriveInfo Drive { get; }

        public string Id => Drive.DriveLetter;

        public bool IsReady => Drive.IsReady;

        public string Description => Drive.DriveLabel;

        public DriveViewModel(IDriveInfo drive)
        {
            Drive = drive;
        }
    }
}