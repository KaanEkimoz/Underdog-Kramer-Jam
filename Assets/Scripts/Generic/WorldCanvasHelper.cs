using UnityEngine;

namespace com.game.generics
{
    public class WorldCanvasHelper : MonoBehaviour
    {
        [SerializeField] private bool m_refreshInUpdate;

        private void Start()
        {
            LookAtCamera();
        }

        private void Update()
        {
            if (!m_refreshInUpdate)
                return;

            LookAtCamera();
        }

        void LookAtCamera()
        {
            transform.forward = (Camera.main.transform.position - transform.position).normalized;
        }
    }
}
