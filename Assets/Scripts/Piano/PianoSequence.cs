using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NoteEvent
{
    public string keyName;
    public float pressTime;
    public float releaseTime;
    public int intensity;

    public NoteEvent(string keyName, float pressTime)
    {
        this.keyName = keyName;
        this.pressTime = pressTime;
        this.releaseTime = -1f;
        this.intensity = 10;
    }

    public void SetReleaseTime(float time)
    {
        releaseTime = time;
    }
}

public class PianoSequence : MonoBehaviour
{
    public List<NoteEvent> notes = new List<NoteEvent>();
    private bool isRecording = false;
    private float startTime;
    private Coroutine playbackCoroutine;

    // Start recording by clearing notes and capturing start time
    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
        notes.Clear();
        Debug.Log("Recording started.");
    }

    // Stop recording
    public void StopRecording()
    {
        isRecording = false;
        Debug.Log("Recording stopped. Total notes recorded: " + notes.Count);
    }

    // Add a new note event for press
    public void RecordNotePress(string keyName)
    {
        if (isRecording)
        {
            float pressTime = Time.time - startTime;
            NoteEvent noteEvent = new NoteEvent(keyName, pressTime);
            notes.Add(noteEvent);
            Debug.Log($"Note {keyName} pressed at {pressTime} seconds.");
        }
    }

    // Update release time for a note
    public void RecordNoteRelease(string keyName)
    {
        if (isRecording)
        {
            NoteEvent noteEvent = notes.FindLast(note => note.keyName == keyName && note.releaseTime == -1f);
            if (noteEvent != null)
            {
                float releaseTime = Time.time - startTime;
                noteEvent.SetReleaseTime(releaseTime);
                Debug.Log($"Note {keyName} released at {releaseTime} seconds.");
            }
        }
    }

    // Start playback coroutine
    public void StartPlayback()
    {
        if (playbackCoroutine != null) StopCoroutine(playbackCoroutine);
        playbackCoroutine = StartCoroutine(PlaybackCoroutine());
    }

    // Playback coroutine to play notes based on timing
    private IEnumerator PlaybackCoroutine()
    {
        foreach (var note in notes)
        {
            yield return new WaitForSeconds(note.pressTime - (Time.time - startTime));
            PianoTile tile = FindTileByName(note.keyName);
            if (tile != null)
            {
                tile.PressTile(note.intensity);
                yield return new WaitForSeconds(note.releaseTime - note.pressTime);
                tile.ReleaseTile();
            }
        }
    }

    // Find PianoTile by key name
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
