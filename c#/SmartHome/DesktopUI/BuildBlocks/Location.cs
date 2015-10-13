using System;

namespace DesktopUI.BuildBlocks
{
    public class Location
    {
        public Location(string name, Generated generated)
        {
            Name = name;
            Id = Guid.NewGuid();
            Generated = generated;
        }

        protected Generated Generated { get; private set; }

        protected Guid Id { get; private set; }

        public string Name
        {
            get; private set;
        }
    }
}