namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

        private readonly IDVDInfo _parentProfile;

        internal IDVDInfo NewProfile { get; private set; }

        private UI.IUIServices UIServices => _serviceProvider.UIServices;

        private IDVDProfilerAPI Api => _serviceProvider.Api;

        private DefaultValues DefaultValues => _serviceProvider.DefaultValues;

        public AddDiscForm(ServiceProvider serviceProvider, Locality locality, string profileId, string discId, IDriveInfo drive, IDVDInfo parentProfile, IEnumerable<IDVDInfo> profiles, string formattedDiscId)
        {
            _serviceProvider = serviceProvider;
            _locality = locality;
            _profileId = profileId;
            _discId = discId;
            _drive = drive;
            _parentProfile = parentProfile;

            InitializeComponent();

            Icon = Properties.Resources.DJDSOFT;

            Text = $"{Text}: {formattedDiscId} ({locality.Description})";

            DialogResult = DialogResult.None;

            Load += (s, e) => OnAddDiscFormLoad(profiles);
        }

        private void OnAddDiscFormLoad(IEnumerable<IDVDInfo> profiles)
        {
            CollectionNumberUpDown.Value = GetNextFreeNumber(profiles);

            LoadProfile();

            InitForm();
        }

        private int GetNextFreeNumber(IEnumerable<IDVDInfo> profiles)
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
                    Api.GetOnlineProfileByID(out var profile, _profileId);

                    NewProfile = profile;
                }
                catch
                { }
            }

            if (NewProfile == null)
            {
                InitProfile();
            }
        }

        private void InitProfile()
        {
            NewProfile = Api.CreateDVD();

            NewProfile.SetProfileID(_profileId);

            NewProfile.SetProfileTimestamp(DateTime.Now);

            NewProfile.SetChangesMadeIndicator(true);
            NewProfile.SetContributableChangesMade(true);

            NewProfile.SetCollectionType(PluginConstants.COLLTYPE_Owned);
            NewProfile.SetRegionByID(_locality.DVDRegion, true);
            NewProfile.SetMediaTypes(true, false, false, false);

            if (GetNtscCountries().Any(c => c.Equals(_locality.Description)))
            {
                NewProfile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                NewProfile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }

            try
            {
                Api.GetInfoOnlineProfileID(_profileId, out var title, out _, out var edition, out _, out _, out _, out _, out _, out _, out _, out _);

                NewProfile.SetTitle(title);
                NewProfile.SetSortTitle(title);

                NewProfile.SetEdition(edition);
            }
            catch
            { }
        }

        private void InitForm()
        {
            TitleTextBox.Text = NewProfile.GetTitle();
            EditionTextBox.Text = NewProfile.GetEdition();
            SortTitleTextBox.Text = NewProfile.GetSortTitle();

            Region1CheckBox.Checked = NewProfile.GetRegionByID(1);
            Region2CheckBox.Checked = NewProfile.GetRegionByID(2);
            Region3CheckBox.Checked = NewProfile.GetRegionByID(3);
            Region4CheckBox.Checked = NewProfile.GetRegionByID(4);
            Region5CheckBox.Checked = NewProfile.GetRegionByID(5);
            Region6CheckBox.Checked = NewProfile.GetRegionByID(6);

            if (NewProfile.GetVideoStandard() == PluginConstants.VIDSTD_NTSC)
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

        private void OnNoCollectionNumberCheckBoxCheckedChanged(object sender, EventArgs e)
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
            TitleTextBox.Text = GetWindows1252Text(TitleTextBox.Text);

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

        private void OnEditionTextBoxTextChanged(object sender, EventArgs e)
        {
            EditionTextBox.Text = GetWindows1252Text(EditionTextBox.Text);
        }

        private void OnSortTitleTextBoxTextChanged(object sender, EventArgs e)
        {
            SortTitleTextBox.Text = GetWindows1252Text(SortTitleTextBox.Text);
        }

        private static string GetWindows1252Text(string utfText)
        {
            var utf8Encoding = Encoding.UTF8;

            var utfbytes = utf8Encoding.GetBytes(utfText);

            var win1252Encoding = Encoding.GetEncoding(1252);

            var win1252Bytes = Encoding.Convert(utf8Encoding, win1252Encoding, utfbytes);

            var win1252Text = win1252Encoding.GetString(win1252Bytes);

            return win1252Text;
        }


        private void OnAddDiscFormFormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                DefaultValues.CreateDiscIdContent = CreateDiscContentCheckBox.Checked;
            }
        }

        private void OnAddDiscButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                UIServices.ShowMessageBox(MessageBoxTexts.TitleIsEmpty, MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            NewProfile.SetTitle(TitleTextBox.Text);
            NewProfile.SetEdition(EditionTextBox.Text);

            if (!string.IsNullOrWhiteSpace(SortTitleTextBox.Text))
            {
                NewProfile.SetSortTitle(SortTitleTextBox.Text);
            }
            else
            {
                NewProfile.SetSortTitle(TitleTextBox.Text);
            }

            NewProfile.SetRegionByID(1, Region1CheckBox.Checked);
            NewProfile.SetRegionByID(2, Region2CheckBox.Checked);
            NewProfile.SetRegionByID(3, Region3CheckBox.Checked);
            NewProfile.SetRegionByID(4, Region4CheckBox.Checked);
            NewProfile.SetRegionByID(5, Region5CheckBox.Checked);
            NewProfile.SetRegionByID(6, Region6CheckBox.Checked);

            if (VideoStandardNtscRadioButton.Checked)
            {
                NewProfile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                NewProfile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }

            NewProfile.SetPurchaseDate(PurchaseDatePicker.Value.Date);

            if (NoCollectionNumberCheckBox.Checked)
            {
                NewProfile.SetCollectionNumber("-1");
            }
            else if (CollectionNumberUpDown.Value == 0)
            {
                NewProfile.SetCollectionNumber(string.Empty);
            }
            else
            {
                NewProfile.SetCollectionNumber(((int)CollectionNumberUpDown.Value).ToString());
            }

            if (CreateDiscContentCheckBox.Checked)
            {
                CreateDiscContent();
            }

            NewProfile.SetCountAs((int)CountAsUpDown.Value);

            Api.SaveDVDToCollection(NewProfile);

            if (DefaultValues.DownloadProfile)
            {
                try
                {
                    Api.DownloadCoverImagesForProfileID(_profileId, string.Empty);
                }
                catch
                { }
            }

            if (_parentProfile != null)
            {
                _parentProfile.AddBoxSetContent(_profileId);

                Api.SaveDVDToCollection(_parentProfile);
            }

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

            for (var discIndex = 0; discIndex < NewProfile.GetDiscCount(); discIndex++)
            {
                NewProfile.GetDiscByIndex(discIndex, out _, out _, out _, out _, out var discIdSideA, out var discIdSideB, out _, out _, out _, out _);

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

                    NewProfile.AddDisc("Main Feature", string.Empty, _drive.VolumeLabel, string.Empty, _discId, string.Empty, isDualLayered, false, string.Empty, string.Empty);
                }
                catch
                { }
            }
        }

        private void OnAbortButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
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