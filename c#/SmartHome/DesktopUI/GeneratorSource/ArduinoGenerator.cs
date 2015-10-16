using System.Collections.Generic;
using System.Linq;
using DesktopUI.BuildBlocks;
using DesktopUI.GeneratorSource.Server;
using DesktopUI.Graph;

namespace DesktopUI.GeneratorSource
{
    public class ArduinoGenerator
    {
        private readonly IdentityGenerator _identity = new IdentityGenerator();

        public void GenerateFiles(MyGraph<Location> graph, string foldername)
        {
            var segments = new List<Segment>();
            var serverTemplate = new ServerTemplate();



            foreach (var edge in graph.Edges.OfType<MyEdge<Location>>())
            {
                var segment = segments.FirstOrDefault(sgmt => 
                    sgmt.Switches.Any(switchItem => switchItem.Name == edge.Source.Name)
                    || sgmt.Lights.Any(lightItem => lightItem.Name == edge.Source.Name));
                if (segment != null)
                {
                    if (segment.Switches.All(switchItem => switchItem.Name != edge.Source.Name))
                    {
                        segment.AddSwitch(edge.Source.Name, _identity.NextAddress());
                    }

                    if (segment.Lights.All(lightItem => lightItem.Name != edge.Source.Name))
                    {
                        segment.AddLight(edge.Source.Name, _identity.NextAddress());
                    }
                    
                }
                else
                {
                    segment = new Segment(_identity.NextSegmentName(), _identity.NextSequentialId());
                    serverTemplate.AddSegmentItem(segment);
                    segment.AddSwitch(edge.Source.Name, _identity.NextAddress());
                    segment.AddLight(edge.Source.Name, _identity.NextAddress());
                }
            }

            var serverFileContent = serverTemplate.TransformText();
        }
    }
}
