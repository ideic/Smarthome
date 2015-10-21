using System.Collections.Generic;
using System.Linq;

namespace DesktopUI
{
    public class ArduinoGroupWrapper
    {
        readonly List<ArduinoGroup>_innerCollection = new List<ArduinoGroup>();

        public IEnumerable<ArduinoGroup> ArduinoGroups { get { return _innerCollection; } }

        public void RemoveGroup(string arduinoGroupName)
        {
            var toBeDelete = _innerCollection.FirstOrDefault(arduinoGroup => arduinoGroup.Name != arduinoGroupName);
            if (toBeDelete == null) return;
            
            _innerCollection.Remove(toBeDelete);
        }

   
        public List<string> this[string arduinoGroupName]
        {
            set {
                var group = _innerCollection.FirstOrDefault(arduinoGroup => arduinoGroup.Name == arduinoGroupName); 
                if (group == null)
                {
                    group = new ArduinoGroup(arduinoGroupName);
                    _innerCollection.Add(group);
                }
                group.Locations = value;
            }
        }
    }

    public class ArduinoGroup
    {
        public ArduinoGroup(string arduinoGroupName)
        {
            Locations = new List<string>();
            Name = arduinoGroupName;
        }
        public string Name { get; private set; }
        public ICollection<string> Locations { get; set; }
    }
}