using com.absence.attributes;
using com.game.interactionsystem;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace com.game.generics
{
    public class InteractableCanvasHelper : MonoBehaviour
    {
        [SerializeField] private bool m_hideIfNotInteractable = false;
        [SerializeField, Required] private Canvas m_canvas;
        [SerializeField, Required] private TMP_Text m_interactionText;

        IInteractable m_interactable;
        IInteractor m_player;
        bool m_seen;

        private void Awake()
        {
            SetCanvasVisibility(false);

            m_interactable = GetComponent<IInteractable>();
            m_interactable.OnSeen += OnSeen;
        }

        private void Update()
        {
            m_canvas.gameObject.SetActive((!m_interactable.Hidden) && m_seen);

            if (m_player == null)
                return;

            StringBuilder sb = new();

            string interactableMessage = m_interactable.GetInteractionMessage(m_player, m_interactable.Interactable, 
                out bool clearInteractorMessage);

            if (!clearInteractorMessage)
            {
                sb.Append(m_player.GenerateInteractorMessage(m_interactable));
                sb.Append("\n\n");
            }

            sb.Append(interactableMessage);

            m_interactionText.text = sb.ToString().Trim();
        }

        private void OnSeen(IInteractor interactor, bool seen)
        {
            if (!interactor.IsPlayer)
                return;

            m_player = interactor;
            m_seen = seen;
            if (m_hideIfNotInteractable) SetCanvasVisibility(seen && m_interactable.Interactable);
            else SetCanvasVisibility(seen);
        }

        private void SetCanvasVisibility(bool visibility)
        {
            m_canvas.gameObject.SetActive(visibility);
        }
    }
}
