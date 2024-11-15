using HGDFall2024.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HGDFall2024.UI
{
    public class UIFunctions : MonoBehaviour
    {
        private Transform screens;

        private void Start()
        {
            screens = GameObject.Find("Canvas/Screens").transform;
            ShowMainMenuScreen();
        }

        public void StartClick()
        {
            ApplicationManager.Instance.LoadLevel(1);
        }

        public void LoadLevel(int level)
        {
            ApplicationManager.Instance.LoadLevel(level);
        }

        public void ShowMainMenuScreen()
        {
            HideScreens();

            Transform mainMenu = screens.Find("MainMenu");

            Button select = mainMenu.Find("Buttons/Select").GetComponent<Button>();
            select.interactable = ProgressManager.Instance.AvailableLevels != 0;

            mainMenu.gameObject.SetActive(true);
        }

        public void ShowLevelsScreen()
        {
            HideScreens();

            Transform levels = screens.Find("Levels");

            Transform buttons = levels.Find("Buttons");
            if (buttons.childCount == 1)
            {
                GameObject template = buttons.Find("1").gameObject;
                template.GetComponent<Button>().onClick.AddListener(() => LoadLevel(1));

                for (int i = 2; i <= SceneManager.sceneCountInBuildSettings - 3; i++)
                {
                    GameObject newButton = Instantiate(template, buttons);
                    newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
                    newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(i));
                }
            }

            for (int i = 0; i < buttons.childCount; i++)
            {
                Button button = buttons.GetChild(i).GetComponent<Button>();
                if (i <= ProgressManager.Instance.AvailableLevels)
                {
                    button.interactable = true;
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                }
                else
                {
                    button.interactable = false;
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = button.colors.disabledColor;
                }
            }


            levels.gameObject.SetActive(true);
        }

        public void HideScreens()
        {
            foreach (Transform screen in screens)
            {
                screen.gameObject.SetActive(false);
            }
        }

        public void Explode()
        {
            Application.Quit();
        }
    }
}
