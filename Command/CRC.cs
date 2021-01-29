using System;

namespace Sociedade_Serial
{
    public enum algorithmTypes
    {
        none,
        checksum,
        crc
    }
    
    public class CRC
    {
 
        private algorithmTypes algorithm;
        private UInt64 polynominal, initialValue, finalXorVal;
        private bool inputReflected, resultReflected, lowBitFirst;
        private int from, width;

        /// <summary>
        /// Creates the object
        /// </summary>
        /// <param name="algorithm">CRC/Checksum/None</param>
        /// <param name="polynominal">CRC polynominal</param>
        /// <param name="initialValue">CRC initial value</param>
        /// <param name="finalXorVal">CRC initial value</param>
        /// <param name="inputReflected">CRC parameter</param>
        /// <param name="resultReflected">CRC parameter</param>
        /// <param name="lowBitFirst">If the lower bit comes first in the frame</param>
        /// <param name="from">Offset of ignored bytes</param>
        /// <param name="width">8/16/32/64 bits</param>
        public CRC(algorithmTypes algorithm, ulong polynominal , ulong initialValue, ulong finalXorVal, bool inputReflected, bool resultReflected, bool lowBitFirst, int from, int width)
        {
            this.algorithm = algorithm;
            this.polynominal = polynominal;
            this.initialValue = initialValue;
            this.finalXorVal = finalXorVal;
            this.inputReflected = inputReflected;
            this.resultReflected = resultReflected;
            this.lowBitFirst = lowBitFirst;
            this.from = from;
            this.width = width;
        }

        /// <summary>
        /// Calculates the crc of the frame "f"
        /// </summary>
        /// <param name="f">Frame</param>
        /// <returns>CRC value</returns>
        public byte[] Calc(byte[] f)
        {
            if (this.algorithm == algorithmTypes.none)
            {
                return null;
            }
            else
            {
                byte[] frame = new byte[f.Length - this.from];
                
                f.CopyTo(frame, this.from);

                UInt64 crcValue = this.initialValue;
                UInt64 c;
                UInt64 msbMask = (UInt64)(0x1 << (this.width - 1));
                UInt64 crcMask;

                switch (this.width)
                {
                    case 8: crcMask = 0xFF; break;
                    case 16: crcMask = 0xFFFF; break;
                    case 32: crcMask = 0xFFFFFFFF; break;
                    default: crcMask = UInt64.MaxValue; break;
                }

                foreach (byte b in frame)
                {
                    switch (this.algorithm)
                    {
                        case algorithmTypes.checksum:
                            crcValue += (UInt64)b;
                            break;
                        case algorithmTypes.crc:
                            if (!this.inputReflected)
                                c = b;
                            else
                                c = Reflect(b);

                            crcValue ^= (c << (this.width - 8));
                            crcValue &= crcMask;
                            for (int i = 0; i < 8; i++)
                            {
                                if ((crcValue & msbMask) != 0)
                                {
                                    crcValue <<= 1;
                                    crcValue ^= this.polynominal;
                                }
                                else
                                {
                                    crcValue <<= 1;
                                }
                                crcValue &= crcMask;

                            }
                            break;
                    }
                }


                if (this.resultReflected)
                {
                    crcValue = Reflect(crcValue);
                    crcValue &= crcMask;
                }

                crcValue ^= this.finalXorVal;

                crcValue &= crcMask;
                int n = (this.width / 8) - 1;
                byte[] finalValue = new byte[n];
                if (this.lowBitFirst)
                {
                    for(int i =0; i<= n; i++)
                    {
                        finalValue[n - i] = (byte)(crcValue & ((UInt64)0xFF<<(i * 8)));
                    }
                }
                else
                {
                    for (int i = 0; i < finalValue.Length; i++)
                    {
                        finalValue[i] = (byte)(crcValue & ((UInt64)0xFF << (i * 8)));
                    }
                }
                return finalValue;
            }
            


        }
       
        /// <summary>
        /// reflect the bits
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private UInt64 Reflect(UInt64 val)
        {
            UInt64 resByte = 0;
            for (int i = 0; i < this.width; i++)
            {
                if ((val & (UInt64)(1 << i)) != 0)
                    resByte |= (UInt64)(1 << (this.width - 1 - i));
            }
            return resByte;
        }
    }
}