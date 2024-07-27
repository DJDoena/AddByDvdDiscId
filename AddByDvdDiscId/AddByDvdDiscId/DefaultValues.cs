using System;
using System.Runtime.InteropServices;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

[ComVisible(false)]
[Serializable]
public class DefaultValues
{
    public int SelectedLocality;

    public string SelectedDrive;

    public bool DownloadProfile = true;

    public bool CreateDiscIdContent = true;

    public bool AddAsChild = false;
}