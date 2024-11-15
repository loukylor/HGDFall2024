using HGDFall2024.LevelElements;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HGDFall2024.Managers
{
    public class ApplicationManager : BaseManager
    {
        public static ApplicationManager Instance { get; private set; }

        public bool HasQuit { get; private set; } = false;

        private int _currentLevel;

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
                        _currentLevel = scene.buildIndex - 2;
                    }
                }
            }

            LevelEndTrigger.OnLevelEnd += () =>
            {
                Debug.Log("finish level: " + _currentLevel);
                ShowLevelEndScreen(false);
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            LevelEndTrigger.OnLevelEnd -= () => FinishLevel(_currentLevel);
        }

        private void OnSceneChange(Scene _, Scene scene)
        {
            HideLevelEndScreen();

            if (scene.buildIndex >= 3)
            {
                _currentLevel = scene.buildIndex - 2;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && _currentLevel != 0)
            {
                FinishLevel(_currentLevel);
            }
        }

        private void OnApplicationQuit()
        {
            HasQuit = true;
        }

        public void LoadIntro() => LoadScene(1);

        public void LoadMainMenu() => LoadScene(2);

        public void LoadLevel(int level) => LoadScene(3 + level - 1);

        public void ReloadLevel() => LoadLevel(_currentLevel);

        public void ShowLevelEndScreen(bool died)
        {
            Transform screen = transform.GetChild(0);
            Transform canvas = screen.Find("Canvas");
            canvas.Find("Buttons/Next").gameObject.SetActive(!died);
            canvas.Find("DeathText").gameObject.SetActive(died);
            canvas.Find("FinishedText").gameObject.SetActive(!died);

            screen.gameObject.SetActive(true);
        }

        public void HideLevelEndScreen() => transform.GetChild(0).gameObject.SetActive(false);

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

        public void FinishLevel() => FinishLevel(_currentLevel);
    }
}
