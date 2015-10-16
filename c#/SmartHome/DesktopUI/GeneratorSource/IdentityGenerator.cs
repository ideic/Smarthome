using System;

namespace DesktopUI.GeneratorSource
{
    internal class IdentityGenerator
    {
        private int _address = 2;
        private int _sequentialId = 0;
        private byte _abc = (byte)'A';

        public string NextAddress()
        {
            _address += 2;

            return "B" + Convert.ToString(_address, 2).PadLeft(8,'0');
        }

        public string NextSegmentName()
        {
            _abc++;
            return "Segment" + (char)_abc;
        }

        public string NextSequentialId()
        {
            _sequentialId++;
            return _sequentialId.ToString();
        }
    }
}