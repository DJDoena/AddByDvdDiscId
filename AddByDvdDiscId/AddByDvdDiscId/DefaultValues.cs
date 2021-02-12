namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(false)]
    [Serializable]
    public class DefaultValues
    {
        public int SelectedLocality;

        public string SelectedDrive;

        public bool DownloadProfile = true;

        public bool CreateDiscIdContent = true;
    }
}