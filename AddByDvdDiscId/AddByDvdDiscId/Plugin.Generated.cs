using System;
using System.Runtime.InteropServices;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

[Guid(ClassGuid.ClassID), ComVisible(true)]

public partial class Plugin : IDVDProfilerPluginInfo
{
    #region IDVDProfilerPluginInfo

    public string GetName()
        => Texts.PluginName;

    public string GetDescription() 
        => Texts.PluginDescription;

    public string GetAuthorName() 
        => "Doena Soft.";

    public string GetAuthorWebsite()
        => Texts.PluginUrl;

    public int GetPluginAPIVersion()
        => PluginConstants.API_VERSION;

    public int GetVersionMajor()
    {
        var version = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;

        return version.Major;
    }

    public int GetVersionMinor()
    {
        var version = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;

        var minor = version.Minor * 100 + version.Build * 10 + version.Revision;

        return minor;
    }

    #endregion

    #region Plugin Registering

    [DllImport("user32.dll")]
    public extern static int SetParent(int child, int parent);

    [ComImport(), Guid("0002E005-0000-0000-C000-000000000046")]
    internal class StdComponentCategoriesMgr { }

    [ComRegisterFunction()]
    public static void RegisterServer(Type _)
    {
        CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();

        Guid clsidThis = new Guid(ClassGuid.ClassID);

        Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

        cr.RegisterClassImplCategories(ref clsidThis, 1, new Guid[] { catid });
    }

    [ComUnregisterFunction()]
    public static void UnregisterServer(Type _)
    {
        CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();

        Guid clsidThis = new Guid(ClassGuid.ClassID);

        Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

        cr.UnRegisterClassImplCategories(ref clsidThis, 1, new Guid[] { catid });
    }

    #endregion
}