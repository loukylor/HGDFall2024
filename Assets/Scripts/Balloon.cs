using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024
{
    public class Balloon : MonoBehaviour
    {
        public GameObject popAnim;

        protected virtual void OnDestroy()
        {
            if (ApplicationManager.Instance.HasQuit)
            {
                return;
            }

            GameObject anim = Instantiate(popAnim);
            anim.transform.position = transform.position;
        }
    }
}
