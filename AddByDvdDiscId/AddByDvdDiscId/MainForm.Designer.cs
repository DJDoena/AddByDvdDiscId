
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LocalityComboBox = new System.Windows.Forms.ComboBox();
            this.DriveComboBox = new System.Windows.Forms.ComboBox();
            this.RefreshDrivesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LocalityComboBox
            // 
            this.LocalityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LocalityComboBox.FormattingEnabled = true;
            this.LocalityComboBox.Location = new System.Drawing.Point(135, 111);
            this.LocalityComboBox.Name = "LocalityComboBox";
            this.LocalityComboBox.Size = new System.Drawing.Size(203, 21);
            this.LocalityComboBox.TabIndex = 0;
            // 
            // DriveComboBox
            // 
            this.DriveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DriveComboBox.FormattingEnabled = true;
            this.DriveComboBox.Location = new System.Drawing.Point(135, 63);
            this.DriveComboBox.Name = "DriveComboBox";
            this.DriveComboBox.Size = new System.Drawing.Size(203, 21);
            this.DriveComboBox.TabIndex = 1;
            // 
            // RefreshDrivesButton
            // 
            this.RefreshDrivesButton.Location = new System.Drawing.Point(344, 63);
            this.RefreshDrivesButton.Name = "RefreshDrivesButton";
            this.RefreshDrivesButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshDrivesButton.TabIndex = 2;
            this.RefreshDrivesButton.Text = "Refresh";
            this.RefreshDrivesButton.UseVisualStyleBackColor = true;
            this.RefreshDrivesButton.Click += new System.EventHandler(this.OnRefreshDrivesButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RefreshDrivesButton);
            this.Controls.Add(this.DriveComboBox);
            this.Controls.Add(this.LocalityComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Add by DVD Disc ID";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormFormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox LocalityComboBox;
        private System.Windows.Forms.ComboBox DriveComboBox;
        private System.Windows.Forms.Button RefreshDrivesButton;
    }
}