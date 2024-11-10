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
    public Transform referencePoint;

    public GameObject noteBubblePrefab;
    public GameObject visualMusicPrefab;
    public float spacingMultiplier = 2.0f;
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
        // Calculate the target world position with an offset from the current PianoSequence position
        //Vector3 targetWorldPosition = transform.position + worldPositionOffset;

        // Instantiate a new VisualMusic container at the reference position and rotation of the reference point   

        GameObject newContainer = Instantiate(visualMusicPrefab, referencePoint.position, referencePoint.rotation);
        PianoSequence targetSequence = newContainer.GetComponent<PianoSequence>();

        // Clear the target sequence before copying
        targetSequence.notes.Clear();

        foreach (var note in notes)
        {
            // Create a new NoteEvent for the target sequence
            NoteEvent copiedNote = new NoteEvent(
                note.keyName,
                note.pressTime,
                note.color,
                spacingMultiplier,// * scaleMultiplier,
                noteBubblePrefab,
                newContainer.transform,
                lineY,// * scaleMultiplier,
                lineZ// * scaleMultiplier
            );

            copiedNote.StopParticleEffect();
            // Adjust copied NoteBubble scale based on the scaleMultiplier
            copiedNote.noteBubble.transform.localScale = note.noteBubble.transform.localScale;

            // Position each note relative to the new container’s local space
            copiedNote.noteBubble.transform.localPosition = note.noteBubble.transform.localPosition;

            // Add the copied note to the target sequence
            targetSequence.notes.Add(copiedNote);
            //now destroy the note bubble
            Destroy(note.noteBubble.gameObject);
        }
        
        newContainer.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }


    public void StartPlayback(PianoSequence targetSequence)
    {
        if (playbackCoroutine != null) StopCoroutine(playbackCoroutine);

        Debug.Log("StartPlayback.");
        playbackCoroutine = StartCoroutine(PlaybackCoroutine(targetSequence));
    }

    private IEnumerator PlaybackCoroutine(PianoSequence targetSequence)
    {
        Debug.Log("PlayBackCoroutine");
        float playbackStartTime = Time.time;

        foreach (var note in notes)
        {
            yield return new WaitForSeconds(note.pressTime - (Time.time - playbackStartTime));

            // Trigger the particle effect and play sound directly from the NoteBubble
            note.StartParticleEffect();

            AudioSource noteAudioSource = note.noteBubble.GetComponent<AudioSource>();
            if (noteAudioSource != null)
            {
                noteAudioSource.Play();
            }

            yield return new WaitForSeconds(note.releaseTime - note.pressTime);

            // Stop the particle effect after the release time
            note.StopParticleEffect();
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
