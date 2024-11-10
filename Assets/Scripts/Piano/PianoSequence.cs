using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PianoSequence : MonoBehaviour
{
    public List<NoteEvent> notes = new List<NoteEvent>();
    private bool isRecording = false;
    private float startTime;
    private Coroutine playbackCoroutine;
    private bool isPlaying = false;

    public GameObject noteBubblePrefab;
    public float spacingMultiplier = 0.5f;
    public float lineY = 0f;
    public float lineZ = 0f;

    public VisualMusic container;

    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
        notes.Clear();
        Debug.Log("Recording started.");
    }

    public void StopRecording()
    {
        isRecording = false;
        Debug.Log("Recording stopped. Total notes recorded: " + notes.Count);
    }

    public void RecordNotePress(string keyName, Color color, bool isPlayback=false)
    {
        if (isRecording)
        {
            float pressTime = Time.time - startTime;


            //GameObject musicVisual = GameObject.FindAnyObjectByType<VisualMusic>().gameObject;
            GameObject targetVisual = this.container.gameObject;

            // Create a new NoteEvent with the calculated position and reference to NoteBubble prefab
            NoteEvent noteEvent = new NoteEvent(keyName, pressTime, color, spacingMultiplier, noteBubblePrefab, targetVisual.transform, lineY, lineZ);
            notes.Add(noteEvent);

            // Start particle effect
            noteEvent.StartParticleEffect();

            Debug.Log($"Note {keyName} pressed at {pressTime} seconds.");
        }
    }

    public void RecordNoteRelease(string keyName)
    {
        if (isRecording)
        {
            NoteEvent noteEvent = notes.FindLast(note => note.keyName == keyName && note.releaseTime == -1f);
            if (noteEvent != null)
            {
                float releaseTime = Time.time - startTime;
                noteEvent.SetReleaseTime(releaseTime);

                // Stop particle effect
                noteEvent.StopParticleEffect();

                Debug.Log($"Note {keyName} released at {releaseTime} seconds.");
            }
        }
    }

    public void CopySequenceToWorld(float scaleMultiplier)
    {
        PianoSequence targetSequence = GameObject.Find("Visual Music").GetComponent<PianoSequence>();

        // Clear target sequence
        targetSequence.notes.Clear();

        foreach (var note in notes)
        {
            // Create a new NoteEvent for the target sequence
            NoteEvent copiedNote = new NoteEvent(note.keyName, note.pressTime, note.color, spacingMultiplier * scaleMultiplier,
                noteBubblePrefab, targetSequence.transform, lineY * scaleMultiplier, lineZ * scaleMultiplier);

            // Adjust copied NoteBubble scale and position based on the scaleMultiplier and targetPositionOffset
            copiedNote.noteBubble.transform.localScale = note.noteBubble.transform.localScale * scaleMultiplier;
            copiedNote.noteBubble.transform.position = note.noteBubble.transform.position * scaleMultiplier;

            // Add copied note to the target sequence
            targetSequence.notes.Add(copiedNote);
        }
    }
    public void StartPlayback(PianoSequence targetSequence)
    {
        if (playbackCoroutine != null) StopCoroutine(playbackCoroutine);
        playbackCoroutine = StartCoroutine(PlaybackCoroutine(targetSequence));
    }

    private IEnumerator PlaybackCoroutine(PianoSequence targetSequence)
    {
        float playbackStartTime = Time.time;

        foreach (var note in notes)
        {
            // Wait until the correct time to play this note
            yield return new WaitForSeconds(note.pressTime - (Time.time - playbackStartTime));

            PianoTile tile = FindTileByName(note.keyName);
            if (tile != null)
            {
                // Simulate the press action and start particle effect
                tile.PressTile();
                note.StartParticleEffect();

                yield return new WaitForSeconds(note.releaseTime - note.pressTime); // Wait for the duration

                // Simulate the release action and stop particle effect
                tile.ReleaseTile();
                note.StopParticleEffect();
            }
        }
    }



    private PianoTile FindTileByName(string keyName)
    {
        foreach (var tile in GetComponentsInChildren<PianoTile>())
        {
            if (tile.keyName == keyName)
            {
                return tile;
            }
        }
        return null;
    }
}
