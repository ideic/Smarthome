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
            _graph.AddVertex(new Location());
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
        public string Name
        {
            get { return "Test"; } 
        }

        public override string ToString()
        {
            return "Location";
        }
    }

    public class Switch
    {
    }

    public class Light
    {

    }
    public class DiamondArrow
    {
    }

    public class Arrow
    {
    }


}
