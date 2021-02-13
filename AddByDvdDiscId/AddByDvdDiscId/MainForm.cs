namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using AbstractionLayer.IOServices;
    using CalculateDvdDiscId;
    using DVDProfilerXML.Version400.Localities;
    using Invelos.DVDProfilerPlugin;
    using UI = AbstractionLayer.UIServices;

    internal partial class MainForm : Form
    {
        private readonly ServiceProvider _serviceProvider;

        private UI.IUIServices UIServices => _serviceProvider.UIServices;

        private IIOServices IOServices => _serviceProvider.IOServices;

        private IDVDProfilerAPI Api => _serviceProvider.Api;

        private IEnumerable<Locality> Localities => _serviceProvider.Localities;

        private DefaultValues DefaultValues => _serviceProvider.DefaultValues;

        private readonly IDVDInfo _parentProfile;

        private List<IDVDInfo> _allProfiles;

        public MainForm(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();

            Icon = Properties.Resources.DJDSOFT;

            UpdateFromOnlineDatabaseCheckBox.Checked = DefaultValues.DownloadProfile;

            var failure = false;

            failure |= InitializeDriveComboBox();
            failure |= InitializeLocalityComboBox();

            failure |= CheckDecrypterRunning();

            if (failure)
            {
                Load += (s, e) => Close();
            }
            else
            {
                _parentProfile = InitializeParentProfile();
            }
        }

        private bool InitializeDriveComboBox()
        {
            IEnumerable<IDriveInfo> drives;
            try
            {
                drives = IOServices.GetDriveInfos(DriveType.CDRom);
            }
            catch
            {
                drives = Enumerable.Empty<IDriveInfo>();
            }

            var driveViewModels = drives.OrderBy(d => d.DriveLetter).Select(d => new DriveViewModel(d)).ToList();

            DriveComboBox.DataSource = driveViewModels;
            DriveComboBox.ValueMember = nameof(DriveViewModel.Id);
            DriveComboBox.DisplayMember = nameof(DriveViewModel.Description);

            var selectedDrive = driveViewModels.FirstOrDefault(d => d.Id == DefaultValues.SelectedDrive);

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
                UIServices.ShowMessageBox(MessageBoxTexts.NoDriveFound, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InitializeLocalityComboBox()
        {
            var localityViewModels = Localities.OrderBy(l => l.Description).Select(l => new LocalityViewModel(l)).ToList();

            LocalityComboBox.DataSource = localityViewModels;
            LocalityComboBox.ValueMember = nameof(LocalityViewModel.Id);
            LocalityComboBox.DisplayMember = nameof(LocalityViewModel.Description);

            var selectedLocality = localityViewModels.FirstOrDefault(l => l.Id == DefaultValues.SelectedLocality);

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
                UIServices.ShowMessageBox(MessageBoxTexts.NoProfilerLocalityFound, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

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
                UIServices.ShowMessageBox(MessageBoxTexts.DecrypterRunning, MessageBoxTexts.ErrorHeader, UI.Buttons.OK, UI.Icon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private IDVDInfo InitializeParentProfile()
        {
            var displayedProfile = Api.GetDisplayedDVD();

            IDVDInfo parentProfile = null;

            if (displayedProfile != null)
            {
                Api.DVDByProfileID(out parentProfile, displayedProfile.GetProfileID(), PluginConstants.DATASEC_AllSections, -1);

                AddAsChildCheckBox.Enabled = true;
                AddAsChildCheckBox.Checked = DefaultValues.AddAsChild;
            }
            else
            {
                AddAsChildCheckBox.Enabled = false;
                AddAsChildCheckBox.Checked = false;
            }

            return parentProfile;
        }

        private void OnFormFormClosed(object sender, FormClosedEventArgs e)
        {
            TrySaveDrive();

            TrySaveLocality();

            DefaultValues.DownloadProfile = UpdateFromOnlineDatabaseCheckBox.Checked;

            if (AddAsChildCheckBox.Enabled)
            {
                DefaultValues.AddAsChild = AddAsChildCheckBox.Checked;
            }
        }

        private void OnRefreshDrivesButtonClick(object sender, EventArgs e)
        {
            TrySaveDrive();

            DriveComboBox.DataSource = null;
            DriveComboBox.Items.Clear();

            InitializeDriveComboBox();
        }

        private void TrySaveDrive()
        {
            if (DriveComboBox.SelectedIndex >= 0)
            {
                DefaultValues.SelectedDrive = (string)DriveComboBox.SelectedValue;
            }
        }

        private void TrySaveLocality()
        {
            if (LocalityComboBox.SelectedIndex >= 0)
            {
                DefaultValues.SelectedLocality = (int)LocalityComboBox.SelectedValue;
            }
        }

        private void OnAddDiscButtonClick(object sender, EventArgs e)
        {
            try
            {
                var drive = (DriveViewModel)DriveComboBox.SelectedItem;

                var locality = (LocalityViewModel)LocalityComboBox.SelectedItem;

                AddDisc(drive.Drive, locality.Locality);
            }
            catch (Exception ex)
            {
                UIServices.ShowMessageBox(ex.Message, MessageBoxTexts.CriticalErrorHeader, UI.Buttons.OK, UI.Icon.Error);
            }
        }

        private void AddDisc(IDriveInfo drive, Locality locality)
        {
            if (!drive.IsReady)
            {
                UIServices.ShowMessageBox(string.Format(MessageBoxTexts.DriveNotReady, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var videoFolder = IOServices.GetFolderInfo(IOServices.Path.Combine(drive.RootFolder, "VIDEO_TS"));

            if (!videoFolder.Exists)
            {
                UIServices.ShowMessageBox(string.Format(MessageBoxTexts.NoDvdInDrive, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var previousCursor = Cursor;

            try
            {
                TryAddDisc(drive, locality, previousCursor);
            }
            finally
            {
                Cursor = previousCursor;
            }
        }

        private void TryAddDisc(IDriveInfo drive, Locality locality, Cursor previousCursor)
        {
            Cursor = Cursors.WaitCursor;

            Application.DoEvents();

            string discId;
            try
            {
                discId = DvdDiscIdCalculator.Calculate(drive.DriveLetter);
            }
            catch
            {
                UIServices.ShowMessageBox(string.Format(MessageBoxTexts.DriveNotReady, drive.DriveLetter), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            var suffix = locality.ID > 0
                ? $".{locality.ID}"
                : string.Empty;

            var profileId = $"I{discId}{suffix}";

            var profileIds = ((object[])Api.GetAllProfileIDs()).Cast<string>().ToList();

            var existingProfileId = profileIds.FirstOrDefault(id => profileId.Equals(id));

            var formattedDiscId = FormatDiscId(discId);

            if (!string.IsNullOrEmpty(existingProfileId))
            {
                Api.SelectDVDByProfileID(profileId);

                Cursor = previousCursor;

                UIServices.ShowMessageBox(string.Format(MessageBoxTexts.ProfileAlreadyExists, formattedDiscId, locality.Description), MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);
            }
            else
            {
                if (_allProfiles == null)
                {
                    _allProfiles = profileIds.Select(id =>
                    {
                        Api.DVDByProfileID(out var profile, id, 0, 0);

                        return profile;
                    }).ToList();
                }

                DefaultValues.DownloadProfile = UpdateFromOnlineDatabaseCheckBox.Checked;

                var parentProfile = AddAsChildCheckBox.Checked
                    ? _parentProfile
                    : null;

                Cursor = previousCursor;

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
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}