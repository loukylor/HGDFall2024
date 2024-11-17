using UnityEngine;

namespace HGDFall2024.CameraUtils
{
    public class CameraTargetChanger : MonoBehaviour
    {
        public Transform newTarget;
        public float newSize;

        private Transform oldTarget;
        private float oldSize;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ChangeTarget(Camera.main.gameObject, false);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ChangeTarget(Camera.main.gameObject, true);
            }
        }

        private void ChangeTarget(GameObject go, bool revert)
        {
            if (!go.TryGetComponent(out FollowCamera cam))
            {
                return;
            }

            cam.GetComponent<Collider2D>().enabled = revert;

            if (revert)
            {
                cam.followObject = oldTarget;
                cam.targetSize = oldSize;
            }
            else
            {
                oldTarget = cam.followObject;
                cam.followObject = newTarget;

                oldSize = cam.targetSize;
                if (newSize > 0)
                {
                    cam.targetSize = newSize;
                }
            }
        }
    }
}
