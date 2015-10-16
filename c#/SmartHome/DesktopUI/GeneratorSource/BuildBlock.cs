namespace DesktopUI.GeneratorSource
{
    public class BuildBlock
    {
        public BuildBlock(string name, string address)
        {
            Name = name;
            Address = address;
        }

        protected string Address { get; private set; }

        public string Name { get; private set; }
    }
}