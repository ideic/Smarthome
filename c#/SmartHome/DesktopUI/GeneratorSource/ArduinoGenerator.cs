using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopUI.BuildBlocks;
using DesktopUI.GeneratorSource.Client;
using DesktopUI.GeneratorSource.Common;
using DesktopUI.GeneratorSource.Server;
using DesktopUI.Graph;

namespace DesktopUI.GeneratorSource
{
    public class ArduinoGenerator
    {
        private readonly IdentityGenerator _identity = new IdentityGenerator();
        private SegmentManager _segmentManager;
        private List<Arduino> _arduinos;
        private const int START_PIN_NUMBER = 4;
        private const int MAX_DEVICE_NUMBER = 9;

        public void GenerateFiles(MyGraph<Location> graph, string foldername)
        {
            CreateSegments(graph);

            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }

            CopyServerFolder(foldername);

            CreateArduinos();

            foreach (var arduino in _arduinos)
            {
                CopyArduinoFolder(arduino, foldername);
            }
        }

        public IEnumerable<Segment> Segments
        {
            get { return _segmentManager.Segments; }
        } 

        public IEnumerable<Arduino> Arduinos
        {
            get { return _arduinos; }
        } 

        private void CopyArduinoFolder(Arduino arduino, string folder)
        {
            var folderName = Path.Combine(folder, arduino.Name);

            CopyFolderCore(folderName, () => GetClientFileContent(arduino), arduino.Name + ".Ino");

            var clientFolder = Path.Combine(Directory.GetCurrentDirectory(), "GeneratorSource", "Client");
            var destinationFolder = Path.Combine(folder, arduino.Name);

            foreach (var clientFile in Directory.GetFiles(clientFolder))
            {
                File.Copy(clientFile, Path.Combine(destinationFolder, Path.GetFileName(clientFile)));
            }
        }

        private void CreateArduinos()
        {
            _arduinos = new List<Arduino>();
            foreach (var segment in _segmentManager.Segments)
            {
                var deviceNumber = 0;
                var currentArduino = new Arduino(segment.Name);

                Segment segment1 = segment;
                deviceNumber = CreateArduinoCore(()=>segment1.Switches, deviceNumber, ref currentArduino, DeviceType.LightSwitchDeviceType);
                CreateArduinoCore(() => segment1.Lights, deviceNumber, ref currentArduino, DeviceType.RelayDeviceType);
                _arduinos.Add(currentArduino);
            }
        }

        private int CreateArduinoCore(Func<IEnumerable<BuildBlock>> buildBlockProvider, int deviceNumber, ref Arduino currentArduino, DeviceType deviceType)
        {
            foreach (var buildBlockItem in buildBlockProvider())
            {
                deviceNumber++;
                if (deviceNumber >= MAX_DEVICE_NUMBER)
                {
                    _arduinos.Add(currentArduino);
                    var arduinoName = currentArduino.Name + "_1";
                    currentArduino = new Arduino(arduinoName);
                    deviceNumber = 1;
                }

                currentArduino.AddDevice(new Device(deviceType, buildBlockItem.Name,
                                                    deviceNumber + START_PIN_NUMBER));
            }
            return deviceNumber;
        }

        private void CopyServerFolder(string foldername)
        {
            CopyFolderCore(Path.Combine(foldername, "Server"), GetServerFileContent, "Server.Ino");
        }

        private void CopyFolderCore(string folder, Func<string> contentProvider, string fileName)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            Directory.CreateDirectory(folder);

            var content = contentProvider();

            File.WriteAllText(Path.Combine(folder, fileName), content);

            CopyCommonFolder(folder);

        }


        private void CopyCommonFolder(string destinationFolder)
        {
            var commonFolder = Path.Combine(Directory.GetCurrentDirectory(), "GeneratorSource", "Common");

            var addressContent = GetAddressFileContent();
            File.WriteAllText(Path.Combine(destinationFolder, "Addresses.h"), addressContent);

            foreach (var commonFile in Directory.GetFiles(commonFolder))
            {
                File.Copy(commonFile, Path.Combine(destinationFolder, Path.GetFileName(commonFile)));
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

        private string GetClientFileContent(Arduino arduino)
        {
            var clientTemplate = new ClientTemplate(arduino);
            return clientTemplate.TransformText();
        }


        private void CreateSegments(MyGraph<Location> graph)
        {
            _segmentManager = new SegmentManager();

            foreach (var edge in graph.Edges.OfType<MyEdge<Location>>())
            {
                var segment = _segmentManager.Segments.FirstOrDefault(sgmt =>
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
