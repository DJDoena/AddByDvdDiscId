using System.Diagnostics;
using System.IO;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    [DebuggerDisplay("{Description}")]
    internal sealed class DriveViewModel
    {
        private readonly DriveInfo _drive;

        public string Id => _drive.Name;

        public bool IsReady => _drive.IsReady;

        public string Description
        {
            get
            {
                try
                {
                    if (IsReady)
                    {
                        return $"{Id} [ {_drive.VolumeLabel} ]";
                    }
                    else
                    {
                        return Id;
                    }
                }
                catch
                {
                    return Id;
                }
            }
        }

        public DriveViewModel(DriveInfo drive)
        {
            _drive = drive;
        }
    }
}