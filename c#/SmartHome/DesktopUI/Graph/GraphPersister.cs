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
        public void Persist(MyGraph<Location> graph, string fileName)
        {
            var binder = new TypeNameSerializationBinder();

            var serializedJson = JsonConvert.SerializeObject(graph, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Binder = binder
                });
            
            File.WriteAllText(fileName, serializedJson);
        }

        public MyGraph<Location> Deserialize(string fileName)
        {
            var binder = new TypeNameSerializationBinder();
            var graphString = File.ReadAllText(fileName);
            var deserialized = JsonConvert.DeserializeObject<MyGraph<Location>>(graphString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });

            foreach (var edge in deserialized.Edges.OfType<MyEdge<Location>>().ToList())
            {
                var from = edge.Source;
                var to = edge.Destination;

                var vertices = deserialized.SubGraphs.SelectMany(subgraph => subgraph.Vertices).ToList();
                vertices.AddRange(deserialized.Vertices);

                deserialized.RemoveEdge(edge);
                deserialized.AddEdge(
                    new MyEdge<Location>(vertices.First(vertex => vertex.Name == from.Name),
                    vertices.First(vertex => vertex.Name == to.Name),edge.DestinationArrow)
                );
            }
            return deserialized;
        }
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