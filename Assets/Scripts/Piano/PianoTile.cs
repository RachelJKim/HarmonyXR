using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class PianoTile : MonoBehaviour
{
    public string keyName;
    public AudioClip keySound;
    public PianoSequence sequence; // Reference to the Sequence script

    private AudioSource audioSource;

    private void Start()
    {
        InitializeAudioSource();
        sequence = GetComponentInParent<PianoSequence>();

        if (sequence == null)
        {
            Debug.LogError("PianoSequence component not found in parent hierarchy. Ensure it's attached to a parent object.");
        }
    }

    private void OnValidate()
    {
        InitializeAudioSource();
    }

    private void InitializeAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        if (audioSource.clip != keySound)
        {
            audioSource.clip = keySound;
        }
    }

    private float IntensityToVolume(int intensity) {
        return Mathf.Clamp(intensity / 10f, 0f, 1f);
        // (float)(2.0 / (1.0 + Math.Exp(-2 * intensity)) - 1.0); // tanh
    }

    public void PressTile(int intensity=5)
    {
        Debug.Log("Tile Pressed!");
        Debug.Log("Key: " + keyName);

        if (keySound != null && audioSource != null)
        {
            audioSource.volume = IntensityToVolume(intensity);
            audioSource.Play();
            TriggerHapticFeedback(intensity);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned to the tile: " + keyName);
        }

        // Notify the sequence to record the note press
        sequence?.RecordNotePress(keyName);
    }

    public void ReleaseTile()
    {
        Debug.Log("Tile Released!");
        StopHapticFeedback();

        // Notify the sequence to record the note release
        sequence?.RecordNoteRelease(keyName);
    }

    private void TriggerHapticFeedback(int intensity)
    {
        // Use OVRInput to provide haptic feedback
        float vibrationIntensity = Mathf.Clamp(intensity / 10f, 0f, 1f); // Ensure intensity is between 0 and 1

        // You can adjust the controller to which you want to apply feedback (LTouch, RTouch)
        OVRInput.SetControllerVibration(vibrationIntensity, vibrationIntensity, OVRInput.Controller.RTouch); // Right controller
        OVRInput.SetControllerVibration(vibrationIntensity, vibrationIntensity, OVRInput.Controller.LTouch); // Left controller
    }

    private void StopHapticFeedback()
    {
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch); // Stop vibration on right controller
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch); // Stop vibration on left controller
    }
}
