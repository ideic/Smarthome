using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DesktopUI.Annotations;
using DesktopUI.BuildBlocks;
using DesktopUI.Graph;
using Graphviz4Net.Graphs;
using Newtonsoft.Json;

namespace DesktopUI
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private readonly MyGraph<Location> _graph;

        public MainViewModel()
        {
            _graph = new MyGraph<Location>();
            _graph.Changed += GraphChanged;

            /*      //_graph.AddVertex(new Location("Előszoba"));
            //_graph.AddVertex(new Location("Gépészet"));
            var subGraph = new MySubGraph<Location>(){Label = "Eloszoba"};
            _graph.AddSubGraph(subGraph);

            subGraph.AddVertex(new Switch("EK1", "EK1"));
            subGraph.AddVertex(new Light("LEK1", "LEK1"));

            _graph.AddEdge(new MyEdge<Location>(subGraph.Vertices.First(), subGraph.Vertices.Last(), new Arrow()));

            var subGraph2 = new MySubGraph<Location>(){Label = "Gepeszet"};
            _graph.AddSubGraph(subGraph2);
            subGraph2.AddVertex(new Switch("GK1", "GK1"));
            */
        }

        private void GraphChanged(object sender, GraphChangedArgs e)

        {
            OnPropertyChanged("Graph");

        }


        public class TypeNameSerializationBinder : SerializationBinder
        {
            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.FullName;
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                return Type.GetType(typeName, true);
            }
        }


        public MyGraph<Location> Graph
        {
            get { return _graph; }
        }

        public string NewLocationName { get; set; }

        public string NewSwitchName { get; set; }

        public string NewLightName { get; set; }

        public string NewLocationAssign { get; set; }

        public string NewSwitchAssign { get; set; }

        public string SwitchToLightAssign { get; set; }

        public string LightAssign { get; set; }
    

        public IEnumerable<string> LocationNames
        {
            get { return _graph.SubGraphs.Select(subGraph => subGraph.Label); }
        }

        public IEnumerable<string> SwitchNames
        {
            get
            {
                var result = new List<string>();
                foreach (var subgraph in _graph.SubGraphs)
                {
                    result.AddRange(subgraph.Vertices.OfType<Switch>().Select(vertex => vertex.Name));
                }
                return result;
            }
        }

        public IEnumerable<string> LightNames
        {
            get 
            {
                var result = new List<string>();
                foreach (var subgraph in _graph.SubGraphs)
                {
                    result.AddRange(subgraph.Vertices.OfType<Light>().Select(vertex => vertex.Name));
                }
                return result;
            }
        }
        public void CreateNewLocation()
        {
            var subGraph = new MySubGraph<Location>{Label = NewLocationName};
            subGraph.AddVertex(new Location("?", Generated.True));

            _graph.AddSubGraph(subGraph);
        }

        public void CreateNewSwitch()
        {
            _graph.AddVertex(new Switch(NewSwitchName, Generated.False));
        }

        public void CreateNewLight()
        {
            _graph.AddVertex(new Light(NewLightName, Generated.False));
        }

        public void AssignSwitchToLocation()
        {
            var locationSubGraph = _graph.SubGraphs.First(graph => graph.Label == NewLocationAssign);
            locationSubGraph.AddVertex(new Switch(NewSwitchAssign, Generated.False));
            var switchVertex = _graph.Vertices.First(vertex => vertex.Name == NewSwitchAssign);
            _graph.RemoveVertex(switchVertex);
        }

        public void AssignLightToSwitch()
        {
            var locationSubGraph = _graph.SubGraphs.First(graph => graph.Vertices.Any(vertex => vertex.Name == SwitchToLightAssign));
            locationSubGraph.AddVertex(new Light(LightAssign, Generated.False));
            var switchVertex = _graph.Vertices.First(vertex => vertex.Name == LightAssign);
            _graph.RemoveVertex(switchVertex);
            
        }

        public void GenerateArduinos(string filename)
        {
            var binder = new TypeNameSerializationBinder();

            var resString = JsonConvert.SerializeObject(_graph, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });



            //_graph = new MyGraph<Location>();


            var deserialized = JsonConvert.DeserializeObject<MyGraph<Location>>(resString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });


            //_graph = deserialized;

            _graph.Edges.Cast<MyEdge<Location>>().First().Source = (Location)_graph.SubGraphs.First().Vertices.First();
            _graph.Edges.Cast<MyEdge<Location>>().First().Destination = (Location)_graph.SubGraphs.First().Vertices.Last();            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class DiamondArrow
    {
    }

    public class Arrow
    {
    }


}
