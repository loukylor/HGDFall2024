using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HGDFall2024.Managers
{
    public class ApplicationManager : BaseManager
    {
        public static ApplicationManager Instance { get; private set; }

        public bool HasQuit { get; private set; } = false;

        protected override void Awake()
        {
            base.Awake();

            SceneManager.activeSceneChanged += OnSceneChange;

            if (!Application.isEditor || SceneManager.GetActiveScene().buildIndex == 0)
            {
                LoadIntro();
            }
        }

        private void OnSceneChange(Scene _, Scene scene)
        {
            switch (scene.buildIndex)
            {
                case 0:
                    break;
                case 1:
                    Button skipButton = GameObject.Find("Canvas").transform
                        .Find("Background/Skip").GetComponent<Button>();

                    skipButton.onClick.AddListener(LoadMainMenu);
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        private void OnApplicationQuit()
        {
            HasQuit = true;
        }

        public void LoadIntro()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        public void LoadLevel(int level)
        {
            SceneManager.LoadScene(3 + level - 1, LoadSceneMode.Single);
        }

        public void FinishLevel(int level)
        {
            if (ProgressManager.Instance.AvailableLevels < level)
            {
                ProgressManager.Instance.AvailableLevels = (uint)level;
            }
            LoadLevel(level);
        }
    }
}
