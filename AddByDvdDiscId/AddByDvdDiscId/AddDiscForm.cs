using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DoenaSoft.AbstractionLayer.IOServices;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version400.Localities;
using Invelos.DVDProfilerPlugin;
using UI = DoenaSoft.AbstractionLayer.UIServices;

namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
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

            this.InitializeComponent();

            this.Icon = Properties.Resources.DJDSOFT;

            this.Text = $"{this.Text}: {formattedDiscId} ({locality.Description})";

            this.DialogResult = DialogResult.None;

            Load += (s, e) => this.OnAddDiscFormLoad(profiles);
        }

        private void OnAddDiscFormLoad(IEnumerable<IDVDInfo> profiles)
        {
            CollectionNumberUpDown.Value = this.GetNextFreeNumber(profiles);

            this.LoadProfile();

            this.InitForm();
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
            if (this.DefaultValues.DownloadProfile)
            {
                try
                {
                    this.Api.GetOnlineProfileByID(out var profile, _profileId);

                    this.NewProfile = profile;
                }
                catch
                { }
            }

            if (this.NewProfile == null)
            {
                this.InitProfile();
            }
        }

        private void InitProfile()
        {
            this.NewProfile = this.Api.CreateDVD();

            this.NewProfile.SetProfileID(_profileId);

            this.NewProfile.SetProfileTimestamp(DateTime.Now);

            this.NewProfile.SetChangesMadeIndicator(true);
            this.NewProfile.SetContributableChangesMade(true);

            this.NewProfile.SetCollectionType(PluginConstants.COLLTYPE_Owned);
            this.NewProfile.SetRegionByID(_locality.DVDRegion, true);
            this.NewProfile.SetMediaTypes(true, false, false, false);

            if (GetNtscCountries().Any(c => c.Equals(_locality.ID)))
            {
                this.NewProfile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                this.NewProfile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }

            var currency = this.GetCurrency();

            if (currency.HasValue)
            {
                this.NewProfile.SetSRPCurrency(currency.Value);
            }

            try
            {
                this.Api.GetInfoOnlineProfileID(_profileId, out var title, out _, out var edition, out _, out _, out _, out _, out _, out _, out _, out _);

                this.NewProfile.SetTitle(title);
                this.NewProfile.SetSortTitle(title);
                this.NewProfile.SetEdition(edition);
            }
            catch
            { }

            if (string.IsNullOrEmpty(this.NewProfile.GetTitle()) && _parentProfile != null)
            {
                var title = _parentProfile.GetTitle();

                var sortTitle = _parentProfile.GetSortTitle();

                var edition = _parentProfile.GetEdition();

                this.NewProfile.SetTitle(title);
                this.NewProfile.SetSortTitle(title);
                this.NewProfile.SetEdition(edition);
            }
        }

        private void InitForm()
        {
            TitleTextBox.Text = this.NewProfile.GetTitle();
            EditionTextBox.Text = this.NewProfile.GetEdition();
            SortTitleTextBox.Text = this.NewProfile.GetSortTitle();

            Region1CheckBox.Checked = this.NewProfile.GetRegionByID(1);
            Region2CheckBox.Checked = this.NewProfile.GetRegionByID(2);
            Region3CheckBox.Checked = this.NewProfile.GetRegionByID(3);
            Region4CheckBox.Checked = this.NewProfile.GetRegionByID(4);
            Region5CheckBox.Checked = this.NewProfile.GetRegionByID(5);
            Region6CheckBox.Checked = this.NewProfile.GetRegionByID(6);

            if (this.NewProfile.GetVideoStandard() == PluginConstants.VIDSTD_NTSC)
            {
                VideoStandardNtscRadioButton.Checked = true;
            }
            else
            {
                VideoStandardPalRadioButton.Checked = true;
            }

            PurchaseDatePicker.Value = DateTime.Now.Date;

            CreateDiscContentCheckBox.Checked = this.DefaultValues.CreateDiscIdContent;
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
            if (this.DialogResult == DialogResult.OK)
            {
                this.DefaultValues.CreateDiscIdContent = CreateDiscContentCheckBox.Checked;
            }
        }

        private void OnAddDiscButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                this.UIServices.ShowMessageBox(MessageBoxTexts.TitleIsEmpty, MessageBoxTexts.WarningHeader, UI.Buttons.OK, UI.Icon.Warning);

                return;
            }

            this.NewProfile.SetTitle(TitleTextBox.Text);
            this.NewProfile.SetEdition(EditionTextBox.Text);

            if (!string.IsNullOrWhiteSpace(SortTitleTextBox.Text))
            {
                this.NewProfile.SetSortTitle(SortTitleTextBox.Text);
            }
            else
            {
                this.NewProfile.SetSortTitle(TitleTextBox.Text);
            }

            this.NewProfile.SetRegionByID(1, Region1CheckBox.Checked);
            this.NewProfile.SetRegionByID(2, Region2CheckBox.Checked);
            this.NewProfile.SetRegionByID(3, Region3CheckBox.Checked);
            this.NewProfile.SetRegionByID(4, Region4CheckBox.Checked);
            this.NewProfile.SetRegionByID(5, Region5CheckBox.Checked);
            this.NewProfile.SetRegionByID(6, Region6CheckBox.Checked);

            if (VideoStandardNtscRadioButton.Checked)
            {
                this.NewProfile.SetVideoStandard(PluginConstants.VIDSTD_NTSC);
            }
            else
            {
                this.NewProfile.SetVideoStandard(PluginConstants.VIDSTD_PAL);
            }

            var currency = this.GetCurrency();

            if (currency.HasValue)
            {
                this.NewProfile.SetPurchasePriceCurrency(currency.Value);
            }

            this.NewProfile.SetPurchaseDate(PurchaseDatePicker.Value.Date);

            if (NoCollectionNumberCheckBox.Checked)
            {
                this.NewProfile.SetCollectionNumber("-1");
            }
            else if (CollectionNumberUpDown.Value == 0)
            {
                this.NewProfile.SetCollectionNumber(string.Empty);
            }
            else
            {
                this.NewProfile.SetCollectionNumber(((int)CollectionNumberUpDown.Value).ToString());
            }

            if (CreateDiscContentCheckBox.Checked)
            {
                this.CreateDiscContent();
            }

            this.NewProfile.SetCountAs((int)CountAsUpDown.Value);

            this.Api.SaveDVDToCollection(this.NewProfile);

            if (this.DefaultValues.DownloadProfile)
            {
                try
                {
                    this.Api.DownloadCoverImagesForProfileID(_profileId, string.Empty);
                }
                catch
                { }
            }

            if (_parentProfile != null)
            {
                _parentProfile.AddBoxSetContent(_profileId);

                this.Api.SaveDVDToCollection(_parentProfile);
            }

            this.Api.ClearAllFilters();
            this.Api.RequeryDatabase();

            if (_parentProfile != null)
            {
                this.Api.SelectDVDByProfileID(_parentProfile.GetProfileID());
                this.Api.ReloadCurrentDVD();
            }
            else
            {
                this.Api.SelectDVDByProfileID(_profileId);
                this.Api.ReloadCurrentDVD();
            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void CreateDiscContent()
        {
            var hasDisc = false;

            for (var discIndex = 0; discIndex < this.NewProfile.GetDiscCount(); discIndex++)
            {
                this.NewProfile.GetDiscByIndex(discIndex, out _, out _, out _, out _, out var discIdSideA, out var discIdSideB, out _, out _, out _, out _);

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
                    this.Api.GetDiscIDFromDrive(_drive.RootFolderName, out _, out var isDualLayered);

                    this.NewProfile.AddDisc("Main Feature", string.Empty, _drive.VolumeLabel, string.Empty, _discId, string.Empty, isDualLayered, false, string.Empty, string.Empty);
                }
                catch
                { }
            }
        }

        private void OnAbortButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private static IEnumerable<int> GetNtscCountries()
        {
            yield return 3; //Canada
            yield return 19; //Canada (Quebec)
            yield return 49; //Chile
            yield return 50; //Colombia
            yield return 17; //Japan
            yield return 25; //Mexico
            yield return 51; //Peru
            yield return 43; //Philippines
            yield return 18; //South Korea
            yield return 28; //Taiwan
            yield return 0; //United States
        }

        private int? GetCurrency()
        {
            switch (_locality.ID)
            {
                case 32: //Argentina
                    {
                        return PluginConstants.CURRENCY_ARP; //Argentina (Peso)
                    }
                case 2:  //Australia
                    {
                        return PluginConstants.CURRENCY_AUD; //Australia (Dollar)
                    }
                case 23: //Brazil
                    {
                        return PluginConstants.CURRENCY_BRL; //Brazil (Real)
                    }
                case 3:  //Canada
                case 19: //Canada (Quebec)
                    {
                        return PluginConstants.CURRENCY_CAD; //Canada (Dollar)
                    }
                case 49: //Chile
                    {
                        return PluginConstants.CURRENCY_CLP; //Chile (Peso)
                    }
                case 6:  //China
                    {
                        return PluginConstants.CURRENCY_CNY; //China (Renminbi)
                    }
                case 36: //Czech Republic
                    {
                        return PluginConstants.CURRENCY_CZK; //Czech Republic (Koruna)
                    }
                case 14: //"Denmark"
                    {
                        return PluginConstants.CURRENCY_DKK; //Denmark (Krone)
                    }
                case 45: //Estonia
                    {
                        return PluginConstants.CURRENCY_EEK; //Estonia (Kroon)
                    }
                case 4:  //United Kingdom
                    {
                        return PluginConstants.CURRENCY_GBP; //Great Britain (Pound)
                    }
                case 21: //Hong Kong
                    {
                        return PluginConstants.CURRENCY_HKD; //Hong Kong (Dollar)
                    }
                case 34: //Hungary
                    {
                        return PluginConstants.CURRENCY_HUF; //Hungary (Forint)
                    }
                case 26: //Iceland
                    {
                        return PluginConstants.CURRENCY_ISK; //Iceland (Krona)
                    }
                case 39: //India
                    {
                        return PluginConstants.CURRENCY_INR; //India (Rupee)
                    }
                case 27: //Indonesia
                    {
                        return PluginConstants.CURRENCY_IDR; //Indonesia (Rupiah)
                    }
                case 24: //Israel
                    {
                        return PluginConstants.CURRENCY_ILS; //Israel (Shekel)
                    }
                case 17: //Japan
                    {
                        return PluginConstants.CURRENCY_JPY; //Japan (Yen)
                    }
                case 37: //Malaysia
                    {
                        return PluginConstants.CURRENCY_MYR; //Malaysia (Ringgit)
                    }
                case 25: //Mexico
                    {
                        return PluginConstants.CURRENCY_MXP; //Mexico (New Peso)
                    }
                case 1:  //New Zealand
                    {
                        return PluginConstants.CURRENCY_NZD; //New Zealand (Dollar)
                    }
                case 12: //Norway
                    {
                        return PluginConstants.CURRENCY_NOK; //Norway (Krone)
                    }
                case 43: //Philippines
                    {
                        return PluginConstants.CURRENCY_PHP; //Philippines (Peso)
                    }
                case 29: //Poland
                    {
                        return PluginConstants.CURRENCY_PLN; //Poland (Zloty)
                    }
                case 46: //Romania
                    {
                        return PluginConstants.CURRENCY_RON; //Romania (New leu)
                    }
                case 48: //Russia
                    {
                        return PluginConstants.CURRENCY_RUR; //Russia (Rouble)
                    }
                case 35: //Singapore
                    {
                        return PluginConstants.CURRENCY_SGD; //Singapore (Dollar)
                    }
                //case 33: //Slovakia - part of the Euro since 2009
                //    {
                //        return PluginConstants.CURRENCY_SKK; //Slovakia (Koruna)
                //    }
                case 20: //South Africa
                    {
                        return PluginConstants.CURRENCY_ZAR; //South Africa (Rand)
                    }
                case 18: //South Korea
                    {
                        return PluginConstants.CURRENCY_KRW; //South Korea (Won)
                    }
                case 11: //Sweden
                    {
                        return PluginConstants.CURRENCY_SEK; //Sweden (Krona)
                    }
                case 22: //Switzerland
                    {
                        return PluginConstants.CURRENCY_CHF; //Switzerland (Franc)
                    }
                case 28: //Taiwan
                    {
                        return PluginConstants.CURRENCY_TWD; //Taiwan (Dollar)
                    }
                case 38: //Thailand
                    {
                        return PluginConstants.CURRENCY_THB; //Thailand (Baht)
                    }
                case 31: //Turkey
                    {
                        return PluginConstants.CURRENCY_TRL; //Turkey (1 million Lira)
                    }
                case 0:  //United States
                    {
                        return PluginConstants.CURRENCY_USD; //United States (Dollar)
                    }
                case 42: //Vietnam
                    {
                        return PluginConstants.CURRENCY_VND; //Vietnam (Dong)
                    }
                case 40: //Austria
                case 30: //Belgium
                case 16: //Finland
                case 8:  //France
                case 5:  //Germany
                case 41: //Greece
                case 44: //Ireland
                case 13: //Italy
                case 9:  //Netherlands
                case 15: //Portugal
                case 33: //Slovakia - part of the Euro since 2009
                case 10: //Spain
                    {
                        return PluginConstants.CURRENCY_EUR; //Europe (Euro)
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}