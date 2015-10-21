using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DesktopUI.Annotations;
using DesktopUI.BuildBlocks;
using DesktopUI.GeneratorSource;
using DesktopUI.Graph;

namespace DesktopUI
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private MyGraph<Location> _graph;
        private Dictionary<string, List<string>> _arduinoGroup = new Dictionary<string, List<string>>();

        public MainViewModel()
        {
            _graph = new MyGraph<Location>();

                  //_graph.AddVertex(new Location("Előszoba"));
            //_graph.AddVertex(new Location("Gépészet"));
           /* var subGraph = new MySubGraph<Location>(){Label = "Eloszoba"};
            _graph.AddSubGraph(subGraph);

            subGraph.AddVertex(new Switch("EK1", Generated.False));
            subGraph.AddVertex(new Light("LEK1", Generated.False));
           
            _graph.AddEdge(new MyEdge<Location>(subGraph.Vertices.First(), subGraph.Vertices.Last(), new Arrow()));*/
            /*
            var subGraph2 = new MySubGraph<Location>(){Label = "Gepeszet"};
            _graph.AddSubGraph(subGraph2);
            subGraph2.AddVertex(new Switch("GK1", "GK1"));
            */
          /*  _arduinoGroup.Add("eloter", new List<string>(){"asdas", "masodik"});
            _arduinoGroup.Add("eloter2", new List<string>() { "asdas", "masodik" });
            _arduinoGroup.Add("eloter3", new List<string>());*/
        }

        public MyGraph<Location> Graph
        {
            get { return _graph; }
        }

        public string NewLocationName { get; set; }

        public string NewSwitchName { get; set; }

        public string NewLightName { get; set; }

        public string AssignSwitchToLocationLocation { get; set; }

        public string AssignSwitchToLocationSwitch { get; set; }

        public string AssignSwitchToLightSwitch { get; set; }

        public string AssignSwitchToLightLight { get; set; }

        public string AssignLight2LocationLocation { get; set; }
        public string AssignLight2LocationLight { get; set; }

        public string ArduinoGroupName { get; set; }


        public IEnumerable<string> LocationNames
        {
            get { return _graph.SubGraphs.Select(subGraph => subGraph.Label); }
        }

        public IEnumerable<string> SwitchNames
        {
            get
            {
                var result = new List<string>();
                
                result.AddRange(GetBuildBlocks<Switch>().Select(vertex => vertex.Name));
                return result;
            }
        }
        
        public IEnumerable<string> LightNames
        {
            get 
            {
                var result = new List<string>();

                result.AddRange(GetBuildBlocks<Light>().Select(light => light.Name));
                return result;
            }
        }

        private IEnumerable<T> GetBuildBlocks<T>() where T : Location
        {
            var result = new List<T>();
            foreach (var subgraph in _graph.SubGraphs)
            {
                result.AddRange(subgraph.Vertices.OfType<T>());
            }

            result.AddRange(_graph.Vertices.OfType<T>());
            return result;
        }

        public IEnumerable<string> ArduinoGroupNames
        {
            get { return _arduinoGroup.Keys.OrderBy(key => key); }
        }

        public Dictionary<string, List<string>> ArduinoGroups
        {
            get { return _arduinoGroup; }
            set { _arduinoGroup = value; }
        }

        public string AssignLocationToArduinoGroupsLocation { get; set; }

        public string AssignLocationToArduinoGroupsGroup { get; set; }

        public void CreateNewLocation()
        {
            if (_graph.SubGraphs.Any(subgraph => subgraph.Label == NewLocationName)) return;

            var subGraph = new MySubGraph<Location>{Label = NewLocationName};
            subGraph.AddVertex(new Location("?", Generated.True));

            _graph.AddSubGraph(subGraph);

            OnPropertyChanged("LocationNames");
 
        }

        public void DeleteLocation()
        {
            var location= _graph.SubGraphs.FirstOrDefault(subgraph => subgraph.Label == NewLocationName);

            if (location == null) return;
            
            foreach (var edge in location.Vertices.Select(vertex1 => _graph.Edges.Cast<MyEdge<Location>>().Where(
                edge => edge.Source.Name == vertex1.Name || edge.Destination.Name == vertex1.Name).ToList()).SelectMany(edges => edges))
            {
                _graph.RemoveEdge(edge);
            }

            while (location.Vertices.Any())
            {
                location.RemoveVertex(location.Vertices.First());
            }

            _graph.RemoveSubGraph(location);
        }
 
        public void CreateNewSwitch()
        {
            if (_graph.Vertices.Any(vertex => vertex.Name == NewSwitchName)) return;

            _graph.AddVertex(new Switch(NewSwitchName, Generated.False));
            OnPropertyChanged("SwitchNames");
        }

        public void DeleteSwitch()
        {
            DeleteBuildBlock<Switch>(NewSwitchName);
        }

        public void CreateNewLight()
        {
            if (_graph.Vertices.Any(vertex => vertex.Name == NewLightName)) return;

            _graph.AddVertex(new Light(NewLightName, Generated.False));
            OnPropertyChanged("LightNames");
        }

        public void DeleteLight()
        {
            DeleteBuildBlock<Light>(NewLightName);
        }

        private void DeleteBuildBlock<T>(string buildBlockName) where T: Location
        {
            var location = GetBuildBlocks<T>().FirstOrDefault(light => light.Name == buildBlockName);

            if (location == null) return;

            var edges = _graph.Edges.Cast<MyEdge<Location>>()
                              .Where(edge => edge.Source.Name == location.Name
                                             || edge.Destination.Name == location.Name).ToList();

            foreach (var edge in edges)
            {
                _graph.RemoveEdge(edge);
            }


            var subGraph =
                _graph.SubGraphs.FirstOrDefault(subgraph => subgraph.Vertices.Any(vertex => vertex.Name == location.Name));

            if (subGraph != null)
            {
                subGraph.RemoveVertex(location);
            }

            if (_graph.Vertices.Any(vertex => vertex.Name == location.Name))
            {
                _graph.RemoveVertex(location);
            }

            _graph.Refresh();
        }

        public void AssignSwitchToLocation()
        {
            var locationSubGraph = _graph.SubGraphs.First(graph => graph.Label == AssignSwitchToLocationLocation);

            if (locationSubGraph.Vertices.Any(vertex => vertex.Name == AssignSwitchToLocationSwitch)) return;

            locationSubGraph.AddVertex(new Switch(AssignSwitchToLocationSwitch, Generated.False));
            var switchVertex = _graph.Vertices.First(vertex => vertex.Name == AssignSwitchToLocationSwitch);

            PurifyLocation(locationSubGraph);

            _graph.RemoveVertex(switchVertex);
        }

        public void RemoveSwitchFromLocation()
        {
            RemoveBuildBlockFromLocation(new Switch(AssignSwitchToLocationSwitch, Generated.False), AssignSwitchToLocationLocation, AssignSwitchToLocationSwitch);
        }

        public void RemoveLightFromLocation()
        {
            RemoveBuildBlockFromLocation(new Switch(AssignLight2LocationLight, Generated.False), AssignLight2LocationLocation, AssignLight2LocationLight);
        }

        private void RemoveBuildBlockFromLocation(Location buildBlock, string locationName, string buildblockName)
        {
            var locationSubGraph = _graph.SubGraphs.FirstOrDefault(graph => graph.Label == locationName);

            if (locationSubGraph == null) return;

            var switchT = locationSubGraph.Vertices.FirstOrDefault(vertex => vertex.Name == buildblockName);

            if (switchT == null) return;

            var edges =
                _graph.Edges.OfType<MyEdge<Location>>()
                      .Where(edge => edge.Source.Name == buildblockName || edge.Destination.Name == buildblockName)
                      .ToList();

            foreach (var edge in edges)
            {
                _graph.RemoveEdge(edge);
            }


            locationSubGraph.RemoveVertex(switchT);

            _graph.AddVertex(buildBlock);

        }

        private static void PurifyLocation(MySubGraph<Location> locationSubGraph)
        {
            var initItem = locationSubGraph.Vertices.FirstOrDefault(location => location.Generated == Generated.True);

            if (initItem != null)
            {
                locationSubGraph.RemoveVertex(initItem);
            }
        }

        public void AssignLightToLocation()
        {
            var locationSubGraph = _graph.SubGraphs.First(graph => graph.Label == AssignLight2LocationLocation);

            if (locationSubGraph.Vertices.Any(vertex => vertex.Name == AssignLight2LocationLight)) return;
            
            var light = new Light(AssignLight2LocationLight, Generated.False);
            locationSubGraph.AddVertex(light);
            var switchVertex = _graph.Vertices.First(vertex => vertex.Name == AssignLight2LocationLight);
            
            PurifyLocation(locationSubGraph);

            _graph.RemoveVertex(switchVertex);
        }

        public void AssignSwitchToLight()
        {
            var switches = _graph.SubGraphs.SelectMany(subGraph => subGraph.Vertices.OfType<Switch>()).ToList();
            
            var switchFrom = switches.First(item => item.Name == AssignSwitchToLightSwitch);


            var lightes = _graph.SubGraphs.SelectMany(subGraph => subGraph.Vertices.OfType<Light>()).ToList();

            var lightTo = lightes.First(item => item.Name == AssignSwitchToLightLight);

            //edge already exists
            if (_graph.Edges.Any(edge => ((Location)edge.Source).Name == AssignSwitchToLightSwitch && ((Location)edge.Destination).Name == AssignSwitchToLightLight)) return;

            _graph.AddEdge(new MyEdge<Location>(switchFrom, lightTo, new Arrow()));
        }

        public void RemoveSwitchFromLight()
        {
            //edge already exists
            var edgeToDelete = _graph.Edges.OfType<MyEdge<Location>>().FirstOrDefault(edge => edge.Source.Name == AssignSwitchToLightSwitch && edge.Destination.Name == AssignSwitchToLightLight);

            if (edgeToDelete == null) return;

            _graph.RemoveEdge(edgeToDelete);

        }


        public void GenerateArduinos(string foldername)
        {
            var sourceGen = new ArduinoGenerator();
            sourceGen.GenerateFiles(_graph, foldername);


        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public void SaveGraph(string fileName)
        {
            new GraphPersister().Persist(_graph, fileName);


        }

        public void OpenGraph(string fileName)
        {
            _graph = new GraphPersister().Deserialize(fileName);
            OnPropertyChanged("Graph");
            OnPropertyChanged("LocationNames");
            OnPropertyChanged("LightNames");
            OnPropertyChanged("SwitchNames");
        }

        public void AddArduinoGroup()
        {
            if (_arduinoGroup.ContainsKey(ArduinoGroupName)) return;
            _arduinoGroup[ArduinoGroupName] = new List<string>();
            OnPropertyChanged("ArduinoGroupNames");
            OnPropertyChanged("ArduinoGroups");
        }

        public void DeleteArduinoGroup()
        {
            if (!_arduinoGroup.ContainsKey(ArduinoGroupName)) return;

            _arduinoGroup.Remove(ArduinoGroupName);
            OnPropertyChanged("ArduinoGroupNames");
            OnPropertyChanged("ArduinoGroups");
        }

        public void AssignLocationToArduinoGroup()
        {
            List<string> group;
            if (!_arduinoGroup.TryGetValue(AssignLocationToArduinoGroupsGroup, out group)) return;

            if (group.Contains(AssignLocationToArduinoGroupsLocation)) return;

            group.Add(AssignLocationToArduinoGroupsLocation);
            OnPropertyChanged("ArduinoGroups");

        }

        public void RemoveLocationFromArduinoGroup()
        {
            List<string> group;
            if (!_arduinoGroup.TryGetValue(AssignLocationToArduinoGroupsLocation, out group)) return;

            if (group.Contains(AssignLocationToArduinoGroupsGroup)) return;

            group.Remove(AssignLocationToArduinoGroupsGroup);
            OnPropertyChanged("ArduinoGroups");

        }
    }


    public class DiamondArrow
    {
    }

    public class Arrow
    {
    }


}
