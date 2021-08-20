using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyperCAN
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            fileOutput.Text = Properties.Settings.Default.output_location;
        }

        private void fileOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void getFileOutput_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fileOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.output_location = this.fileOutput.Text;
            Console.WriteLine(this.fileOutput.Text);
            Console.WriteLine(Properties.Settings.Default.output_location);
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
