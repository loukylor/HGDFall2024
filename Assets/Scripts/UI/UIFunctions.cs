using HGDFall2024.Managers;
using System.Threading;
using UnityEngine;

namespace HGDFall2024.UI
{
    public class UIFunctions : MonoBehaviour
    {
        private Transform screens;

        public void StartClick()
        {
            ApplicationManager.Instance.LoadLevel(1);

            screens = GameObject.Find("Canvas").transform;
            ShowMainMenuScreen();
        }

        public void LoadLevel(int level)
        {
            ApplicationManager.Instance.LoadLevel(level);
        }

        public void ShowMainMenuScreen()
        {
            HideScreens();

            Transform mainMenu = screens.Find("MainMenu");

            //Button select = mainMenu.Find("Buttons/Select").GetComponent<Button>();
            //select.interactable = ProgressManager.Instance.AvailableLevels != 0;

            mainMenu.gameObject.SetActive(true);
        }

        public void ShowLevelsScreen()
        {
            HideScreens();

            
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
