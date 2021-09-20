// 
// Serial COM Port receive message event handler
// 12/16/2017, Dale Gambill
// When a line of text arrives from the COM port terminated by a \n character, this module will pass the message to
// the function specified by the application.   The application can also send a line of text.
//
// IMPORTANT: The dot net function below, comPort.ReadLine(), will not throw an error if there is no data, but might throw 'System.TimeoutException', if the data
// is not lines of text terminated by /n.  This would be because ReadLine() cannot find a line terminator in the wrong type of data.
// This code is intended for use with lines of text only.  It is not intended for use with any other type of data.
//

// Modified from: https://github.com/dalegambill/CsharpTerminal
// 8/20/2021, Brandon Van Pelt
// Added circular buffer

/*
using System;
using System.IO.Ports;
using System.Diagnostics;

namespace SERIAL_RX_TX
{
   

    public class SerialComPort
    {
        public int getbufferSize()
        {
            return myStack.stack_size();
        }
        public String getFRAME()
        {
            return myStack.pop();
        }
        public void Stop()
        {
            myStack.reset();
        }

        // constructor
        public SerialComPort()
        {
            
        }

        ~SerialComPort()
        {
            Close();
        }


        // User must register function to call when a line of text terminated by \n has been received
        
    }
}
*/