using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphviz4Net.Graphs;

namespace DesktopUI
{
    public class MainViewModel
    {

        private Graph<Location> _graph;

        public MainViewModel()
        {
            _graph = new Graph<Location>();


            //_graph.AddVertex(new Location("Előszoba"));
            //_graph.AddVertex(new Location("Gépészet"));
            var subGraph = new SubGraph<Location>(){Label = "Eloszoba"};
            _graph.AddSubGraph(subGraph);

            subGraph.AddVertex(new Switch("EK1"));
            subGraph.AddVertex(new Light("LEK1"));

            _graph.AddEdge(new Edge<Location>(subGraph.Vertices.First(), subGraph.Vertices.Last(), new Arrow()));

            var subGraph2 = new SubGraph<Location>(){Label = "Gepeszet"};
            _graph.AddSubGraph(subGraph2);
            subGraph2.AddVertex(new Switch("GK1"));


        }

        public Graph<Location> Graph
        {
            get { return _graph; }
        }

        public string NewLocationName { get; set; }

        public string NewSwitchName { get; set; }

        public string NewLightnName { get; set; }

        public IEnumerable<string> LocationNames
        {
            get { return Graph.AllVertices.Select(x => x.Name); }
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
