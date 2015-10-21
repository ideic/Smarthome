using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using DesktopUI.BuildBlocks;
using Newtonsoft.Json;

namespace DesktopUI.Graph
{
    public class GraphPersister
    {
        public void Persist(MyGraph<Location> graph, ArduinoGroupWrapper arduinoGroup, string fileName)
        {
            var binder = new TypeNameSerializationBinder();
            var serilaizeWrapper = new SerializeWrapper(graph, arduinoGroup);

            var serializedJson = JsonConvert.SerializeObject(serilaizeWrapper, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Binder = binder
                });
            
            File.WriteAllText(fileName, serializedJson);
        }

        public void Deserialize(string fileName, out MyGraph<Location> graph, out ArduinoGroupWrapper arduinoGroup)
        {
            var binder = new TypeNameSerializationBinder();
            var graphString = File.ReadAllText(fileName);
            var deserialized = JsonConvert.DeserializeObject<SerializeWrapper>(graphString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });

            graph = deserialized.Graph;
            arduinoGroup = deserialized.ArduinoGroup;

            foreach (var edge in graph.Edges.OfType<MyEdge<Location>>().ToList())
            {
                var from = edge.Source;
                var to = edge.Destination;

                var vertices = graph.SubGraphs.SelectMany(subgraph => subgraph.Vertices).ToList();
                vertices.AddRange(graph.Vertices);

                graph.RemoveEdge(edge);
                graph.AddEdge(
                    new MyEdge<Location>(vertices.First(vertex => vertex.Name == from.Name),
                    vertices.First(vertex => vertex.Name == to.Name),edge.DestinationArrow)
                );
            }
        }
    }

    public class SerializeWrapper
    {
        public SerializeWrapper(MyGraph<Location> graph, ArduinoGroupWrapper arduinoGroup)
        {
            Graph = graph;
            ArduinoGroup = arduinoGroup;
        }

        public ArduinoGroupWrapper ArduinoGroup{ get; set; }

        public MyGraph<Location> Graph { get; set; }
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

}