using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DoenaSoft.AbstractionLayer.IOServices;
using DoenaSoft.AbstractionLayer.UIServices;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using DoenaSoft.ToolBox.Generics;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId;

public partial class Plugin : IDVDProfilerPlugin
{
    private const int AddMenuId = 1;

    private readonly IServiceProvider _serviceProvider;

    private readonly string _errorFile;

    private readonly string _applicationPath;

    private readonly Version _pluginVersion;

    private string _addMenuToken = "";

    private readonly string _settingsFile;

    private Settings _settings;

    private IIOServices IOServices 
        => _serviceProvider.IOServices;

    private IUIServices UIServices 
        => _serviceProvider.UIServices;

    private IDVDProfilerAPI Api 
        => _serviceProvider.Api;

    public Plugin()
    {
        //System.Diagnostics.Debugger.Launch();

        try
        {
            _serviceProvider = new ServiceProvider();

            _applicationPath = this.IOServices.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Doena Soft", "AddByDvdDiscId");

            _settingsFile = this.IOServices.Path.Combine(_applicationPath, "AddByDvdDiscId.xml");

            _errorFile = this.IOServices.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "AddByDvdDiscId.xml");

            _pluginVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;

            var localities = Localities.Deserialize();

            _serviceProvider.Localities = localities?.Locality;
        }
        catch (Exception ex)
        {
            this.ShowPluginLoadError(ex, "initialization");
        }
    }

    #region I... Members

    #region IDVDProfilerPlugin

    public void Load(IDVDProfilerAPI api)
    {
        try
        {
            _serviceProvider.Api = api;

            if (this.IOServices.Folder.Exists(_applicationPath) == false)
            {
                this.IOServices.Folder.CreateFolder(_applicationPath);
            }

            if (this.IOServices.File.Exists(_settingsFile))
            {
                try
                {
                    _settings = Serializer<Settings>.Deserialize(_settingsFile);
                }
                catch (Exception ex)
                {
                    this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeRead, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
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

            this.Api.RegisterForEvent(PluginConstants.EVENTID_FormCreated);

            _addMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"DVD", "Add by DVD Disc ID", AddMenuId);
        }
        catch (Exception ex)
        {
            this.ShowPluginLoadError(ex, "loading");
        }
    }

    public void Unload()
    {
        this.Api.UnregisterMenuItem(_addMenuToken);

        try
        {
            Serializer<Settings>.Serialize(_settingsFile, _settings);
        }
        catch (Exception ex)
        {
            this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeWritten, _settingsFile, ex.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
        }

        _serviceProvider.Api = null;
    }

    public void HandleEvent(int EventType, object EventData)
    {
        try
        {
            if (EventType == PluginConstants.EVENTID_CustomMenuClick)
            {
                this.HandleMenuClick((int)EventData);
            }
        }
        catch (Exception ex)
        {
            try
            {
                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.CriticalError, ex.Message, _errorFile), MessageBoxTexts.CriticalErrorHeader, Buttons.OK, Icon.Error);

                if (this.IOServices.File.Exists(_errorFile))
                {
                    this.IOServices.File.Delete(_errorFile);
                }

                this.LogException(ex);
            }
            catch (Exception inEx)
            {
                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.FileCantBeWritten, _errorFile, inEx.Message), MessageBoxTexts.ErrorHeader, Buttons.OK, Icon.Error);
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
        ex = this.WrapCOMException(ex);

        var exceptionXml = new ExceptionXml(ex);

        Serializer<ExceptionXml>.Serialize(_errorFile, exceptionXml);
    }

    private Exception WrapCOMException(Exception ex)
    {
        var returnEx = ex;

        if (ex is COMException comEx)
        {
            var lastApiError = this.Api.GetLastError();

            returnEx = new EnhancedCOMException(lastApiError, comEx);
        }

        return returnEx;
    }

    private void ShowPluginLoadError(Exception ex, string stage)
    {
        ex = this.WrapCOMException(ex);

        MessageBox.Show($"{Texts.PluginName}: Error during {stage}: {ex.Message}");

        try
        {
            this.LogException(ex);
        }
        catch
        { }
    }
}