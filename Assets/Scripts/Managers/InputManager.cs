namespace HGDFall2024.Managers
{
    public class InputManager : BaseManager
    {
        public static InputManager Instance { get; private set; }

        public InputMap.PlayerActions Player => _inputMap.Player;

        private InputMap _inputMap;

        protected override void Awake()
        {
            base.Awake();

            _inputMap = new InputMap();
            _inputMap.Enable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _inputMap.Dispose();
            _inputMap = null;
        }
    }
}
