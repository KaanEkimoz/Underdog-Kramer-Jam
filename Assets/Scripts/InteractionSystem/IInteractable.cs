using com.game.utilities;
using System;

namespace com.game.interactionsystem
{
    public interface IInteractable : IObject, IDisposable
    {
        bool Interactable { get; }
        bool Hidden { get; }
        bool Disposed { get; }

        event Action<IInteractable, IInteractor, bool> OnInteraction;
        event Action<IInteractor, bool> OnSeen;
        event Action<IInteractable> OnDispose;

        bool Interact(IInteractor interactor);
        void CommitSeen(IInteractor sender, bool seen);
        string GetInteractionMessage(IInteractor interactor, bool canInteract, out bool clearInteractorMessage);
    }
}
