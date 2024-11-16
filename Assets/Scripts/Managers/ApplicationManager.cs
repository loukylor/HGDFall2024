using HGDFall2024.LevelElements;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HGDFall2024.Managers
{
    public class ApplicationManager : BaseManager
    {
        public static ApplicationManager Instance { get; private set; }

        public bool HasQuit { get; private set; } = false;

        private int currentLevel;
        private bool menuOpen = false; 

        protected override void Awake()
        {
            base.Awake();

            SceneManager.activeSceneChanged += OnSceneChange;

            if (!Application.isEditor || SceneManager.sceneCount == 1)
            {
                LoadIntro();
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene.buildIndex >= 3)
                    {
                        currentLevel = scene.buildIndex - 2;
                    }
                }
            }

            LevelEndTrigger.OnLevelEnd += () =>
            {
                Debug.Log("finish level: " + currentLevel);
                ShowLevelMenu(LevelMenuState.LevelComplete);
            };

            InputManager.Instance.Player.Pause.started += OnPause;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            LevelEndTrigger.OnLevelEnd -= () => FinishLevel(currentLevel);
            InputManager.Instance.Player.Pause.started -= OnPause;
        }

        private void OnSceneChange(Scene _, Scene scene)
        {
            HideLevelMenu();

            if (scene.buildIndex >= 3)
            {
                currentLevel = scene.buildIndex - 2;
            }

            StopAllCoroutines();
            switch (scene.buildIndex)
            {
                case 0:
                    break;
                case 1:
                    StartCoroutine(IntroWaiter());
                    Button skipButton = GameObject.Find("Canvas").transform
                        .Find("Background/Skip").GetComponent<Button>();

                    skipButton.onClick.AddListener(LoadMainMenu);
                    break;
                default:
                    break;
            }
        }

        private IEnumerator IntroWaiter()
        {
            yield return new WaitForSeconds(11.5f);

            LoadMainMenu();
        }

        private void OnPause(InputAction.CallbackContext _)
        {
            if (menuOpen)
            {
                HideLevelMenu();
            }
            else
            {
                ShowLevelMenu(LevelMenuState.Paused);
            }
        }

        private void OnApplicationQuit()
        {
            HasQuit = true;
        }

        public void LoadIntro() => LoadScene(1);

        public void LoadMainMenu() => LoadScene(2);

        public void LoadLevel(int level) => LoadScene(3 + level - 1);

        public void ReloadLevel() => LoadLevel(currentLevel);

        public void ShowLevelMenu(LevelMenuState state)
        {
            Transform screen = transform.GetChild(0);
            Transform canvas = screen.Find("Canvas");
            canvas.Find("Buttons/Next").gameObject.SetActive(state == LevelMenuState.LevelComplete);
            canvas.Find("DeathText").gameObject.SetActive(state == LevelMenuState.Died);
            canvas.Find("FinishedText").gameObject.SetActive(state == LevelMenuState.LevelComplete);
            canvas.Find("PausedText").gameObject.SetActive(state == LevelMenuState.Paused);
            Time.timeScale = state == LevelMenuState.Paused ? 0 : 1;

            screen.gameObject.SetActive(true);
            menuOpen = true;
        }

        public void HideLevelMenu()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Time.timeScale = 1;
            menuOpen = false;
        }

        private void LoadScene(int index)
        {
            Debug.Log("Loading scene: " + index);
            Debug.Log(new System.Diagnostics.StackTrace());
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }

        public void FinishLevel(int level)
        {
            if (ProgressManager.Instance.AvailableLevels < level)
            {
                ProgressManager.Instance.AvailableLevels = (uint)level;
            }
            LoadLevel(level + 1);
        }

        public void FinishLevel() => FinishLevel(currentLevel);

        public enum LevelMenuState
        {
            Died,
            LevelComplete,
            Paused
        }
    }
}
