using System;
using com.absence.attributes;
using com.game.interactionsystem;
using UnityEngine;

namespace com.game.player
{
    public class PlayerInteractor : MonoBehaviour, IInteractor
    {
        [SerializeField, Required] private Transform m_rayOrigin;
        [SerializeField] private LayerMask m_interactableMask;
        [SerializeField] private float m_range;
        [SerializeField] private float m_errorRadius;

        IInteractable m_lastInteractable;

        public bool IsPlayer => true;

        public string GenerateInteractorMessage(IInteractable interactable)
        {
            return string.Empty;
        }

        private void FixedUpdate()
        {
            bool hit = Physics.SphereCast(m_rayOrigin.position, m_errorRadius, m_rayOrigin.forward, out RaycastHit info, m_range, 
                m_interactableMask, QueryTriggerInteraction.UseGlobal);

            if (!hit)
            {
                m_lastInteractable?.CommitSeen(this, false);
                ClearInteractable();
                return;
            }

            if (!info.rigidbody.TryGetComponent(out IInteractable interactable))
            {
                m_lastInteractable?.CommitSeen(this, false);
                ClearInteractable();
                return;
            }

            m_lastInteractable?.CommitSeen(this, false);
            m_lastInteractable = interactable;
            m_lastInteractable.CommitSeen(this, true);
        }

        private void Update()
        {
            if (m_lastInteractable == null || m_lastInteractable.Disposed)
                return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                Interact();
        }

        private void LateUpdate()
        {
            if (m_lastInteractable != null && m_lastInteractable.Disposed)
                m_lastInteractable = null;
        }

        private void OnDrawGizmosSelected()
        {
            if (m_rayOrigin == null)
                return;

            Gizmos.color = Color.red;
            if (m_lastInteractable == null || m_lastInteractable.Disposed)
            {
                Vector3 endPosition = m_rayOrigin.position + (m_rayOrigin.forward * m_range);
                Gizmos.DrawLine(m_rayOrigin.position, endPosition);
                Gizmos.DrawWireSphere(endPosition, m_errorRadius);
                return;
            }

            Gizmos.DrawLine(m_rayOrigin.position, m_lastInteractable.transform.position);
            Gizmos.DrawWireSphere(m_rayOrigin.position, m_errorRadius);
        }

        void Interact()
        {
            m_lastInteractable.Interact(this);
        }

        void ClearInteractable()
        {
            m_lastInteractable = null;
        }
    }
}