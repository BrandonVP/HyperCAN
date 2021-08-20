using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SERIAL_RX_TX;

namespace HyperCAN
{
    public partial class main : Form
    {
        private main _form;
        private SerialComPort serialPort;
        private Timer receivedDataTimer;
        private Timer receivedDataTimer2;
        private Timer receivedDataTimer3;
        private Timer receivedDataTimer4;
        private Timer receivedDataTimer5;
        private Timer receivedDataTimer6;
        private string receivedData;
        private bool dataReady = false;

        String SerialBaudRate = "500000";
        String PortCOM = Properties.Settings.Default.COMPort;
        internal SaveFileDialog SaveFileDialog1;
        

        public main()
        {
            InitializeComponent();
            serialPort = new SerialComPort();
            serialPort.RegisterReceiveCallback(ReceiveDataHandler);

            receivedDataTimer = new Timer();
            receivedDataTimer.Interval = 1;   // 25 ms
            receivedDataTimer.Tick += new EventHandler(ReceivedDataTimerTick);
            receivedDataTimer.Start();
            
            receivedDataTimer2 = new Timer();
            receivedDataTimer2.Interval = 1;   // 25 ms
            receivedDataTimer2.Tick += new EventHandler(ReceivedDataTimerTick);
            receivedDataTimer2.Start();
            
            receivedDataTimer3 = new Timer();
            receivedDataTimer3.Interval = 1;   // 25 ms
            receivedDataTimer3.Tick += new EventHandler(ReceivedDataTimerTick);
            receivedDataTimer3.Start();
            
            receivedDataTimer4 = new Timer();
            receivedDataTimer4.Interval = 1;   // 25 ms
            receivedDataTimer4.Tick += new EventHandler(ReceivedDataTimerTick);
            receivedDataTimer4.Start();

            receivedDataTimer5 = new Timer();
            receivedDataTimer5.Interval = 1;   // 25 ms
            receivedDataTimer5.Tick += new EventHandler(ReceivedDataTimerTick);
            receivedDataTimer5.Start();

            receivedDataTimer6 = new Timer();
            receivedDataTimer6.Interval = 500;   // 25 ms
            receivedDataTimer6.Tick += new EventHandler(bufferCount);
            receivedDataTimer6.Start();

            this.SaveFileDialog1 = new SaveFileDialog();
        }

        private void startSerial()
        {
            // Handles the Open/Close button, which toggles its label, depending on previous state.
            string status;
           
            status = serialPort.Open(PortCOM, SerialBaudRate, "8", "None", "One");
            if (!status.Contains("Opened") && !status.Contains("Closed"))
            {
                UpdateDataWindow(status);
            }
           
        }

        private void ReceiveDataHandler(string data)
        {
            if (dataReady)
            {
                Debug.Print("Received data was thrown away because line buffer not emptied");
            }
            else
            {
                dataReady = true;
                receivedData = data;
            }
        }

        private void saveCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }


        private void bufferCount(object sender, EventArgs e)
        {
            textBox1.Text = serialPort.getbufferSize().ToString();
        }

        private void ReceivedDataTimerTick(object sender, EventArgs e)
        {
            if (dataReady || serialPort.getbufferSize() > 1)
            {
                //UpdateDataWindow(receivedData);
                int temp = serialPort.popBuffer();
                if(temp != -1)
                {
                    UpdateDataWindow(serialPort.RXBuffer[serialPort.getbufferOutPtr()]);
                }
                dataReady = false;
            }
        }

        private void UpdateDataWindow(string message)
        {
            tbDataWindow.AppendText(message);
        }

        private void main_Load(object sender, EventArgs e)
        {

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
            tbDataWindow.Font = new Font("Courier New", fontsize);
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
            Properties.Settings.Default.COMPort = PortCOM;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void captureSearch_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.output_file_name = textBox2.Text;
            saveCapture();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                captureSearch_Click(sender, e);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/BrandonVP/HyperCAN");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 0.1", "Release");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startSerial();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort.Stop();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            tbDataWindow.Clear();
        }

        public void tbDataWindow_TextChanged(object sender, EventArgs e)
        {

        }

        public void saveCapture()
        {
            Console.WriteLine(Properties.Settings.Default.output_location);
            Console.WriteLine(Properties.Settings.Default.output_file_name);
            Console.WriteLine(Properties.Settings.Default.output_location + "\\" + Properties.Settings.Default.output_file_name);
            using (var stream = File.CreateText(Properties.Settings.Default.output_location + "\\" + Properties.Settings.Default.output_file_name))
            {
                stream.Write(this.tbDataWindow.Text);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
