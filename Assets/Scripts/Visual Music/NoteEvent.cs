using UnityEngine;

[System.Serializable]
public class NoteEvent
{
    public string keyName;
    public float pressTime;
    public float releaseTime;
    public float intensity; // Intensity based on bubble size
    public NoteBubble noteBubble; // Reference to the corresponding NoteBubble script

    public NoteEvent(string keyName, float pressTime, Color color, float spacingMultiplier, GameObject noteBubblePrefab, Transform parent, float lineY, float lineZ)
    {
        this.keyName = keyName;
        this.pressTime = pressTime;
        this.releaseTime = -1f;
        this.intensity = 1f; // Default intensity is 1 (based on initial size)

        // Calculate position based on pressTime and spacingMultiplier
        float xPosition = pressTime * spacingMultiplier;
        Vector3 position = new Vector3(xPosition, lineY, lineZ);


        // Instantiate the noteBubble prefab under the specified parent
        GameObject noteBubbleObject = GameObject.Instantiate(noteBubblePrefab, position, Quaternion.identity, parent);
        //noteBubbleObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Initial size
        noteBubbleObject.GetComponent<NoteBubble>().Initialize(this, 0.1f);

        // Set color and initialize the NoteBubble script
        Renderer renderer = noteBubbleObject.GetComponent<Renderer>();
        renderer.material.color = color;

        noteBubble = noteBubbleObject.AddComponent<NoteBubble>(); // Add NoteBubble script
        noteBubble.Initialize(this, 0.1f); // Link NoteEvent to NoteBubble and set initial size
    }

    public void SetReleaseTime(float time)
    {
        releaseTime = time;
        Debug.Log("Duration of note " + keyName + ": " + (releaseTime - pressTime) + " seconds.");
    }

    // Update the intensity based on size change
    public void UpdateIntensity(float newIntensity)
    {
        intensity = newIntensity;
        Debug.Log($"Updated intensity of {keyName} to {intensity}");
    }
}
