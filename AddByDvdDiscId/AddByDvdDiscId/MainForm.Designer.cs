
namespace DoenaSoft.DVDProfiler.AddByDvdDiscId
{
    partial class MainForm
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
            System.Windows.Forms.Button RefreshDrivesButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.Button AddDiscButton;
            System.Windows.Forms.Label DriveLabel;
            System.Windows.Forms.Label LocalityLabel;
            System.Windows.Forms.Button AbortButton;
            this.LocalityComboBox = new System.Windows.Forms.ComboBox();
            this.DriveComboBox = new System.Windows.Forms.ComboBox();
            this.UpdateFromOnlineDatabaseCheckBox = new System.Windows.Forms.CheckBox();
            this.AddAsChildCheckBox = new System.Windows.Forms.CheckBox();
            RefreshDrivesButton = new System.Windows.Forms.Button();
            AddDiscButton = new System.Windows.Forms.Button();
            DriveLabel = new System.Windows.Forms.Label();
            LocalityLabel = new System.Windows.Forms.Label();
            AbortButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RefreshDrivesButton
            // 
            resources.ApplyResources(RefreshDrivesButton, "RefreshDrivesButton");
            RefreshDrivesButton.Name = "RefreshDrivesButton";
            RefreshDrivesButton.UseVisualStyleBackColor = true;
            RefreshDrivesButton.Click += new System.EventHandler(this.OnRefreshDrivesButtonClick);
            // 
            // AddDiscButton
            // 
            resources.ApplyResources(AddDiscButton, "AddDiscButton");
            AddDiscButton.Name = "AddDiscButton";
            AddDiscButton.UseVisualStyleBackColor = true;
            AddDiscButton.Click += new System.EventHandler(this.OnAddDiscButtonClick);
            // 
            // DriveLabel
            // 
            resources.ApplyResources(DriveLabel, "DriveLabel");
            DriveLabel.Name = "DriveLabel";
            // 
            // LocalityLabel
            // 
            resources.ApplyResources(LocalityLabel, "LocalityLabel");
            LocalityLabel.Name = "LocalityLabel";
            // 
            // AbortButton
            // 
            AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(AbortButton, "AbortButton");
            AbortButton.Name = "AbortButton";
            AbortButton.UseVisualStyleBackColor = true;
            AbortButton.Click += new System.EventHandler(this.OnAbortButtonClick);
            // 
            // LocalityComboBox
            // 
            this.LocalityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LocalityComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.LocalityComboBox, "LocalityComboBox");
            this.LocalityComboBox.Name = "LocalityComboBox";
            // 
            // DriveComboBox
            // 
            this.DriveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DriveComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.DriveComboBox, "DriveComboBox");
            this.DriveComboBox.Name = "DriveComboBox";
            // 
            // UpdateFromOnlineDatabaseCheckBox
            // 
            resources.ApplyResources(this.UpdateFromOnlineDatabaseCheckBox, "UpdateFromOnlineDatabaseCheckBox");
            this.UpdateFromOnlineDatabaseCheckBox.Checked = true;
            this.UpdateFromOnlineDatabaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UpdateFromOnlineDatabaseCheckBox.Name = "UpdateFromOnlineDatabaseCheckBox";
            this.UpdateFromOnlineDatabaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAsChildCheckBox
            // 
            resources.ApplyResources(this.AddAsChildCheckBox, "AddAsChildCheckBox");
            this.AddAsChildCheckBox.Checked = true;
            this.AddAsChildCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AddAsChildCheckBox.Name = "AddAsChildCheckBox";
            this.AddAsChildCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = AddDiscButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = AbortButton;
            this.Controls.Add(this.AddAsChildCheckBox);
            this.Controls.Add(AbortButton);
            this.Controls.Add(LocalityLabel);
            this.Controls.Add(DriveLabel);
            this.Controls.Add(this.UpdateFromOnlineDatabaseCheckBox);
            this.Controls.Add(AddDiscButton);
            this.Controls.Add(RefreshDrivesButton);
            this.Controls.Add(this.DriveComboBox);
            this.Controls.Add(this.LocalityComboBox);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormFormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox LocalityComboBox;
        private System.Windows.Forms.ComboBox DriveComboBox;
        private System.Windows.Forms.CheckBox UpdateFromOnlineDatabaseCheckBox;
        private System.Windows.Forms.CheckBox AddAsChildCheckBox;
    }
}