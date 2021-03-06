﻿using System.Collections.Generic;

namespace DesktopUI.GeneratorSource
{
    public class Segment
    {
        private readonly List<BuildBlock> _switches = new List<BuildBlock>();
        private readonly List<BuildBlock> _lights = new List<BuildBlock>();
        public string Name { get; private set; }
        public string Index { get; private set; }

        public Segment(string name, string index)
        {
            Name = name;
            Index = index;
        }

        public IEnumerable<BuildBlock> Switches
        {
            get { return _switches; }
        }

        public List<BuildBlock> Lights
        {
            get { return _lights; }
        }

        public void AddSwitch(string name, string address)
        {
            _switches.Add(new BuildBlock(name, address));
        }

        public void AddLight(string name, string nextAddress)
        {
            _lights.Add(new BuildBlock(name, nextAddress));
        }
    }
}