namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using AbstractionLayer.IOServices;
    using DVDProfilerXML.Version400.Localities;
    using Invelos.DVDProfilerPlugin;
    using UI = AbstractionLayer.UIServices;

    internal partial class AddDiscForm : Form
    {
        private readonly ServiceProvider _serviceProvider;

        private readonly Locality _locality;

        private readonly string _profileId;

        private readonly string _discId;

        private readonly IDriveInfo _drive;

        private IDVDInfo _profile;

        private UI.IUIServices UIServices => _serviceProvider.UIServices;

        private IDVDProfilerAPI Api => _serviceProvider.ProfilerApi;

        private DefaultValues DefaultValues => _serviceProvider.DefaultValues;

        public AddDiscForm(ServiceProvider serviceProvider, IEnumerable<string> profileIds, Locality locality, string profileId, string discId, string formattedDiscId, AbstractionLayer.IOServices.IDriveInfo drive)
        {
            _serviceProvider = serviceProvider;
            _locality = locality;
            _profileId = profileId;
            _discId = discId;
            _drive = drive;

            InitializeComponent();

            Text = $"{Text}: {formattedDiscId} ({locality.Description})";

            DialogResult = DialogResult.None;

            Load += (s, e) => OnAddDiscFormLoad(profileIds);
        }

        private void OnAddDiscFormLoad(IEnumerable<string> profileIds)
        {
            var previousCursor = Cursor;

            try
            {
                LoadForm(profileIds);
            }
            finally
            {
                Cursor = previousCursor;
            }
        }

        private void LoadForm(IEnumerable<string> profileIds)
        {
            Cursor = Cursors.WaitCursor;

            Application.DoEvents();

            var profiles = profileIds.Select(id =>
            {
                Api.DVDByProfileID(out var profile, id, 0, 0);

                return profile;
            }).ToList();

            CollectionNumberUpDown.Value = GetNextFreeNumber(profiles);

            LoadProfile();

            InitForm();
        }

        private int GetNextFreeNumber(List<IDVDInfo> profiles)
        {
            var collectionNumbers = profiles.Where(profile => profile.GetCollectionType() == PluginConstants.COLLTYPE_Owned).Select(profile => profile.GetCollectionNumber()).ToList();

            var nextFreeNumber = 1;
            for (; nextFreeNumber < ushort.MaxValue; nextFreeNumber++)
            {
                if (!collectionNumbers.Any(n => nextFreeNumber.ToString().Equals(n)))
                {
                    break;
                }
            }

            return nextFreeNumber;
        }

        private void LoadProfile()
        {
            if (DefaultValues.DownloadProfile)
            {
                try
                {
                    Api.GetOnlineProfileByID(out _profile, _profileId);
                }
                catch
                { }
            }

            if (_profile == null)
            {
                InitProfile();
            }
        }

        private void InitProfile()
        {
            _profile = Api.CreateDVD();

            _profile.SetProfileID(_profileId);

            _profile.SetProfileTimestamp(DateTime.Now);

            _profile.SetChangesMadeIndicator(true);
            _profile.SetContributableChangesMade(true);

            _profile.SetCollectionType(PluginConstants.COLLTYPE_Owned);
            _profile.SetRegionByID(_locality.DVDRegion, true);
            _profile.SetMediaTypes(true, false, false, false);

            if (GetNtscCountries().Any(c => c.Equals(_locality.Description)))
            {
                _profile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                _profile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }
        }

        private void InitForm()
        {
            TitleTextBox.Text = _profile.GetTitle();
            EditionTextBox.Text = _profile.GetEdition();
            SortTitleTextBox.Text = _profile.GetSortTitle();

            Region1CheckBox.Checked = _profile.GetRegionByID(1);
            Region2CheckBox.Checked = _profile.GetRegionByID(2);
            Region3CheckBox.Checked = _profile.GetRegionByID(3);
            Region4CheckBox.Checked = _profile.GetRegionByID(4);
            Region5CheckBox.Checked = _profile.GetRegionByID(5);
            Region6CheckBox.Checked = _profile.GetRegionByID(6);

            if (_profile.GetVideoStandard() == PluginConstants.VIDSTD_NTSC)
            {
                VideoStandardNtscRadioButton.Checked = true;
            }
            else
            {
                VideoStandardPalRadioButton.Checked = true;
            }

            PurchaseDatePicker.Value = DateTime.Now.Date;

            CreateDiscContentCheckBox.Checked = DefaultValues.CreateDiscIdContent;
        }

        private void OnNoCollectionNumberCheckBoxCheckedChanged(object sender, System.EventArgs e)
        {
            if (NoCollectionNumberCheckBox.Checked)
            {
                CollectionNumberUpDown.Enabled = false;
            }
            else
            {
                CollectionNumberUpDown.Enabled = true;
            }
        }

        private void OnTitleTextBoxTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SortTitleTextBox.Text))
            {
                SortTitleTextBox.Text = TitleTextBox.Text;
            }
            else if (TitleTextBox.Text.IndexOf(SortTitleTextBox.Text) == 0)
            {
                SortTitleTextBox.Text = TitleTextBox.Text;
            }
            else if (SortTitleTextBox.Text.Length > TitleTextBox.Text.Length)
            {
                var sortTitle = SortTitleTextBox.Text.Substring(0, TitleTextBox.Text.Length);

                if (TitleTextBox.Text.IndexOf(sortTitle) == 0)
                {
                    SortTitleTextBox.Text = TitleTextBox.Text;
                }
            }
        }

        private void OnAddDiscFormFormClosed(object sender, FormClosedEventArgs e)
        {
            DefaultValues.CreateDiscIdContent = CreateDiscContentCheckBox.Checked;
        }

        private void OnAddDiscButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                UIServices.ShowMessageBox(MessageBoxTexts.TitleIsEmpty, MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            _profile.SetTitle(TitleTextBox.Text);
            _profile.SetEdition(EditionTextBox.Text);

            if (!string.IsNullOrWhiteSpace(SortTitleTextBox.Text))
            {
                _profile.SetSortTitle(SortTitleTextBox.Text);
            }
            else
            {
                _profile.SetSortTitle(TitleTextBox.Text);
            }

            _profile.SetRegionByID(1, Region1CheckBox.Checked);
            _profile.SetRegionByID(2, Region2CheckBox.Checked);
            _profile.SetRegionByID(3, Region3CheckBox.Checked);
            _profile.SetRegionByID(4, Region4CheckBox.Checked);
            _profile.SetRegionByID(5, Region5CheckBox.Checked);
            _profile.SetRegionByID(6, Region6CheckBox.Checked);

            if (VideoStandardNtscRadioButton.Checked)
            {
                _profile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                _profile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }

            _profile.SetPurchaseDate(PurchaseDatePicker.Value.Date);

            if (NoCollectionNumberCheckBox.Checked)
            {
                _profile.SetCollectionNumber("-1");
            }
            else if (CollectionNumberUpDown.Value == 0)
            {
                _profile.SetCollectionNumber(string.Empty);
            }
            else
            {
                _profile.SetCollectionNumber(((int)CollectionNumberUpDown.Value).ToString());
            }

            if (CreateDiscContentCheckBox.Checked)
            {
                CreateDiscContent();
            }

            _profile.SetCountAs((int)CountAsUpDown.Value);

            Api.SaveDVDToCollection(_profile);

            try
            {
                Api.DownloadCoverImagesForProfileID(_profileId, string.Empty);
            }
            catch
            { }

            Api.ClearAllFilters();
            Api.RequeryDatabase();

            Api.SelectDVDByProfileID(_profileId);

            Api.ReloadCurrentDVD();

            DialogResult = DialogResult.OK;

            Close();
        }

        private void CreateDiscContent()
        {
            var hasDisc = false;

            for (var discIndex = 0; discIndex < _profile.GetDiscCount(); discIndex++)
            {
                _profile.GetDiscByIndex(discIndex, out _, out _, out _, out _, out var discIdSideA, out var discIdSideB, out _, out _, out _, out _);

                if (_discId.Equals(discIdSideA) || _discId.Equals(discIdSideB))
                {
                    hasDisc = true;

                    break;
                }
            }

            if (!hasDisc)
            {
                try
                {
                    Api.GetDiscIDFromDrive(_drive.RootFolder, out _, out var isDualLayered);

                    _profile.AddDisc("Main Feature", string.Empty, _drive.VolumeLabel, string.Empty, _discId, string.Empty, isDualLayered, false, string.Empty, string.Empty);
                }
                catch
                { }
            }
        }

        private void OnAbortButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private static IEnumerable<string> GetNtscCountries()
        {
            yield return "Canada";
            yield return "Canada (Quebec)";
            yield return "Chile";
            yield return "Colombia";
            yield return "Japan";
            yield return "Mexico";
            yield return "Peru";
            yield return "Philippines";
            yield return "South Korea";
            yield return "Taiwan";
            yield return "United States";
        }
    }
}