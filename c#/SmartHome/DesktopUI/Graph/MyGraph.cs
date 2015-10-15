using System;
using System.Collections.Generic;
using System.Linq;
using Graphviz4Net.Graphs;

namespace DesktopUI.Graph
{
    public class MyGraph<T> : IGraph
    {
        private readonly ICollection<MySubGraph<T>> _subGraphs;
        private readonly ICollection<MyEdge<T>> _edges;
        private readonly ICollection<T> _vertices;

        public MyGraph()
        {
            _edges = new List<MyEdge<T>>();
            _vertices = new List<T>();
            _subGraphs = new List<MySubGraph<T>>();

        }
        public IEnumerable<IEdge> Edges {
            get { return _edges; }
        }

        IEnumerable<object> IGraph.Vertices {
            get { return _vertices.Cast<object>(); }
        }

        public ICollection<T> Vertices
        {
            get { return _vertices; }
        } 

        IEnumerable<ISubGraph> IGraph.SubGraphs {
            get { return _subGraphs; }
        }

        public IEnumerable<MySubGraph<T>> SubGraphs
        {
            get { return _subGraphs; }
        }


        public event EventHandler<GraphChangedArgs> Changed;

        public void AddSubGraph(MySubGraph<T> subGraph)
        {
            _subGraphs.Add(subGraph);
            Changed.SafeInvoke(this);
        }

        public void AddEdge(MyEdge<T> edge)
        {
            _edges.Add(edge);
            Changed.SafeInvoke(this);
        }

        public void AddVertex(T vertex)
        {
            _vertices.Add(vertex);
            Changed.SafeInvoke(this);
        }

        public void RemoveVertex(T vertex)
        {
            _vertices.Remove(vertex);
            Changed.SafeInvoke(this);
        }

        public void RemoveSubGraph(MySubGraph<T> subGraph)
        {
            _subGraphs.Remove(subGraph);
            Changed.SafeInvoke(this);
        }

        public void RemoveEdge(MyEdge<T> edge)
        {
            _edges.Remove(edge);
            Changed.SafeInvoke(this);
        }

        public void Refresh()
        {
            Changed.SafeInvoke(this);
        }
    }
    public static class EventHandlerExtension
    {
        public static void SafeInvoke(this EventHandler<GraphChangedArgs> evnt, object sender)
        {
            var e = evnt;
            if (e != null)
            {
                e.Invoke(sender, new GraphChangedArgs());
            }
        }
    }
}