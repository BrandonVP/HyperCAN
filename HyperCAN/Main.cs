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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO: Index error when using stop

namespace HyperCAN
{
    public partial class main : Form
    {
        // Stack for buffer
        Stack myStack;

        SerialPort comPort;

        String SerialBaudRate = "500000";
        String PortCOM = Properties.Settings.Default.COMPort;
        internal SaveFileDialog SaveFileDialog1;
        private System.Windows.Forms.Timer receivedDataTimer6;

        public main()
        {
            InitializeComponent();

            comPort = new SerialPort();
            myStack = new Stack(2048);

            receivedDataTimer6 = new System.Windows.Forms.Timer();
            receivedDataTimer6.Interval = 500;   
            receivedDataTimer6.Tick += new EventHandler(bufferCount);
            receivedDataTimer6.Start();
            
            this.SaveFileDialog1 = new SaveFileDialog();

            RegisterReceiveCallback(ReceiveDataHandler);
            
            // Thread to poll for incomming messages stored in the buffer
            //ThreadPool.QueueUserWorkItem(new WaitCallback(printFRAMES));
        }

        // https://stackoverflow.com/questions/519233/writing-to-a-textbox-from-another-thread
        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            tbDataWindow.Text += value;

            // https://stackoverflow.com/questions/1228675/scroll-to-bottom-of-c-sharp-textbox
            tbDataWindow.SelectionStart = tbDataWindow.Text.Length;
            tbDataWindow.ScrollToCaret();
        }

        // Poll messages stored in the buffer
        public void printFRAMES(object obj)
        {
            while (true)
            {
                try
                {
                    if (myStack.stack_size() > 0 && comPort.IsOpen)
                    {
                        AppendTextBox(myStack.pop());
                        //tbDataWindow.Invoke(new MethodInvoker(delegate { tbDataWindow.AppendText(myStack.pop()); }));
                    }
                    else if (!comPort.IsOpen)
                    {
                        // Clear messages in buffer when serial port is closed
                        myStack.reset();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }

        // Open serial COM port
        private void startSerial()
        {
            // Handles the Open/Close button, which toggles its label, depending on previous state.
            string status;
           
            status = Open(PortCOM, SerialBaudRate, "8", "None", "One");
            if (!status.Contains("Opened") && !status.Contains("Closed"))
            {
                Console.WriteLine(status);
            }
        }

        public void ReceiveDataHandler(string data)
        {
   
        }

        private void saveCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void bufferCount(object sender, EventArgs e)
        {
            textBox1.Text = myStack.stack_size().ToString();
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
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                Properties.Settings.Default.output_file_name = textBox2.Text;
                saveCapture();
            }
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
            MessageBox.Show("Version 1.0", "Release");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startSerial();

        //https://stackoverflow.com/questions/23340894/polling-the-right-way
            int delay = 1;
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var listener = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    // poll hardware
                    try
                    {
                        if (myStack.stack_size() > 0 && comPort.IsOpen)
                        {
                            AppendTextBox(myStack.pop());
                        }
                        else if (!comPort.IsOpen)
                        {
                            // Clear messages in buffer when serial port is closed
                            myStack.reset();
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.Message);
                    }

                    Thread.Sleep(delay);
                    if (token.IsCancellationRequested)
                        break;
                }

                // cleanup, e.g. close connection
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            ClosePort();
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

        public delegate void ReceiveCallback(string receivedMessage);
        public event ReceiveCallback onMessageReceived = null;
        public void RegisterReceiveCallback(ReceiveCallback FunctionToCall)
        {
            onMessageReceived += FunctionToCall;
        }
        public void DeRegisterReceiveCallback(ReceiveCallback FunctionToCall)
        {
            onMessageReceived -= FunctionToCall;
        }

        public void SendLine(string aString)
        {
            try
            {
                if (comPort.IsOpen)
                {
                    comPort.Write(aString);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        public string Open(string portName, string baudRate, string dataBits, string parity, string stopBits)
        {
            try
            {
                comPort.WriteBufferSize = 4096;
                comPort.ReadBufferSize = 4096;
                comPort.WriteTimeout = 500;
                comPort.ReadTimeout = 500;
                comPort.DtrEnable = true;
                comPort.Handshake = Handshake.None;
                comPort.PortName = portName.TrimEnd();
                comPort.BaudRate = Convert.ToInt32(baudRate);
                comPort.DataBits = Convert.ToInt32(dataBits);
                switch (parity)
                {
                    case "None":
                        comPort.Parity = Parity.None;
                        break;
                    case "Even":
                        comPort.Parity = Parity.Even;
                        break;
                    case "Odd":
                        comPort.Parity = Parity.Odd;
                        break;
                }
                switch (stopBits)
                {
                    case "One":
                        comPort.StopBits = StopBits.One;
                        break;
                    case "Two":
                        comPort.StopBits = StopBits.Two;
                        break;
                }
                comPort.Open();
                comPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception error)
            {
                return error.Message + "\r\n";
            }
            if (comPort.IsOpen)
            {
                return string.Format("{0} Opened \r\n", comPort.PortName);
            }
            else
            {
                return string.Format("{0} Open Failed \r\n", comPort.PortName);
            }
        }

        public string ClosePort()
        {
            try
            {
                comPort.Close();
            }
            catch (Exception error)
            {
                return error.Message + "\r\n";
            }
            return string.Format("{0} Closed\r\n", comPort.PortName);
        }

        public bool IsOpen()
        {
            return comPort.IsOpen;
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (!comPort.IsOpen)
            {
                return;
            }
            string indata = string.Empty;
            try
            {
                indata = comPort.ReadLine();
                indata += "\n";
                if (onMessageReceived != null)
                {
                    myStack.push(indata);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void BaudStripComboBox1_Click(object sender, EventArgs e)
        {

        }
    }
    public class Stack
    {
        private int array_size;
        private int bufferOutPtr;
        private int bufferInPtr;
        private int MessagesInBuffer;
        private String[] CAN_Bus_Stack;

        public Stack(int size)
        {
            array_size = size;
            CAN_Bus_Stack = new string[array_size];
            bufferOutPtr = 0;
            bufferInPtr = 0;
            MessagesInBuffer = 0;
        }

        public bool push(String addMe)
        {
            CAN_Bus_Stack[bufferInPtr] = addMe;

            bufferInPtr++;
            // End of circular buffer 
            if (bufferInPtr > array_size - 1)
            {
                bufferInPtr = 0;
            }
            // Overflow case
            if (bufferInPtr == bufferOutPtr)
            {
                bufferOutPtr++;
                if (bufferOutPtr > array_size - 1)
                {
                    bufferOutPtr = 0;
                }
                // Let user know an overwrite occurred
                return false;
            }
            else
            {
                MessagesInBuffer++;
            }
            return true;
        }

        public String pop()
        {
            int temp = bufferOutPtr;

            bufferOutPtr++;
            MessagesInBuffer--;

            // End of circular buffer 
            if (bufferOutPtr > array_size - 1)
            {
                bufferOutPtr = 0;
            }

            MessagesInBuffer--;

            return CAN_Bus_Stack[temp];
        }

        public int stack_size()
        {
            return MessagesInBuffer;
        }

        public String peek()
        {
            return CAN_Bus_Stack[bufferOutPtr];
        }

        // Clears out all messages currently in the buffer
        public void reset()
        {
            bufferOutPtr = 0;
            bufferInPtr = 0;
            MessagesInBuffer = 0;
        }
    }
}
