using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PianoTile : MonoBehaviour
{
    public string keyName;  // The name of the key
    public AudioClip keySound;  // The audio clip for this tile

    private AudioSource audioSource;

    private void Start()
    {
        InitializeAudioSource();
    }

    // Called in the editor when any variable is modified in the Inspector
    private void OnValidate()
    {
        InitializeAudioSource();
    }

    // Ensures that audioSource is assigned
    private void InitializeAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;  // Ensure it doesn't play on start
        }

        // Set the audio clip to the assigned key sound if it's not already set
        if (audioSource.clip != keySound)
        {
            audioSource.clip = keySound;
        }
    }

    public void PressTile()
    {
        Debug.Log("Tile Pressed!");
        Debug.Log("Key: " + keyName);

        // Play the audio clip if it¡¯s assigned
        if (keySound != null && audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned to the tile: " + keyName);
        }
    }
}
