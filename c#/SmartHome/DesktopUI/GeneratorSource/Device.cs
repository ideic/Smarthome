﻿using System.Globalization;

namespace DesktopUI.GeneratorSource
{
    public class Device
    {
        public Device(DeviceType deviceType, string name, int pinNumber)
        {
            DeviceType = deviceType.ToString();
            Name = name;
            PinNumber = pinNumber.ToString(CultureInfo.InvariantCulture);
        }

        public string PinNumber { get; set; }

        public string Name { get; private set; }

        public string DeviceType { get; private set; }
    }
}