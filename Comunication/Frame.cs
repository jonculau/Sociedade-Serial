using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sociedade_Serial.Comunication
{

    public class Frame
    {
       
        private string raw;
        private string script;
        private bool hasScript;
        List<FLAG> flags;
        private CRC crc;
        /// <summary>
        /// Creates the object
        /// </summary>
        /// <param name="raw">Raw frame value</param>
        /// <param name="script">JS script</param>
        /// <param name="crc">CRC object</param>
        /// <param name="fs">Optional List of Flags</param>
        public Frame(string raw, string script, CRC crc, List<FLAG> fs = null)
        {
            this.raw = raw;
            this.script = script;
            this.crc = crc;
            hasScript = script != "" ?true:false;
            if (fs.Count > 0)
            {
                flags.Clear();
                flags = fs;
            }
        }

        public string Script { get => this.script;}
        public CRC Crc { get => crc; set => crc = value; }
        public string Processed { get => Hex2String(ProcessesFrame());}
        public string Raw { get => raw; set => raw = value; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs">Dynamics Flags List</param>
        /// <returns>Processed array of bytes, it calculates the frame CRC and 
        /// set replace the flags by values selectes by the user</returns>
        public byte[] ProcessesFrame(List<FLAG> fs = null)
        {
            if (fs.Count > 0)
            {
                this.flags.Clear();
                this.flags = fs;
            }            
            List<byte> pBytes = new List<byte>(); // processed bytes
            string processed = this.raw.Replace(" ", "");

            foreach(FLAG f in fs)
            {
                while (processed.Contains($"#{f.name}#"))
                {
                    processed = processed.Replace($"#{f.name}#", f.value);
                }
            }
            if (this.hasScript)
            {
                // run script
            }
            else
            {
                pBytes.AddRange(String2Hex(processed)); // Converts string to bytes
                pBytes.AddRange(this.crc.Calc(pBytes.ToArray())); // Calc crc
            }

            return pBytes.ToArray();

        }
        /// <summary>
        /// Convert an array of bytes to a hex string
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>A string with the hex representation of bytes</returns>
        /// <seealso cref="String2Hex(string)"/>
        private string Hex2String(byte[] bytes)
        {
            string str = "";
            foreach(byte b in bytes)
            {
                str += $"{b.ToString("x2").ToUpper()} ";
            }
            return str.Trim();
        }

        /// <summary>
        /// Converts a string with hex values to an array of bytes
        /// </summary>
        /// <param name="str">string with hex values</param>
        /// <returns>array of bytes</returns>
        /// <seealso cref="Hex2String(byte[])"/>
        private byte[] String2Hex(string str)
        {
            
            List<byte> Hex = new List<byte>();



            for (int i = 0; i < str.Length; i += 2)
            {
                int x = byte.Parse(str[i].ToString(), NumberStyles.HexNumber) << 4; // High Nibble
                x += byte.Parse(str[i].ToString(), NumberStyles.HexNumber); // Low Nibble
                Hex.Add((byte)x);
            }
            return Hex.ToArray();
        }
    }

    public struct FLAG
    {
        public string name;
        public string value;
    }

}


}
