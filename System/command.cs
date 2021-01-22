using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace Sociedade_Serial
{


    
    public struct Check
    {
        UInt16 size;
        checkAlgorithm algorithm;
        Byte offset;
        UInt64 poly;
        UInt64 initialValue;
        UInt64 finalXOR;
    }

    public struct FLAG {
        string name;
        Boolean send;
        UInt16 offset;
        List <UInt16> newValue;
    }


    public enum checkAlgorithm
    {
        none,
        checksum,
        crc
    }
    class command
    {
        private string Name;
        private List<UInt16> sendCommand;
        private List<UInt16> expectAnswer;
        private List<FLAG> SendFlags;
        private List<FLAG> ReceiveFlags;


        public Check check;
        public command(string Name, string sendCommand, string expectAnswer, Check check)
        {
            this.Name = Name;
            this.check = check;
          
        }
        public List<UInt16> Send()
        {
            List<UInt16> send;
            
            
            return send;
           
        }
    }
}
