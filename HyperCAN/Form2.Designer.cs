namespace HyperCAN
{
    partial class Form2
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
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fileOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.getFileOutput = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(67, 114);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 35);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Accept";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(175, 114);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 35);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // fileOutput
            // 
            this.fileOutput.Location = new System.Drawing.Point(75, 35);
            this.fileOutput.Name = "fileOutput";
            this.fileOutput.Size = new System.Drawing.Size(200, 20);
            this.fileOutput.TabIndex = 2;
            this.fileOutput.Text = global::HyperCAN.Properties.Settings.Default.output_location;
            this.fileOutput.TextChanged += new System.EventHandler(this.fileOutput_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Output:";
            // 
            // getFileOutput
            // 
            this.getFileOutput.Location = new System.Drawing.Point(282, 35);
            this.getFileOutput.Name = "getFileOutput";
            this.getFileOutput.Size = new System.Drawing.Size(30, 20);
            this.getFileOutput.TabIndex = 6;
            this.getFileOutput.Text = "...";
            this.getFileOutput.UseVisualStyleBackColor = true;
            this.getFileOutput.Click += new System.EventHandler(this.getFileOutput_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 161);
            this.Controls.Add(this.getFileOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileOutput);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Name = "Form2";
            this.Text = "Save Capture";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox fileOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getFileOutput;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}