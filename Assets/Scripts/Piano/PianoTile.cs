using Oculus.Interaction;
using UnityEngine;
using static Oculus.Interaction.InteractableColorVisual;
using Oculus.Haptics;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PianoTile : MonoBehaviour
{
    public string keyName;
    public AudioClip keySound;
    public HapticClip templateHaptic;
    public PianoSequence sequence; // Reference to the Sequence script
    public Color color = Color.black;
    public InteractableColorVisual colorVisual;

    private AudioSource audioSource;

    private HapticClipPlayer player;
    private Dictionary<char, float> noteToShift = new Dictionary<char, float>() {
        {'C', -.3f},
        {'D', -.2f},
        {'E', -.1f},
        {'F', 0f},
        {'G', .1f},
        {'A', .2f},
        {'B', .3f}
    };
    private float IntensityToVolume(float intensity) {
        return Mathf.Clamp(intensity, 0f, 1f);
    }

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
            audioSource.mute = false;
        }

        if (audioSource.clip != keySound)
        {
            audioSource.clip = keySound;
        }
    }

    private HapticClipPlayer InitializePlayer()
    {
        player = new HapticClipPlayer(templateHaptic);
        player.isLooping = true;
        player.frequencyShift = noteToShift[keyName[0]];
        return player;
    }

    public void PressTile(float intensity=0.5f)
    {
        Debug.Log("Tile Pressed!");
        Debug.Log("Key: " + keyName);

        if (keySound != null && audioSource != null)
        {
            //audioSource.volume = IntensityToVolume(intensity);

            audioSource.Play();

            HapticClipPlayer player = InitializePlayer();
            Debug.Log("WOW " + player.amplitude + " / " + player.clipDuration);
            Debug.Log("player initialized");
            //player.amplitude = IntensityToVolume(intensity);
            player.Play(Controller.Both);
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
        player.Stop();
        Debug.Log("Tile Released!");

        // Notify the sequence to record the note release
        sequence?.RecordNoteRelease(keyName);
    }

}
