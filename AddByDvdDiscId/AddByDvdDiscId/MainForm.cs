using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DoenaSoft.AbstractionLayer.IOServices;
using DoenaSoft.CalculateDvdDiscId;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;
using UI = DoenaSoft.AbstractionLayer.UIServices;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{

    internal partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        private UI.IUIServices UIServices => _serviceProvider.UIServices;

        private IIOServices IOServices => _serviceProvider.IOServices;

        private IDVDProfilerAPI Api => _serviceProvider.Api;

        private IEnumerable<Locality> Localities => _serviceProvider.Localities;

        private DefaultValues DefaultValues => _serviceProvider.DefaultValues;

        private readonly IDVDInfo _parentProfile;

        private List<IDVDInfo> _allProfiles;

        public MainForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            this.InitializeComponent();

            this.Icon = Properties.Resources.DJDSOFT;

            UpdateFromOnlineDatabaseCheckBox.Checked = this.DefaultValues.DownloadProfile;

            var failure = false;

            failure |= this.InitializeDriveComboBox();
            failure |= this.InitializeLocalityComboBox();

            failure |= this.CheckDecrypterRunning();

            if (failure)
            {
                Load += (s, e) => this.Close();
            }
            else
            {
                _parentProfile = this.InitializeParentProfile();
            }

            if (_parentProfile != null)
            {
                this.SetDefaultLocality();
            }
        }

        private bool InitializeDriveComboBox()
        {
            IEnumerable<IDriveInfo> drives;
            try
            {
                drives = this.IOServices.GetDriveInfos(DriveType.CDRom);
            }
            catch
            {
                drives = Enumerable.Empty<IDriveInfo>();
            }

            var driveViewModels = drives.OrderBy(d => d.DriveLetter).Select(d => new DriveViewModel(d)).ToList();

            DriveComboBox.DataSource = driveViewModels;
            DriveComboBox.ValueMember = nameof(DriveViewModel.Id);
            DriveComboBox.DisplayMember = nameof(DriveViewModel.Description);

            var selectedDrive = driveViewModels.FirstOrDefault(d => d.Id == this.DefaultValues.SelectedDrive);

            if (selectedDrive != null)
            {
                DriveComboBox.SelectedValue = selectedDrive.Id;
            }
            else if (driveViewModels.Any())
            {
                var firstReady = driveViewModels.FirstOrDefault(d => d.IsReady);

                if (firstReady != null)
                {
                    DriveComboBox.SelectedValue = firstReady.Id;
                }
                else
                {
                    DriveComboBox.SelectedValue = driveViewModels.First().Id;
                }
            }

            if (!driveViewModels.Any())
            {
                this.UIServices.ShowMessageBox(MessageBoxTexts.NoDriveFound, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InitializeLocalityComboBox()
        {
            var localityViewModels = this.Localities.OrderBy(l => l.Description).Select(l => new LocalityViewModel(l)).ToList();

            LocalityComboBox.DataSource = localityViewModels;
            LocalityComboBox.ValueMember = nameof(LocalityViewModel.Id);
            LocalityComboBox.DisplayMember = nameof(LocalityViewModel.Description);

            var selectedLocality = localityViewModels.FirstOrDefault(l => l.Id == this.DefaultValues.SelectedLocality);

            if (selectedLocality != null)
            {
                LocalityComboBox.SelectedValue = selectedLocality.Id;
            }
            else if (localityViewModels.Any())
            {
                LocalityComboBox.SelectedValue = 0; //United States
            }

            if (!localityViewModels.Any())
            {
                this.UIServices.ShowMessageBox(MessageBoxTexts.NoProfilerLocalityFound, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckDecrypterRunning()
        {
            var anyDVDs = Process.GetProcessesByName("AnyDVD");

            var passKeys = Process.GetProcessesByName("DVDFabPasskey");

            if (anyDVDs?.Any() == true || passKeys?.Any() == true)
            {
                this.UIServices.ShowMessageBox(MessageBoxTexts.DecrypterRunning, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private IDVDInfo InitializeParentProfile()
        {
            var displayedProfile = this.Api.GetDisplayedDVD();

            IDVDInfo parentProfile = null;

            if (displayedProfile != null)
            {
                this.Api.DVDByProfileID(out parentProfile, displayedProfile.GetProfileID(), PluginConstants.DATASEC_AllSections, -1);

                AddAsChildCheckBox.Enabled = true;
                AddAsChildCheckBox.Checked = this.DefaultValues.AddAsChild;
            }
            else
            {
                AddAsChildCheckBox.Enabled = false;
                AddAsChildCheckBox.Checked = false;
            }

            return parentProfile;
        }

        private void SetDefaultLocality()
        {
            var parentProfileId = _parentProfile.GetProfileID();

            this.Api.DecodeProfileID(parentProfileId, out _, out var localityId, out _);

            LocalityComboBox.SelectedValue = localityId;
        }

        private void OnFormFormClosed(object sender, FormClosedEventArgs e)
        {
            this.TrySaveDrive();

            this.TrySaveLocality();

            this.DefaultValues.DownloadProfile = UpdateFromOnlineDatabaseCheckBox.Checked;

            if (AddAsChildCheckBox.Enabled)
            {
                this.DefaultValues.AddAsChild = AddAsChildCheckBox.Checked;
            }
        }

        private void OnRefreshDrivesButtonClick(object sender, EventArgs e)
        {
            this.TrySaveDrive();

            DriveComboBox.DataSource = null;
            DriveComboBox.Items.Clear();

            this.InitializeDriveComboBox();
        }

        private void TrySaveDrive()
        {
            if (DriveComboBox.SelectedIndex >= 0)
            {
                this.DefaultValues.SelectedDrive = (string)DriveComboBox.SelectedValue;
            }
        }

        private void TrySaveLocality()
        {
            if (LocalityComboBox.SelectedIndex >= 0)
            {
                this.DefaultValues.SelectedLocality = (int)LocalityComboBox.SelectedValue;
            }
        }

        private void OnAddDiscButtonClick(object sender, EventArgs e)
        {
            try
            {
                var drive = (DriveViewModel)DriveComboBox.SelectedItem;

                var locality = (LocalityViewModel)LocalityComboBox.SelectedItem;

                this.AddDisc(drive.Drive, locality.Locality);
            }
            catch (Exception ex)
            {
                this.UIServices.ShowMessageBox(ex.Message, MessageBoxTexts.CriticalErrorHeader, UI.Buttons.OK, UI.Icon.Error);
            }
        }

        private void AddDisc(IDriveInfo drive, Locality locality)
        {
            if (!drive.IsReady)
            {
                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.DriveNotReady, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var videoFolder = this.IOServices.GetFolderInfo(this.IOServices.Path.Combine(drive.RootFolderName, "VIDEO_TS"));

            if (!videoFolder.Exists)
            {
                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.NoDvdInDrive, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var previousCursor = this.Cursor;

            try
            {
                this.TryAddDisc(drive, locality, previousCursor);
            }
            finally
            {
                this.Cursor = previousCursor;
            }
        }

        private void TryAddDisc(IDriveInfo drive, Locality locality, Cursor previousCursor)
        {
            this.Cursor = Cursors.WaitCursor;

            Application.DoEvents();

            string discId;
            try
            {
                discId = DvdDiscIdCalculator.Calculate(drive.DriveLetter);
            }
            catch
            {
                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.DriveNotReady, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var suffix = locality.ID > 0
                ? $".{locality.ID}"
                : string.Empty;

            var profileId = $"I{discId}{suffix}";

            var profileIds = ((object[])this.Api.GetAllProfileIDs()).Cast<string>().ToList();

            var existingProfileId = profileIds.FirstOrDefault(id => profileId.Equals(id));

            var formattedDiscId = FormatDiscId(discId);

            if (!string.IsNullOrEmpty(existingProfileId))
            {
                this.Api.SelectDVDByProfileID(profileId);

                this.Cursor = previousCursor;

                this.UIServices.ShowMessageBox(string.Format(MessageBoxTexts.ProfileAlreadyExists, formattedDiscId, locality.Description), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);
            }
            else
            {
                if (_allProfiles == null)
                {
                    _allProfiles = profileIds.Select(id =>
                    {
                        this.Api.DVDByProfileID(out var profile, id, 0, 0);

                        return profile;
                    }).ToList();
                }

                this.DefaultValues.DownloadProfile = UpdateFromOnlineDatabaseCheckBox.Checked;

                var parentProfile = AddAsChildCheckBox.Checked
                    ? _parentProfile
                    : null;

                this.Cursor = previousCursor;

                using (var form = new AddDiscForm(_serviceProvider, locality, profileId, discId, drive, parentProfile, _allProfiles, formattedDiscId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        profileIds.Add(profileId);

                        _allProfiles.Add(form.NewProfile);
                    }
                }
            }
        }

        private static string FormatDiscId(string unformatted)
        {
            const int ChunkSize = 4;

            var chunks = Enumerable.Range(0, unformatted.Length / ChunkSize).Select(chunkIndex => unformatted.Substring(chunkIndex * ChunkSize, ChunkSize));

            var formatted = string.Join("-", chunks);

            return formatted;
        }

        private void OnAbortButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
    }
}