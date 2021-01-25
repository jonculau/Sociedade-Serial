using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Sociedade_Serial.Comunication
{
    class Command
    {
        public enum evalulation 
        {
            approved,
            reproved,
            manual
        }

        Frame sendFrame, receivedFrame, expectedAnswer;
        evalulation eval;
        Command Next;
        List<FLAG> flags;

        
        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="sendFrame">Command Frame</param>
        /// <param name="expectedAnswer">Expected answer from the instrument</param>
        public Command(Frame sendFrame, Frame expectedAnswer)
        {
            this.sendFrame = sendFrame;
            
            this.expectedAnswer = new Frame(expectedAnswer.Raw,
                                            "",                 // The script is executed with the real Answer
                                            expectedAnswer.Crc);
            
            this.receivedFrame = new Frame("",
                                           expectedAnswer.Script,
                                           expectedAnswer.Crc);
        }

        /// <summary>
        /// Send the command to the instrument under test
        /// </summary>
        /// <param name="fs">Dynamics flags list</param>
        /// <param name="port">Serial port connected with the instrument under test</param>
        public void Send(List<FLAG> fs, SerialPort port)
        {
            this.flags.Clear();
            this.flags = fs;
            byte[] sendArray = this.sendFrame.ProcessesFrame(fs);
            port.Write(sendArray, 0, sendArray.Length);            
        }

        /// <summary>
        /// Evaluates the instrument answer
        /// </summary>
        /// <param name="rawAnswer">Instrument answer</param>
        /// <returns></returns>
        public evalulation evalCommand(string rawAnswer)
        {
            if (this.expectedAnswer.Raw != "")
            {
                this.receivedFrame.Raw = rawAnswer;
 
                if (this.expectedAnswer.ProcessesFrame(this.flags).Equals
                   (this.receivedFrame.ProcessesFrame()))
                {
                    return evalulation.approved;
                }else
                {
                    return evalulation.reproved;
                }

            }
            else
            {
                return evalulation.manual;
            }
        }
    }
}
