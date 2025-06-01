using System;
using com.absence.attributes;
using com.game.interactionsystem;
using UnityEngine;

namespace com.game.generics
{
    public class MonitorController : MonoBehaviour, IInteractable
    {
        [SerializeField, Required] private GameObject m_panel;

        public bool Interactable => !Game.InMenus;

        public bool Hidden { get; set; }

        public bool Disposed { get; private set; }

        public event Action<IInteractable, IInteractor, bool> OnInteraction;
        public event Action<IInteractor, bool> OnSeen;
        public event Action<IInteractable> OnDispose;

        public void CommitSeen(IInteractor sender, bool seen)
        {
            OnSeen?.Invoke(sender, seen);
        }

        public void Dispose()
        {
            Disposed = true;
            OnDispose?.Invoke(this);
        }

        public string GetInteractionMessage(IInteractor interactor, bool canInteract, out bool clearInteractorMessage)
        {
            clearInteractorMessage = true;

            if (canInteract)
                return "Open Monitor";

            return string.Empty;
        }

        public bool Interact(IInteractor interactor)
        {
            bool success = true;

            Game.InMenus = true;
            m_panel.SetActive(true);

            OnInteraction?.Invoke(this, interactor, success);
            return success;
        }
    }
}