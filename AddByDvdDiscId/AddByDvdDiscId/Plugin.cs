namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System;
    using System.Runtime.InteropServices;
    using AbstractionLayer.IOServices;
    using AbstractionLayer.UIServices;
    using DVDProfilerHelper;
    using DVDProfilerXML.Version400.Localities;
    using Invelos.DVDProfilerPlugin;

    public partial class Plugin : IDVDProfilerPlugin
    {
        private const int AddMenuId = 1;

        private readonly ServiceProvider _serviceProvider;

        private readonly string _errorFile;

        private readonly string _applicationPath;

        private readonly Version _pluginVersion;

        private string _addMenuToken = "";

        private readonly string _settingsFile;

        private Settings _settings;

        private IIOServices IOServices => _serviceProvider.IOServices;

        private IUIServices UIServices => _serviceProvider.UIServices;

        private IDVDProfilerAPI Api => _serviceProvider.Api;

        public Plugin()
        {
            //System.Diagnostics.Debugger.Launch();

            _serviceProvider = new ServiceProvider();

            _applicationPath = IOServices.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Doena Soft", "AddByDvdDiscId");

            _settingsFile = IOServices.Path.Combine(_applicationPath, "AddByDvdDiscId.xml");

            _errorFile = IOServices.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "AddByDvdDiscId.xml");

            _pluginVersion = System.Reflection.Assembly.GetAssembly(GetType()).GetName().Version;

            var localities = Localities.Deserialize();

            _serviceProvider.Localities = localities?.Locality;
        }

        #region I... Members

        #region IDVDProfilerPlugin

        public void Load(IDVDProfilerAPI api)
        {
            _serviceProvider.Api = api;

            if (IOServices.Folder.Exists(_applicationPath) == false)
            {
                IOServices.Folder.CreateFolder(_applicationPath);
            }

            if (IOServices.File.Exists(_settingsFile))
            {
                try
                {
                    _settings = DVDProfilerSerializer<Settings>.Deserialize(_settingsFile);
                }
                catch (Exception ex)
                {
                    UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeRead, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
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

            _serviceProvider.DefaultValues = _settings.DefaultValues;

            Api.RegisterForEvent(PluginConstants.EVENTID_FormCreated);

            _addMenuToken = Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"DVD", "Add by DVD Disc ID", AddMenuId);
        }

        public void Unload()
        {
            Api.UnregisterMenuItem(_addMenuToken);

            try
            {
                DVDProfilerSerializer<Settings>.Serialize(_settingsFile, _settings);
            }
            catch (Exception ex)
            {
                UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeWritten, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
            }

            _serviceProvider.Api = null;
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
                    UIServices.ShowMessageBox(string.Format(MessageBoxTexts.CriticalError, ex.Message, _errorFile), MessageBoxTexts.CriticalErrorHeader, Buttons.OK, Icon.Error);

                    if (IOServices.File.Exists(_errorFile))
                    {
                        IOServices.File.Delete(_errorFile);
                    }

                    LogException(ex);
                }
                catch (Exception inEx)
                {
                    UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeWritten, _errorFile, inEx.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
                }
            }
        }

        #endregion

        #endregion

        private void HandleMenuClick(int menuEventID)
        {
            switch (menuEventID)
            {
                case AddMenuId:
                    {
                        using (var form = new MainForm(_serviceProvider))
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
                var lastApiError = Api.GetLastError();

                returnEx = new EnhancedCOMException(lastApiError, comEx);
            }

            return returnEx;
        }

      
    }
}