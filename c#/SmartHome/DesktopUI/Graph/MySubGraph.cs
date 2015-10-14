using System;
using System.Collections.Generic;
using System.Linq;
using Graphviz4Net.Graphs;

namespace DesktopUI.Graph
{
    public class MySubGraph<T> : ISubGraph<T>
    {
        private readonly ICollection<T> _vertices;
        private string _label;

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

        public string Label
        {
            get { return _label; }
            set 
            { 
                _label = value;
                Changed.SafeInvoke(this);
            }
        }

        public event EventHandler<GraphChangedArgs> Changed;

        public void AddVertex(T vertex)
        {
            _vertices.Add(vertex);
            Changed.SafeInvoke(this);
        }

        public void RemoveVertex(T initItem)
        {
            _vertices.Remove(initItem);
            Changed.SafeInvoke(this);
        }
    }
}