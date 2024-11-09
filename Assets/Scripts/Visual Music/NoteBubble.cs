using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class NoteBubble : MonoBehaviour
{
    private NoteEvent noteEvent;   // Reference to the corresponding NoteEvent
    private float initialSize;     // Initial size to calculate intensity

    public void Initialize(NoteEvent noteEvent, float initialSize)
    {
        this.noteEvent = noteEvent;
        this.initialSize = initialSize;
    }

    private void Update()
    {
        // Calculate the current size relative to the initial size
        float currentSize = transform.localScale.x;
        float intensity = currentSize / initialSize;

        // Update the NoteEvent's intensity if it has changed
        if (!Mathf.Approximately(noteEvent.intensity, intensity))
        {
            noteEvent.UpdateIntensity(intensity);
        }
    }
}
