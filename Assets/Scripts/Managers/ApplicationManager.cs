namespace HGDFall2024.Managers
{
    public class ApplicationManager : BaseManager
    {
        public static ApplicationManager Instance { get; private set; }

        public bool HasQuit { get; private set; } = false;

        private void OnApplicationQuit()
        {
            HasQuit = true;
        }
    }
}
