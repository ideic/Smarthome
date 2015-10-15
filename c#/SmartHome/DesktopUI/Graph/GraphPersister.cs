using System;
using System.IO;
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