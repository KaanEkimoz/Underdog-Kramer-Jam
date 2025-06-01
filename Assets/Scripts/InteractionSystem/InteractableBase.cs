using com.absence.attributes;
using System.Text;
using System;
using UnityEngine;
//using com.game.miscs;

namespace com.game.interactionsystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject m_container;
        [SerializeField] private bool m_destroyOnInteraction = false;
        [SerializeField, Readonly] private bool m_hidden = false;

        protected virtual float DisposeDelay => 0f;

        public abstract bool Interactable { get; set; }
        public virtual bool Hidden
        {
            get
            {
                return m_hidden; 
            }

            set
            {
                m_hidden = value;
            }
        }
        public bool Disposed { get; protected set; }

        public event Action<IInteractable, IInteractor, bool> OnInteraction;
        public event Action<IInteractor, bool> OnSeen;
        public event Action<IInteractor, bool> OnPicked;
        public event Action<IInteractable> OnDispose;

        Action<StringBuilder> m_interactionMessagePipeline = null;
        Action<StringBuilder> m_interactionCallbackPopupPipeline = null;

        public bool Interact(IInteractor interactor)
        {
            if (!Interactable)
            {
                CastInteractionEvents(interactor, false);
                return false;
            }

            if (!OnInteract(interactor))
            {
                CastInteractionEvents(interactor, false);
                return false;
            }

            if (m_destroyOnInteraction)
                Dispose();

            CastInteractionEvents(interactor, true);
            return true;
        }

        public abstract bool OnInteract(IInteractor interactor);

        public virtual void CastInteractionEvents(IInteractor interactor, bool result)
        {
            OnInteraction?.Invoke(this, interactor, result);
        }

        public virtual void CommitPicked(IInteractor sender, bool picked)
        {
            OnPicked?.Invoke(sender, picked);
        }

        public virtual void CommitSeen(IInteractor sender, bool seen)
        {
            OnSeen?.Invoke(sender, seen);
        }

        public virtual string GenerateInteractionPopupText(IInteractor sender, bool success)
        {
            if (m_interactionCallbackPopupPipeline != null)
            {
                StringBuilder sb = new StringBuilder();
                m_interactionCallbackPopupPipeline.Invoke(sb);

                return sb.ToString().Trim();
            }

            return null;
        }

        public virtual void Dispose()
        {
            Disposed = true;
            OnDispose?.Invoke(this);

            if (m_container != null)
                Destroy(m_container, DisposeDelay);
            else
                Destroy(gameObject, DisposeDelay);
        }

        public void EnpipeInteractionMessage(Action<StringBuilder> builder)
        {
            m_interactionMessagePipeline += builder;
        }

        public void EnpipeInteractionCallbackPopup(Action<StringBuilder> builder)
        {
            m_interactionCallbackPopupPipeline += builder;
            m_interactionCallbackPopupPipeline += (sb) => sb.Append("\n\n");
        }

        [Button("Generate Description")]
        protected void PrintDescription()
        {
            Debug.Log(GetInteractionMessage(null, true, out _));
        }

        public virtual string GetInteractionMessage(IInteractor interactor, bool canInteract, out bool clearInteractorMessage)
        {
            clearInteractorMessage = false;
            if (m_interactionMessagePipeline != null)
            {
                StringBuilder sb = new StringBuilder();
                m_interactionMessagePipeline.Invoke(sb);

                return sb.ToString().Trim();
            }

            return null;
        }
    }
}
