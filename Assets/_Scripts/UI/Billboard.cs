using UnityEngine;

namespace Scripts.UI
{
    public class Billboard : MonoBehaviour
    {
        private Transform camTransform;

        void Start()
        {
            camTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.rotation = camTransform.rotation;
        }
    }
}
