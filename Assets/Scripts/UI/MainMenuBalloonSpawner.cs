using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HGDFall2024.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class MainMenuBalloonSpawner : MonoBehaviour
    {
        public Balloon balloon;
        public float spawnDelay = 0.25f;
        public int spawnCount = 2;

        private void Start()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            float screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
            float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;


            while (true)
            {
                yield return new WaitForSeconds(spawnDelay);

                for (int i = 0; i < spawnCount; i++)
                {
                    GameObject newBalloon = Instantiate(balloon.gameObject);
                    newBalloon.transform.position = new Vector3(
                        Random.Range(screenLeft, screenRight),
                        -6
                    );
                    newBalloon.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out Balloon balloon))
            {
                return;
            }

            balloon.disableAnim = true;
            Destroy(balloon.gameObject);
        }
    }
}
