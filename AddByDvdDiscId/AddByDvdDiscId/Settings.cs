using System;
using System.Runtime.InteropServices;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

[ComVisible(false)]
[Serializable]
public class Settings
{
    public string CurrentVersion;

    public DefaultValues DefaultValues;
}