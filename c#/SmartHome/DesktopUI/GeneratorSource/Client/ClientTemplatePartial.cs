namespace DesktopUI.GeneratorSource.Client
{
    public partial class ClientTemplate
    {
        public ClientTemplate(Arduino arduino)
        {
            Arduino = arduino;
        }

        protected Arduino Arduino { get; private set; }
    }
}
