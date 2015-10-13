using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Graphviz4Net.Dot;
using Graphviz4Net.Dot.AntlrParser;
using Graphviz4Net.Graphs;
using Newtonsoft.Json;

namespace DesktopUI
{
    public class MainViewModel
    {

        private MyGraph<Location> _graph;

        public MainViewModel()
        {
            _graph = new MyGraph<Location>();


            //_graph.AddVertex(new Location("Előszoba"));
            //_graph.AddVertex(new Location("Gépészet"));
            var subGraph = new MySubGraph<Location>(){Label = "Eloszoba"};
            _graph.AddSubGraph(subGraph);

            subGraph.AddVertex(new Switch("EK1"));
            subGraph.AddVertex(new Light("LEK1"));

            _graph.AddEdge(new MyEdge<Location>(subGraph.Vertices.First(), subGraph.Vertices.Last(), new Arrow()));

            var subGraph2 = new MySubGraph<Location>(){Label = "Gepeszet"};
            _graph.AddSubGraph(subGraph2);
            subGraph2.AddVertex(new Switch("GK1"));


            var binder = new TypeNameSerializationBinder("DesktopUI");
      
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


            _graph = deserialized;

            _graph.Edges.Cast<MyEdge<Location>>().First().Source = (Location) _graph.SubGraphs.First().Vertices.First();
            _graph.Edges.Cast<MyEdge<Location>>().First().Destination = (Location)_graph.SubGraphs.First().Vertices.Last();

        }

        public class TypeNameSerializationBinder : SerializationBinder
        {
            public string TypeFormat { get; private set; }

            public TypeNameSerializationBinder(string typeFormat)
            {
                TypeFormat = typeFormat;
            }

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

        public string NewLightnName { get; set; }

        public IEnumerable<string> LocationNames
        {
            get { return Graph.Vertices.Select(x => ((Location)x).Name); }
        }
    }

    public class MyGraph<T> : IGraph
    {
        private ICollection<MySubGraph<T>> _subGraphs;
        private ICollection<MyEdge<T>> _edges;
        private ICollection<T> _vertices;

        public MyGraph()
        {
            _edges = new List<MyEdge<T>>();
            _vertices = new List<T>();
            _subGraphs = new List<MySubGraph<T>>();

        }
        public IEnumerable<IEdge> Edges {
            get { return _edges; }
        }

        public IEnumerable<object> Vertices {
            get { return _vertices.Cast<object>(); }
        }

        public IEnumerable<ISubGraph> SubGraphs {
            get { return _subGraphs; }
        }

        public event EventHandler<GraphChangedArgs> Changed;

        public void AddSubGraph(MySubGraph<T> subGraph)
        {
            _subGraphs.Add(subGraph);
        }

        public void AddEdge(MyEdge<T> edge)
        {
            _edges.Add(edge);
        }
    }

    public class MyEdge<T> : IEdge<T>
    {
        public MyEdge(T source, T destination, Arrow arrow)
        {
            Source = source;
            Destination = destination;
            DestinationArrow = arrow;
        }

        object IEdge.Source
        {
            get { return Source; }
        }

        public T Destination { get; set; }

        public T Source { get; set; }

        object IEdge.Destination
        {
            get { return Destination; }
        }

        public object DestinationPort
        {
             get; set; 
        }

        public object DestinationArrow { get; private set; }

        public object SourceArrow { get; set; }

        public object SourcePort { get; set; }
    }

    public class MySubGraph<T> : ISubGraph<T>
    {
        private ICollection<T> _vertices;
        public MySubGraph()
        {
            _vertices = new List<T>();
        }
        
        public IEnumerable<T> Vertices {
            get {  return _vertices; }
        }

        IEnumerable<object> ISubGraph.Vertices
        {
            get { return _vertices.Cast<object>(); }
        }

        public string Label { get; set; }

        public event EventHandler<GraphChangedArgs> Changed;

        public void AddVertex(T vertex)
        {
            _vertices.Add(vertex);
        }
    }




    public class Location
    {
        public Location(string name)
        {
            Name = name;
        }

        public string Name
        {
            get; private set;
        }
    }

    public class Switch : Location
    {
        public Switch(string name) : base(name)
        {
        }
    }

    public class Light : Location
    {
        public Light(string name) : base(name)
        {
        }
    }
    public class DiamondArrow
    {
    }

    public class Arrow
    {
    }


}
