using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace HyperCAN
{
    public partial class main : Form
    {
        int SerialBaudRate = 500000;
        String PortCOM = "COM10";
        internal SaveFileDialog SaveFileDialog1;
        public event EventHandler SelectionChangeCommitted;

        public main()
        {
            InitializeComponent();
            startSerial();
            this.SaveFileDialog1 = new SaveFileDialog();
        }

        private void startSerial()
        {
            try
            {
                serialPort1.BaudRate = SerialBaudRate;
                serialPort1.PortName = PortCOM;
                serialPort1.Open();
                serialPort1.DataReceived += serialPort1_DataReceived;
            }
            catch(Exception f)
            {
                MessageBox.Show("Unable to open " + PortCOM, "Error");
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = serialPort1.ReadLine();
                richCANBox.Invoke(new MethodInvoker(delegate { richCANBox.AppendText(line + "\n"); }));
            }
            catch(Exception f)
            {

            }
        }
        
        private void mainBody_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CANBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.WriteLine("0");
            }
            catch(Exception f)
            {

            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.WriteLine("1");
            }
            catch (Exception f)
            {

            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.WriteLine("2");
                richCANBox.Clear();
            }
            catch (Exception f)
            {

            }
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        // Declare a new memory stream.
        MemoryStream userInput = new MemoryStream();

        //saveFileDialog1.Filter = "Text File|*.txt|PCAN|*.TRC";
        //System.IO.File.WriteAllLines(@"C:\Users\bvanpelt\source\repos\HyperCAN\HyperCAN\a.txt", richCANBox.Lines);
        private void saveCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richCANBox.SaveFile(userInput, RichTextBoxStreamType.PlainText);
            userInput.WriteByte(13);

            // Display the entire contents of the stream,
            // by setting its position to 0, to RichTextBox2.
            userInput.Position = 0;
            richCANBox.LoadFile(userInput, RichTextBoxStreamType.PlainText);

            // Set the properties on SaveFileDialog1 so the user is 
            // prompted to create the file if it doesn't exist 
            // or overwrite the file if it does exist.
            //SaveFileDialog1.CreatePrompt = true;
            SaveFileDialog1.OverwritePrompt = true;

            // Set the file name to myText.txt, set the type filter
            // to text files, and set the initial directory to the 
            // MyDocuments folder.
            SaveFileDialog1.FileName = "Capture";
            // DefaultExt is only used when "All files" is selected from 
            // the filter box and no extension is specified by the user.
            SaveFileDialog1.DefaultExt = "txt";
            SaveFileDialog1.Filter =
                "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            SaveFileDialog1.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Call ShowDialog and check for a return value of DialogResult.OK,
            // which indicates that the file was saved. 
            DialogResult result = SaveFileDialog1.ShowDialog();
            Stream fileStream;

            if (result == DialogResult.OK)
            {
                // Open the file, copy the contents of memoryStream to fileStream,
                // and close fileStream. Set the memoryStream.Position value to 0 
                // to copy the entire stream. 
                fileStream = SaveFileDialog1.OpenFile();
                userInput.Position = 0;
                userInput.WriteTo(fileStream);
                fileStream.Close();
            }
        }

        private void richCANBox_TextChanged(object sender, EventArgs e)
        {
            richCANBox.ScrollToCaret();
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.fontSizeToolStripMenuItem.SelectedIndexChanged +=
            new System.EventHandler(fontSizeToolStripMenuItem_SelectedIndexChanged);
        }
        
        private void fontSizeToolStripMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fontsize = 12;
            fontsize = int.Parse(fontSizeToolStripMenuItem.SelectedItem.ToString());
            richCANBox.Font = new Font("Courier New", fontsize);
        }

        protected void readPortNames()
        {
            // Clear current list to avoid duplication
            this.toolStripComboBoxCOM.Items.Clear();

            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            // Display each port name to the console.
            foreach (string port in ports)
            {
                this.toolStripComboBoxCOM.Items.Add(port);
            }
        }

        private void cOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readPortNames();
        }

        private void toolStripComboBoxCOM_Click(object sender, EventArgs e)
        {
            this.toolStripComboBoxCOM.SelectedIndexChanged +=
            new System.EventHandler(toolStripComboBoxCOM_SelectedIndexChanged);
        }

        private void toolStripComboBoxCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            PortCOM = toolStripComboBoxCOM.SelectedItem.ToString();
            serialPort1.Close();
            startSerial();
        }
    }
}
