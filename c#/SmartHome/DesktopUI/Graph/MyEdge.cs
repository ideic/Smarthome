using Graphviz4Net.Graphs;

namespace DesktopUI.Graph
{
    public class MyEdge<T> : IEdge<T>
    {
        public MyEdge(T source, T destination, object arrow)
        {
            Source = source;
            Destination = destination;
            DestinationArrow = new Arrow();
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
}