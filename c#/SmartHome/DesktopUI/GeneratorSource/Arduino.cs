using System.Collections.Generic;

namespace DesktopUI.GeneratorSource
{
    public class Arduino
    {
        public Arduino(string name)
        {
            Name = name;
            Devices = new List<Device>();
        }

        public string DevicesNumber 
        { get { return Devices.Count.ToString(); }}

        public ICollection<Device> Devices { get; private set; }

        public string Name { get; set ; }

        public void AddDevice(Device device)
        {
            Devices.Add(device);
        }
    }
}