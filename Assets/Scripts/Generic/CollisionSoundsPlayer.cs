using com.absence.attributes;
using com.absence.soundsystem;
using UnityEngine;

public class CollisionSoundsPlayer : MonoBehaviour
{
    [SerializeField, Required] private Rigidbody m_rigidbody;
    [SerializeField, Required] private SoundAsset m_soundAsset;

    public bool Enabled { get; set; } = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (m_rigidbody.isKinematic)
            return;

        if (!Enabled)
            return;

        Sound.Create(m_soundAsset).AtPosition(collision.GetContact(0).point).Play();
    }
}
