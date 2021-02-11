using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    public partial class MainForm : Form
    {
        private readonly IDVDProfilerAPI _api;

        private readonly DefaultValues _defaultValues;

        public MainForm(IDVDProfilerAPI api, DefaultValues defaultValues, IEnumerable<Locality> localities)
        {
            _api = api;
            _defaultValues = defaultValues;

            InitializeComponent();

            var failure = false;

            failure |= InitializeDriveComboBox();
            failure |= InitializeLocalityComboBox(localities);

            failure |= CheckDecrypterRunning();

            if (failure)
            {
                Load += (s, e) => Close();
            }
        }

        private bool InitializeDriveComboBox()
        {
            IEnumerable<DriveInfo> drives;
            try
            {
                drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom);
            }
            catch
            {
                drives = Enumerable.Empty<DriveInfo>();
            }

            var driveViewModels = drives.OrderBy(d => d.Name).Select(d => new DriveViewModel(d)).ToList();

            DriveComboBox.DataSource = driveViewModels;
            DriveComboBox.ValueMember = nameof(DriveViewModel.Id);
            DriveComboBox.DisplayMember = nameof(DriveViewModel.Description);

            var selectedDrive = driveViewModels.FirstOrDefault(d => d.Id == _defaultValues.SelectedDrive);

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
                MessageBox.Show(MessageBoxTexts.NoDriveFound, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InitializeLocalityComboBox(IEnumerable<Locality> localities)
        {
            var localityViewModels = (localities ?? Enumerable.Empty<Locality>()).OrderBy(l => l.Description).Select(l => new LocalityViewModel(l)).ToList();

            LocalityComboBox.DataSource = localityViewModels;
            LocalityComboBox.ValueMember = nameof(LocalityViewModel.Id);
            LocalityComboBox.DisplayMember = nameof(LocalityViewModel.Description);

            var selectedLocality = localityViewModels.FirstOrDefault(l => l.Id == _defaultValues.SelectedLocality);

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
                MessageBox.Show(MessageBoxTexts.NoProfilerLocalityFound, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show(MessageBoxTexts.DecrypterRunning, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }


        private void OnFormFormClosed(object sender, FormClosedEventArgs e)
        {
            TrySaveDrive();

            TrySaveLocality();
        }

        private void OnRefreshDrivesButtonClick(object sender, System.EventArgs e)
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
                _defaultValues.SelectedDrive = (string)DriveComboBox.SelectedValue;
            }
        }

        private void TrySaveLocality()
        {
            if (LocalityComboBox.SelectedIndex >= 0)
            {
                _defaultValues.SelectedLocality = (int)LocalityComboBox.SelectedValue;
            }
        }
    }
}