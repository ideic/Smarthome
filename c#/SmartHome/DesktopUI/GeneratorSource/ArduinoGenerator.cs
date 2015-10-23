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
        private const int START_PIN_NUMBER_LIGHT = 5;
        private const int START_PIN_NUMBER_RELAY = 11;

        private const int MAX_DEVICE_NUMBER_LIGHT = 6;
        private const int MAX_DEVICE_NUMBER_RELAY = 3;

        public void GenerateFiles(MyGraph<Location> graph, ArduinoGroupWrapper arduinoGroup, string foldername)
        {
            CreateSegments(graph);

            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }

            CopyServerFolder(foldername);

            CreateArduinos(graph, arduinoGroup);

            foreach (var arduino in _arduinos)
            {
                CopyArduinoFolder(arduino, foldername);
            }

            var readme = new Readme(_segmentManager, _arduinos);
            var readmeContent = readme.TransformText();
            File.WriteAllText(Path.Combine(foldername, "readme.txt"), readmeContent);
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

        private void CreateArduinos(MyGraph<Location> graph, ArduinoGroupWrapper arduinoGroup)
        {
            var groupSegment = new Dictionary<string, List<Segment>>();
            _arduinos = new List<Arduino>();

            foreach (var segment in _segmentManager.Segments)
            {
                var buildBlock = segment.Switches.First();
                var locationName =
                    graph.SubGraphs.First(subGraph => subGraph.Vertices.Any(vertex => vertex.Name == buildBlock.Name)).Label;

                var agroup =
                    arduinoGroup.ArduinoGroups.FirstOrDefault(
                        group => group.Locations.Any(location => location == locationName));
                if (agroup != null)
                {
                    if (!groupSegment.ContainsKey(agroup.Name))
                    {
                        groupSegment[agroup.Name] = new List<Segment>();
                    }
                    groupSegment[agroup.Name].Add(segment);
                }
                else
                {
                    if (!groupSegment.ContainsKey(locationName))
                    {
                        groupSegment[locationName] = new List<Segment>();
                    }

                    groupSegment[locationName].Add(segment);
                }
            }

            foreach (var groupSegmentItem in groupSegment)
            {
                var arduinos = new List<Arduino>();
                foreach (var segment in groupSegmentItem.Value)
                {
                    var currentArduino = new Arduino(groupSegmentItem.Key);

                    var segment1 = segment;
                    CreateArduinoSwitches(segment1.Switches, ref currentArduino, arduinos);
                    CreateArduinoRelay(segment1.Lights, ref currentArduino, arduinos);
                    arduinos.Add(currentArduino);
                }                
                _arduinos.AddRange(ZipArduinos(arduinos));
            }
            _arduinos.Sort((x,y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));

            Arduino prevArduino = null;
            string prevArduinoOriginalName = null;
            foreach (var arduino in _arduinos)
            {
                if (arduino.Name == prevArduinoOriginalName)
                {
                    arduino.Name = prevArduino.Name + "I";
                }
                else
                {
                    prevArduinoOriginalName = arduino.Name;
                }

                prevArduino = arduino;
            }
        }

        private List<Arduino> ZipArduinos(List<Arduino> arduinos)
        {
            var result = new List<Arduino>();
            Arduino prevArduino = null;

            foreach (var arduino in arduinos)
            {
                if (prevArduino == null)
                {
                    prevArduino = arduino;
                    continue;
                }


                if (Mergeable(prevArduino,arduino))
                {
                    var newArduino = MergeArduino(prevArduino, arduino);
                    prevArduino = newArduino;
                    continue;
                }
                result.Add(prevArduino);
                prevArduino = arduino;
            }

            if (prevArduino != null)
            {
                result.Add(prevArduino);
            }

            return result;
        }

        private bool Mergeable(Arduino prevArduino, Arduino arduino)
        {
            return
                (prevArduino.Devices.Count(device => device.DeviceType == DeviceType.LightSwitchDeviceType.ToString()) +
                 arduino.Devices.Count(device => device.DeviceType == DeviceType.LightSwitchDeviceType.ToString())
                ) <= MAX_DEVICE_NUMBER_LIGHT &&
                (prevArduino.Devices.Count(device => device.DeviceType == DeviceType.RelayDeviceType.ToString()) +
                 arduino.Devices.Count(device => device.DeviceType == DeviceType.RelayDeviceType.ToString()
                ) <= MAX_DEVICE_NUMBER_RELAY);

        }

        private Arduino MergeArduino(Arduino prevArduino, Arduino arduino)
        {
            var result = new Arduino(prevArduino.Name);

            var pinNumber = START_PIN_NUMBER_LIGHT;
            foreach (var device in prevArduino.Devices.Where(device => device.DeviceType == DeviceType.LightSwitchDeviceType.ToString())
                .Union(arduino.Devices.Where(device => device.DeviceType == DeviceType.LightSwitchDeviceType.ToString())))
            {
                device.PinNumber = (pinNumber).ToString();
                pinNumber++;
                result.AddDevice(device);
            }

            pinNumber = START_PIN_NUMBER_RELAY;
            foreach (var device in prevArduino.Devices.Where(device => device.DeviceType == DeviceType.RelayDeviceType.ToString())
                .Union(arduino.Devices.Where(device => device.DeviceType == DeviceType.RelayDeviceType.ToString())))
            {
                device.PinNumber = (pinNumber).ToString();
                pinNumber++;
                result.AddDevice(device);
            }
          

            return result;
        }


        private void CreateArduinoSwitches(IEnumerable<BuildBlock> switches, ref Arduino currentArduino, List<Arduino> arduinos)
        {
            var pinNumber = START_PIN_NUMBER_LIGHT;
            foreach (var buildBlockItem in switches)
            {
                if (pinNumber >= START_PIN_NUMBER_LIGHT + MAX_DEVICE_NUMBER_LIGHT)
                {
                    arduinos.Add(currentArduino);
                    currentArduino = new Arduino(currentArduino.Name);
                    pinNumber = START_PIN_NUMBER_LIGHT;
                }

                currentArduino.AddDevice(new Device(DeviceType.LightSwitchDeviceType, buildBlockItem.Name, pinNumber));
                pinNumber++;

            }
        }

        private void CreateArduinoRelay(IEnumerable<BuildBlock> relays, ref Arduino currentArduino, List<Arduino> arduinos)
        {
            var pinNumber = START_PIN_NUMBER_RELAY;
            foreach (var buildBlockItem in relays)
            {
                if (pinNumber >= START_PIN_NUMBER_RELAY+MAX_DEVICE_NUMBER_RELAY)
                {
                    arduinos.Add(currentArduino);
                    currentArduino = new Arduino(currentArduino.Name);
                    pinNumber = START_PIN_NUMBER_RELAY;
                }

                currentArduino.AddDevice(new Device(DeviceType.RelayDeviceType, buildBlockItem.Name, pinNumber));
                pinNumber++;
            }
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
