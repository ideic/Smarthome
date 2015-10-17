using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopUI.BuildBlocks;
using DesktopUI.GeneratorSource.Common;
using DesktopUI.GeneratorSource.Server;
using DesktopUI.Graph;

namespace DesktopUI.GeneratorSource
{
    public class ArduinoGenerator
    {
        private readonly IdentityGenerator _identity = new IdentityGenerator();
        private List<Segment> _segments;
        private SegmentManager _segmentManager;

        public void GenerateFiles(MyGraph<Location> graph, string foldername)
        {
            CreateSegments(graph);

            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }

            var serverFolder = Path.Combine(foldername, "Server");
            var commonFolder = Path.Combine(Directory.GetCurrentDirectory(), "GeneratorSource", "Common");
            
            if (Directory.Exists(serverFolder))
            {
                Directory.Delete(serverFolder, true);
            }

            Directory.CreateDirectory(serverFolder);


            var serverFileContent = GetServerFileContent();

            File.WriteAllText(Path.Combine(serverFolder, "Server.Ino"), serverFileContent);

            var addressContent = GetAddressFileContent();
            File.WriteAllText(Path.Combine(serverFolder, "Addresses.h"), addressContent);

            foreach (var commonFile in Directory.GetFiles(commonFolder))
            {
                File.Copy(commonFile, Path.Combine(serverFolder, Path.GetFileName(commonFile)));
            }
            

        }

        private string GetAddressFileContent()
        {
            var addressTemplate = new AddressTemplate(_segmentManager);

            return addressTemplate.TransformText();
        }

        private string GetServerFileContent()
        {
            var serverTemplate = new ServerTemplate(_segmentManager);
            return serverTemplate.TransformText();
        }

        private void CreateSegments(MyGraph<Location> graph)
        {
            _segments = new List<Segment>();
            _segmentManager = new SegmentManager();

            foreach (var edge in graph.Edges.OfType<MyEdge<Location>>())
            {
                var segment = _segments.FirstOrDefault(sgmt =>
                                                       sgmt.Switches.Any(switchItem => switchItem.Name == edge.Source.Name)
                                                       || sgmt.Lights.Any(lightItem => lightItem.Name == edge.Destination.Name));
                if (segment != null)
                {
                    if (segment.Switches.All(switchItem => switchItem.Name != edge.Source.Name))
                    {
                        segment.AddSwitch(edge.Source.Name, _identity.NextAddress());
                    }

                    if (segment.Lights.All(lightItem => lightItem.Name != edge.Destination.Name))
                    {
                        segment.AddLight(edge.Destination.Name, _identity.NextAddress());
                    }
                }
                else
                {
                    segment = new Segment(_identity.NextSegmentName(), _identity.NextSequentialId());
                    _segmentManager.AddSegmentItem(segment);
                    segment.AddSwitch(edge.Source.Name, _identity.NextAddress());
                    segment.AddLight(edge.Destination.Name, _identity.NextAddress());
                }
            }
        }
    }
}
