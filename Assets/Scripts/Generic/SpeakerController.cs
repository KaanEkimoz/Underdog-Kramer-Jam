using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.absence.soundsystem;
using com.absence.soundsystem.internals;
using com.absence.timersystem;
using com.game.interactionsystem;
using UnityEngine;

public class SpeakerController : MonoBehaviour, IInteractable
{
    [SerializeField] private List<AudioSource> m_audioSources;
    [SerializeField] private SoundAsset m_soundAsset;
    [SerializeField] private AudioClip m_beepClip;

    [SerializeField] private float m_delay;
    [SerializeField] private float m_cooldownDuration;

    bool m_isPlaying;
    bool m_isBeeping;
    bool m_inCooldown;
    ITimer m_delayTimer;
    ITimer m_cooldownTimer;

    public event Action<IInteractable, IInteractor, bool> OnInteraction;
    public event Action<IInteractor, bool> OnSeen;
    public event Action<IInteractable> OnDispose;

    public bool Interactable => (!m_inCooldown) && (!m_isPlaying);

    public bool Hidden {  get; set; }

    public bool Disposed { get; private set; }

    public void Play()
    {
        if (m_isPlaying)
            return;

        if (m_inCooldown)
            return;

        m_cooldownTimer?.Fail();
        m_cooldownTimer = Timer.Create(m_cooldownDuration)
            .OnComplete(OnCooldownTimerComplete);

        m_isPlaying = true;
        m_inCooldown = true;

        m_audioSources.ForEach(source => source.clip = m_beepClip);
        m_audioSources.ForEach(source => source.Play());

        m_isBeeping = true;
        StartCoroutine(C_WaitUntilAudioSourcesEnd());
    }

    private IEnumerator C_WaitUntilAudioSourcesEnd()
    {
        yield return new WaitUntil(() => m_audioSources.All(source => (!source.isPlaying) && (!source.loop)));

        if (m_isBeeping)
        {
            m_delayTimer = Timer.Create(m_delay).OnComplete(OnDelayTimerComplete);
            m_isBeeping = false;
        }

        else
        {
            m_isPlaying = false;
        }
    }

    private void OnDelayTimerComplete(TimerCompletionContext context)
    {
        m_delayTimer = null;
        m_isBeeping = false;

        m_audioSources.ForEach(source => source.SetupWithData(m_soundAsset.GetData()));
        m_audioSources.ForEach(source => source.Play());

        StartCoroutine(C_WaitUntilAudioSourcesEnd());
    }

    public void Stop()
    {
        if (!m_isPlaying)
            return;

        m_cooldownTimer?.Fail();
        m_delayTimer?.Fail();
    }

    private void OnCooldownTimerComplete(TimerCompletionContext context)
    {
        m_cooldownTimer = null;
        m_inCooldown = false;

        m_audioSources.ForEach(source => source.Stop());
    }

    public bool Interact(IInteractor interactor)
    {
        bool result = true;

        Play();

        OnInteraction?.Invoke(this, interactor, result);
        return result;
    }

    public void CommitSeen(IInteractor sender, bool seen)
    {
        OnSeen?.Invoke(sender, seen);
    }

    public string GetInteractionMessage(IInteractor interactor, bool canInteract, out bool clearInteractorMessage)
    {
        clearInteractorMessage = !canInteract;

        if (canInteract)
            return "Talk to speakers";
        else
            return $"In Cooldown: {m_cooldownTimer.CurrentTime.ToString("0")}s";
    }

    public void Dispose()
    {
        Disposed = true;
        OnDispose?.Invoke(this);
    }
}
