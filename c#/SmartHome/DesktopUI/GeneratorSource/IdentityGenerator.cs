using System;
using System.Globalization;

namespace DesktopUI.GeneratorSource
{
    internal class IdentityGenerator
    {
        private int _address;
        private int _sequentialId;
        private byte _abc = (byte)'A';

        public string NextAddress()
        {
            _address += 2;

            return "B" + Convert.ToString(_address, 2).PadLeft(8,'0');
        }

        public string NextSegmentName()
        {
            var result = "Segment" + (char)_abc;
            _abc++;
            return result;
        }

        public string NextSequentialId()
        {
            var result = _sequentialId.ToString(CultureInfo.InvariantCulture); 
            _sequentialId++;
            return result;
        }
    }
}