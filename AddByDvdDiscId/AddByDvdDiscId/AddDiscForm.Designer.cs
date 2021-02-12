
namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    partial class AddDiscForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox VideoStandardGroupBox;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDiscForm));
            System.Windows.Forms.GroupBox TitleGroupBox;
            System.Windows.Forms.Label SortTitleLabel;
            System.Windows.Forms.Label EditionLabel;
            System.Windows.Forms.Label TitleLabel;
            System.Windows.Forms.GroupBox RegionGroupBox;
            System.Windows.Forms.GroupBox PurchaseInfoGroupBox;
            System.Windows.Forms.Label CountAsLabel;
            System.Windows.Forms.Label CollectionNumberLabel;
            System.Windows.Forms.Label PurchaseDateLabel;
            System.Windows.Forms.GroupBox DiscsGrooupBox;
            System.Windows.Forms.Button AddDiscButton;
            System.Windows.Forms.Button AbortButton;
            this.VideoStandardPalRadioButton = new System.Windows.Forms.RadioButton();
            this.VideoStandardNtscRadioButton = new System.Windows.Forms.RadioButton();
            this.SortTitleTextBox = new System.Windows.Forms.TextBox();
            this.EditionTextBox = new System.Windows.Forms.TextBox();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.Region6CheckBox = new System.Windows.Forms.CheckBox();
            this.Region5CheckBox = new System.Windows.Forms.CheckBox();
            this.Region4CheckBox = new System.Windows.Forms.CheckBox();
            this.Region3CheckBox = new System.Windows.Forms.CheckBox();
            this.Region2CheckBox = new System.Windows.Forms.CheckBox();
            this.Region1CheckBox = new System.Windows.Forms.CheckBox();
            this.PurchaseDatePicker = new System.Windows.Forms.DateTimePicker();
            this.CountAsUpDown = new System.Windows.Forms.NumericUpDown();
            this.CollectionNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.NoCollectionNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateDiscContentCheckBox = new System.Windows.Forms.CheckBox();
            VideoStandardGroupBox = new System.Windows.Forms.GroupBox();
            TitleGroupBox = new System.Windows.Forms.GroupBox();
            SortTitleLabel = new System.Windows.Forms.Label();
            EditionLabel = new System.Windows.Forms.Label();
            TitleLabel = new System.Windows.Forms.Label();
            RegionGroupBox = new System.Windows.Forms.GroupBox();
            PurchaseInfoGroupBox = new System.Windows.Forms.GroupBox();
            CountAsLabel = new System.Windows.Forms.Label();
            CollectionNumberLabel = new System.Windows.Forms.Label();
            PurchaseDateLabel = new System.Windows.Forms.Label();
            DiscsGrooupBox = new System.Windows.Forms.GroupBox();
            AddDiscButton = new System.Windows.Forms.Button();
            AbortButton = new System.Windows.Forms.Button();
            VideoStandardGroupBox.SuspendLayout();
            TitleGroupBox.SuspendLayout();
            RegionGroupBox.SuspendLayout();
            PurchaseInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CountAsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionNumberUpDown)).BeginInit();
            DiscsGrooupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // VideoStandardGroupBox
            // 
            resources.ApplyResources(VideoStandardGroupBox, "VideoStandardGroupBox");
            VideoStandardGroupBox.Controls.Add(this.VideoStandardPalRadioButton);
            VideoStandardGroupBox.Controls.Add(this.VideoStandardNtscRadioButton);
            VideoStandardGroupBox.Name = "VideoStandardGroupBox";
            VideoStandardGroupBox.TabStop = false;
            // 
            // VideoStandardPalRadioButton
            // 
            resources.ApplyResources(this.VideoStandardPalRadioButton, "VideoStandardPalRadioButton");
            this.VideoStandardPalRadioButton.Name = "VideoStandardPalRadioButton";
            this.VideoStandardPalRadioButton.TabStop = true;
            this.VideoStandardPalRadioButton.UseVisualStyleBackColor = true;
            // 
            // VideoStandardNtscRadioButton
            // 
            resources.ApplyResources(this.VideoStandardNtscRadioButton, "VideoStandardNtscRadioButton");
            this.VideoStandardNtscRadioButton.Name = "VideoStandardNtscRadioButton";
            this.VideoStandardNtscRadioButton.TabStop = true;
            this.VideoStandardNtscRadioButton.UseVisualStyleBackColor = true;
            // 
            // TitleGroupBox
            // 
            resources.ApplyResources(TitleGroupBox, "TitleGroupBox");
            TitleGroupBox.Controls.Add(SortTitleLabel);
            TitleGroupBox.Controls.Add(EditionLabel);
            TitleGroupBox.Controls.Add(TitleLabel);
            TitleGroupBox.Controls.Add(this.SortTitleTextBox);
            TitleGroupBox.Controls.Add(this.EditionTextBox);
            TitleGroupBox.Controls.Add(this.TitleTextBox);
            TitleGroupBox.Name = "TitleGroupBox";
            TitleGroupBox.TabStop = false;
            // 
            // SortTitleLabel
            // 
            resources.ApplyResources(SortTitleLabel, "SortTitleLabel");
            SortTitleLabel.Name = "SortTitleLabel";
            // 
            // EditionLabel
            // 
            resources.ApplyResources(EditionLabel, "EditionLabel");
            EditionLabel.Name = "EditionLabel";
            // 
            // TitleLabel
            // 
            resources.ApplyResources(TitleLabel, "TitleLabel");
            TitleLabel.Name = "TitleLabel";
            // 
            // SortTitleTextBox
            // 
            resources.ApplyResources(this.SortTitleTextBox, "SortTitleTextBox");
            this.SortTitleTextBox.Name = "SortTitleTextBox";
            // 
            // EditionTextBox
            // 
            resources.ApplyResources(this.EditionTextBox, "EditionTextBox");
            this.EditionTextBox.Name = "EditionTextBox";
            // 
            // TitleTextBox
            // 
            resources.ApplyResources(this.TitleTextBox, "TitleTextBox");
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.TextChanged += new System.EventHandler(this.OnTitleTextBoxTextChanged);
            // 
            // RegionGroupBox
            // 
            resources.ApplyResources(RegionGroupBox, "RegionGroupBox");
            RegionGroupBox.Controls.Add(this.Region6CheckBox);
            RegionGroupBox.Controls.Add(this.Region5CheckBox);
            RegionGroupBox.Controls.Add(this.Region4CheckBox);
            RegionGroupBox.Controls.Add(this.Region3CheckBox);
            RegionGroupBox.Controls.Add(this.Region2CheckBox);
            RegionGroupBox.Controls.Add(this.Region1CheckBox);
            RegionGroupBox.Name = "RegionGroupBox";
            RegionGroupBox.TabStop = false;
            // 
            // Region6CheckBox
            // 
            resources.ApplyResources(this.Region6CheckBox, "Region6CheckBox");
            this.Region6CheckBox.Name = "Region6CheckBox";
            this.Region6CheckBox.UseVisualStyleBackColor = true;
            // 
            // Region5CheckBox
            // 
            resources.ApplyResources(this.Region5CheckBox, "Region5CheckBox");
            this.Region5CheckBox.Name = "Region5CheckBox";
            this.Region5CheckBox.UseVisualStyleBackColor = true;
            // 
            // Region4CheckBox
            // 
            resources.ApplyResources(this.Region4CheckBox, "Region4CheckBox");
            this.Region4CheckBox.Name = "Region4CheckBox";
            this.Region4CheckBox.UseVisualStyleBackColor = true;
            // 
            // Region3CheckBox
            // 
            resources.ApplyResources(this.Region3CheckBox, "Region3CheckBox");
            this.Region3CheckBox.Name = "Region3CheckBox";
            this.Region3CheckBox.UseVisualStyleBackColor = true;
            // 
            // Region2CheckBox
            // 
            resources.ApplyResources(this.Region2CheckBox, "Region2CheckBox");
            this.Region2CheckBox.Name = "Region2CheckBox";
            this.Region2CheckBox.UseVisualStyleBackColor = true;
            // 
            // Region1CheckBox
            // 
            resources.ApplyResources(this.Region1CheckBox, "Region1CheckBox");
            this.Region1CheckBox.Name = "Region1CheckBox";
            this.Region1CheckBox.UseVisualStyleBackColor = true;
            // 
            // PurchaseInfoGroupBox
            // 
            resources.ApplyResources(PurchaseInfoGroupBox, "PurchaseInfoGroupBox");
            PurchaseInfoGroupBox.Controls.Add(CountAsLabel);
            PurchaseInfoGroupBox.Controls.Add(CollectionNumberLabel);
            PurchaseInfoGroupBox.Controls.Add(PurchaseDateLabel);
            PurchaseInfoGroupBox.Controls.Add(this.PurchaseDatePicker);
            PurchaseInfoGroupBox.Controls.Add(this.CountAsUpDown);
            PurchaseInfoGroupBox.Controls.Add(this.CollectionNumberUpDown);
            PurchaseInfoGroupBox.Controls.Add(this.NoCollectionNumberCheckBox);
            PurchaseInfoGroupBox.Name = "PurchaseInfoGroupBox";
            PurchaseInfoGroupBox.TabStop = false;
            // 
            // CountAsLabel
            // 
            resources.ApplyResources(CountAsLabel, "CountAsLabel");
            CountAsLabel.Name = "CountAsLabel";
            // 
            // CollectionNumberLabel
            // 
            resources.ApplyResources(CollectionNumberLabel, "CollectionNumberLabel");
            CollectionNumberLabel.Name = "CollectionNumberLabel";
            // 
            // PurchaseDateLabel
            // 
            resources.ApplyResources(PurchaseDateLabel, "PurchaseDateLabel");
            PurchaseDateLabel.Name = "PurchaseDateLabel";
            // 
            // PurchaseDatePicker
            // 
            resources.ApplyResources(this.PurchaseDatePicker, "PurchaseDatePicker");
            this.PurchaseDatePicker.Name = "PurchaseDatePicker";
            // 
            // CountAsUpDown
            // 
            resources.ApplyResources(this.CountAsUpDown, "CountAsUpDown");
            this.CountAsUpDown.Maximum = new decimal(new int[] {
            32676,
            0,
            0,
            0});
            this.CountAsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CountAsUpDown.Name = "CountAsUpDown";
            this.CountAsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CollectionNumberUpDown
            // 
            resources.ApplyResources(this.CollectionNumberUpDown, "CollectionNumberUpDown");
            this.CollectionNumberUpDown.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.CollectionNumberUpDown.Name = "CollectionNumberUpDown";
            this.CollectionNumberUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NoCollectionNumberCheckBox
            // 
            resources.ApplyResources(this.NoCollectionNumberCheckBox, "NoCollectionNumberCheckBox");
            this.NoCollectionNumberCheckBox.Name = "NoCollectionNumberCheckBox";
            this.NoCollectionNumberCheckBox.UseVisualStyleBackColor = true;
            this.NoCollectionNumberCheckBox.CheckedChanged += new System.EventHandler(this.OnNoCollectionNumberCheckBoxCheckedChanged);
            // 
            // DiscsGrooupBox
            // 
            resources.ApplyResources(DiscsGrooupBox, "DiscsGrooupBox");
            DiscsGrooupBox.Controls.Add(this.CreateDiscContentCheckBox);
            DiscsGrooupBox.Name = "DiscsGrooupBox";
            DiscsGrooupBox.TabStop = false;
            // 
            // CreateDiscContentCheckBox
            // 
            resources.ApplyResources(this.CreateDiscContentCheckBox, "CreateDiscContentCheckBox");
            this.CreateDiscContentCheckBox.Name = "CreateDiscContentCheckBox";
            this.CreateDiscContentCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddDiscButton
            // 
            resources.ApplyResources(AddDiscButton, "AddDiscButton");
            AddDiscButton.Name = "AddDiscButton";
            AddDiscButton.UseVisualStyleBackColor = true;
            AddDiscButton.Click += new System.EventHandler(this.OnAddDiscButtonClick);
            // 
            // AbortButton
            // 
            resources.ApplyResources(AbortButton, "AbortButton");
            AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            AbortButton.Name = "AbortButton";
            AbortButton.UseVisualStyleBackColor = true;
            AbortButton.Click += new System.EventHandler(this.OnAbortButtonClick);
            // 
            // AddDiscForm
            // 
            this.AcceptButton = AddDiscButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = AbortButton;
            this.Controls.Add(AbortButton);
            this.Controls.Add(AddDiscButton);
            this.Controls.Add(DiscsGrooupBox);
            this.Controls.Add(PurchaseInfoGroupBox);
            this.Controls.Add(RegionGroupBox);
            this.Controls.Add(TitleGroupBox);
            this.Controls.Add(VideoStandardGroupBox);
            this.MaximizeBox = false;
            this.Name = "AddDiscForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnAddDiscFormFormClosed);
            VideoStandardGroupBox.ResumeLayout(false);
            VideoStandardGroupBox.PerformLayout();
            TitleGroupBox.ResumeLayout(false);
            TitleGroupBox.PerformLayout();
            RegionGroupBox.ResumeLayout(false);
            RegionGroupBox.PerformLayout();
            PurchaseInfoGroupBox.ResumeLayout(false);
            PurchaseInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CountAsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionNumberUpDown)).EndInit();
            DiscsGrooupBox.ResumeLayout(false);
            DiscsGrooupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown CollectionNumberUpDown;
        private System.Windows.Forms.NumericUpDown CountAsUpDown;
        private System.Windows.Forms.CheckBox NoCollectionNumberCheckBox;
        private System.Windows.Forms.RadioButton VideoStandardPalRadioButton;
        private System.Windows.Forms.RadioButton VideoStandardNtscRadioButton;
        private System.Windows.Forms.CheckBox CreateDiscContentCheckBox;
        private System.Windows.Forms.TextBox SortTitleTextBox;
        private System.Windows.Forms.TextBox EditionTextBox;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.CheckBox Region6CheckBox;
        private System.Windows.Forms.CheckBox Region5CheckBox;
        private System.Windows.Forms.CheckBox Region4CheckBox;
        private System.Windows.Forms.CheckBox Region3CheckBox;
        private System.Windows.Forms.CheckBox Region2CheckBox;
        private System.Windows.Forms.CheckBox Region1CheckBox;
        private System.Windows.Forms.DateTimePicker PurchaseDatePicker;
    }
}