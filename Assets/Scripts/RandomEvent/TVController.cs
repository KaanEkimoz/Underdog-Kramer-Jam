using System;
using com.game.interactionsystem;
using UnityEngine;
using com.absence.timersystem;
using com.absence.soundsystem;
using com.absence.soundsystem.internals;

public class TVController : MonoBehaviour, IInteractable
{
    public Renderer tvRenderer;
    public int materialIndex;
    public Material openMaterial;
    public Material closedMaterial;

    public AudioSource tvAudio;

    public float tvOpenDuration = 10f;
    public float tvCooldownDuration = 10f;

    [SerializeField]private SoundAsset soundAsset;
    private bool isOpen = false;
    private bool inCooldown = false;

    public Transform tvFrontPoint;

    public bool Interactable => (!isOpen) && (!inCooldown);
    public bool Hidden { get; set; } = false;
    public bool Disposed { get; private set; } = false;

    public event Action<IInteractable, IInteractor, bool> OnInteraction;
    public event Action<IInteractor, bool> OnSeen;
    public event Action<IInteractable> OnDispose;

    ITimer m_cooldownTimer;
    ITimer m_tvTimer;

    private void Start()
    {
        CloseTV();      
    }

    public void CloseTV()
    {
        if (!isOpen)
            return;

        m_tvTimer?.Fail();

        tvAudio.Stop();
        Material[] tvMaterials = tvRenderer.materials;
        tvMaterials[materialIndex] = closedMaterial;
        tvRenderer.materials = tvMaterials;
        isOpen = false;
    }

    public void OpenTV()
    {
        if (isOpen)
            return;

        if (inCooldown) 
            return;

        inCooldown = true;
        m_cooldownTimer = Timer.Create(tvCooldownDuration)
            .OnComplete(OnCooldownTimerComplete);

        m_tvTimer = Timer.Create(tvOpenDuration)
            .OnComplete(OnTimerComplete);

        tvAudio.SetupWithData(soundAsset.GetData());
        tvAudio.Play();

        Material[] tvMaterials = tvRenderer.materials;
        tvMaterials[materialIndex] = openMaterial;
        tvRenderer.materials = tvMaterials;
        isOpen = true;
    }

    private void OnTimerComplete(TimerCompletionContext context)
    {
        m_tvTimer = null;

        CloseTV();
    }

    private void OnCooldownTimerComplete(TimerCompletionContext context)
    {
        m_cooldownTimer = null;
        inCooldown = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool Interact(IInteractor interactor)
    {
        bool result = true;

        if (!Interactable)
            result = false;

        OpenTV();

        OnInteraction.Invoke(this, interactor, result);

        return result;
    }

    public void CommitSeen(IInteractor sender, bool seen)
    {
        OnSeen?.Invoke(sender, seen);
    }

    public void Dispose()
    {
        if (Disposed)
            return;

        Disposed = true;
        OnDispose?.Invoke(this);
    }

    public string GetInteractionMessage(IInteractor interactor, bool canInteract, out bool clearInteractorMessage)
    {
        clearInteractorMessage = !canInteract;

        if (canInteract)
            return "Turn on";

        return $"In Cooldown: {m_cooldownTimer.CurrentTime.ToString("0")}s";
    }
}
