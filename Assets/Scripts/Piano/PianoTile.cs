using Oculus.Interaction;
using UnityEngine;
using static Oculus.Interaction.InteractableColorVisual;

[RequireComponent(typeof(AudioSource))]
public class PianoTile : MonoBehaviour
{
    public string keyName;
    public AudioClip keySound;
    public PianoSequence sequence; // Reference to the Sequence script
    public Color color = Color.black;
    public InteractableColorVisual colorVisual;

    private AudioSource audioSource;

    private void Start()
    {
        InitializeAudioSource();
        sequence = GetComponentInParent<PianoSequence>();

        if (sequence == null)
        {
            Debug.LogError("PianoSequence component not found in parent hierarchy. Ensure it's attached to a parent object.");
        }
        ColorState colorState = new ColorState() { Color = color };
        colorVisual.InjectOptionalNormalColorState(colorState);
        colorVisual.InjectOptionalHoverColorState(colorState);
        colorVisual.InjectOptionalSelectColorState(colorState);
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

    public void PressTile()
    {
        Debug.Log("Tile Pressed!");
        Debug.Log("Key: " + keyName);

        if (keySound != null && audioSource != null)
        {
            Debug.Log("HERE4000");
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned to the tile: " + keyName);
        }

        // Notify the sequence to record the note press
        sequence?.RecordNotePress(keyName, color);
    }

    public void ReleaseTile()
    {
        Debug.Log("Tile Released!");

        // Notify the sequence to record the note release
        sequence?.RecordNoteRelease(keyName);
    }

}
