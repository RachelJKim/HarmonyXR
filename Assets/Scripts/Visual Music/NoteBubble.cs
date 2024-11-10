using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class NoteBubble : MonoBehaviour
{
    private NoteEvent noteEvent;   // Reference to the corresponding NoteEvent
    private float initialSize;     // Initial size to calculate intensity
    private ParticleSystem particleSystem;

    public void Initialize(NoteEvent noteEvent, float initialSize, AudioSource sourceAudioSource)
    {
        this.noteEvent = noteEvent;
        this.initialSize = initialSize;
        this.gameObject.transform.localScale = new Vector3(initialSize, initialSize, initialSize);
        particleSystem = GetComponentInChildren<ParticleSystem>();

        AudioSource bubbleAudioSource = gameObject.GetComponent<AudioSource>();
        bubbleAudioSource.clip = sourceAudioSource.clip;
        bubbleAudioSource.volume = sourceAudioSource.volume;
        bubbleAudioSource.pitch = sourceAudioSource.pitch;
    }

    private void Update()
    {
        // Calculate the current size relative to the initial size
        float currentSize = transform.localScale.x;
        float intensity = currentSize / initialSize;

        // Update the NoteEvent's intensity if it has changed
        if (!Mathf.Approximately(currentSize, initialSize))
        {
            noteEvent.UpdateIntensity(intensity);
        }
    }

    public void setParticleEffectColor(Color color)
    {
        this.particleSystem.startColor = color;
    }

    public void EnableParticleEffect()
    {
        if (particleSystem != null && !particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }

    // Method to disable the particle effect
    public void DisableParticleEffect()
    {
        if (particleSystem != null && particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }
}
