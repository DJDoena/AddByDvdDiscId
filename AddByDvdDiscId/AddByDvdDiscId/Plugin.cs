using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    [ComVisible(true)]
    [Guid(ClassGuid.ClassID)]
    public class Plugin : IDVDProfilerPlugin, IDVDProfilerPluginInfo
    {
        private readonly string _errorFile;

        private readonly string _applicationPath;

        private readonly Version _pluginVersion;

        private IDVDProfilerAPI _api;

        private const int AddMenuId = 1;

        private string _addMenuToken = "";

        private readonly Localities _localities;

        private readonly string _settingsFile;

        private Settings _settings;

        public Plugin()
        {
            _applicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Doena Soft", "AddByDvdDiscId");

            _settingsFile = Path.Combine(_applicationPath, "AddByDvdDiscId.xml");

            _errorFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "AddByDvdDiscId.xml");

            _pluginVersion = System.Reflection.Assembly.GetAssembly(GetType()).GetName().Version;

            _localities = Localities.Deserialize();
        }

        #region I... Members

        #region IDVDProfilerPlugin

        public void Load(IDVDProfilerAPI api)
        {
            _api = api;

            if (Directory.Exists(_applicationPath) == false)
            {
                Directory.CreateDirectory(_applicationPath);
            }

            if (File.Exists(_settingsFile))
            {
                try
                {
                    _settings = DVDProfilerSerializer<Settings>.Deserialize(_settingsFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(MessageBoxTexts.FileCantBeRead, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (_settings == null)
            {
                _settings = new Settings();
            }

            if (_settings.DefaultValues == null)
            {
                _settings.DefaultValues = new DefaultValues();
            }

            _api.RegisterForEvent(PluginConstants.EVENTID_FormCreated);

            _addMenuToken = _api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"DVD", "Add by DVD Disc ID", AddMenuId);
        }

        public void Unload()
        {
            _api.UnregisterMenuItem(_addMenuToken);

            try
            {
                DVDProfilerSerializer<Settings>.Serialize(_settingsFile, _settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(MessageBoxTexts.FileCantBeWritten, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _api = null;
        }

        public void HandleEvent(int EventType, object EventData)
        {
            try
            {
                if (EventType == PluginConstants.EVENTID_CustomMenuClick)
                {
                    HandleMenuClick((int)EventData);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(string.Format(MessageBoxTexts.CriticalError, ex.Message, _errorFile), MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    if (File.Exists(_errorFile))
                    {
                        File.Delete(_errorFile);
                    }

                    LogException(ex);
                }
                catch (Exception inEx)
                {
                    MessageBox.Show(string.Format(MessageBoxTexts.FileCantBeWritten, _errorFile, inEx.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region IDVDProfilerPluginInfo

        public string GetName() => Texts.PluginName;

        public string GetDescription() => Texts.PluginDescription;

        public string GetAuthorName() => "Doena Soft.";

        public string GetAuthorWebsite() => Texts.PluginUrl;

        public int GetPluginAPIVersion() => PluginConstants.API_VERSION;

        public int GetVersionMajor() => _pluginVersion.Major;

        public int GetVersionMinor() => _pluginVersion.Minor;

        #endregion

        #endregion

        private void HandleMenuClick(int MenuEventID)
        {
            switch (MenuEventID)
            {
                case AddMenuId:
                    {
                        using (var form = new MainForm(_api, _settings.DefaultValues, _localities?.Locality))
                        {
                            form.ShowDialog();
                        }

                        break;
                    }
            }
        }

        private void LogException(Exception ex)
        {
            ex = WrapCOMException(ex);

            var exceptionXml = new ExceptionXml(ex);

            DVDProfilerSerializer<ExceptionXml>.Serialize(_errorFile, exceptionXml);
        }

        private Exception WrapCOMException(Exception ex)
        {
            var returnEx = ex;

            if (ex is COMException comEx)
            {
                var lastApiError = _api.GetLastError();

                returnEx = new EnhancedCOMException(lastApiError, comEx);
            }

            return returnEx;
        }

        #region Plugin Registering

        [DllImport("user32.dll")]
        public extern static int SetParent(int child, int parent);

        [ComImport(), Guid("0002E005-0000-0000-C000-000000000046")]
        internal class StdComponentCategoriesMgr { }

        [ComRegisterFunction()]
        public static void RegisterServer(Type _)
        {
            var cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();

            var clsidThis = new Guid(ClassGuid.ClassID);

            var catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.RegisterClassImplCategories(ref clsidThis, 1, new Guid[] { catid });
        }

        [ComUnregisterFunction()]
        public static void UnregisterServer(Type _)
        {
            var cr = (CategoryRegistrar.ICatRegister)new StdComponentCategoriesMgr();

            var clsidThis = new Guid(ClassGuid.ClassID);

            var catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.UnRegisterClassImplCategories(ref clsidThis, 1, new Guid[] { catid });
        }

        #endregion
    }
}