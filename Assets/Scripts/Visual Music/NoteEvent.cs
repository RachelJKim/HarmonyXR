using UnityEngine;

[System.Serializable]
public class NoteEvent
{
    public string keyName;
    public float pressTime;
    public float releaseTime;
    public float intensity; // Intensity based on bubble size
    public NoteBubble noteBubble; // Reference to the corresponding NoteBubble script
    public Color color;

    public NoteEvent(string keyName, float pressTime, Color color, float spacingMultiplier, GameObject noteBubblePrefab, Transform parent, float lineY, float lineZ, AudioSource audioSource, float intensity)
    {
        this.keyName = keyName;
        this.pressTime = pressTime;
        this.releaseTime = -1f;
        this.intensity = intensity; 
        this.color = color;

        // Calculate position based on pressTime and spacingMultiplier
        float xPosition = pressTime * spacingMultiplier;
        Vector3 position = new Vector3(xPosition, lineY, lineZ);


        // Instantiate the noteBubble prefab under the specified parent
        //GameObject noteBubbleObject = GameObject.Instantiate(noteBubblePrefab, position, Quaternion.identity, parent);
        GameObject noteBubbleObject = GameObject.Instantiate(noteBubblePrefab, parent);
        noteBubbleObject.transform.localPosition = position;

        //noteBubbleObject.GetComponent<NoteBubble>().Initialize(this, 0.01f);

        // Set color and initialize the NoteBubble script
        Renderer renderer = noteBubbleObject.GetComponent<Renderer>();
        renderer.material.color = color;

        noteBubble = noteBubbleObject.GetComponent<NoteBubble>(); // Add NoteBubble script
        //noteBubble.Initialize(this, 0.5f); // Link NoteEvent to NoteBubble and set initial size


        noteBubble.Initialize(this, 0.2f, audioSource);

        noteBubble.setParticleEffectColor(color); // set the particle effect color the same as the note
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
        noteBubble.GetComponent<AudioSource>().volume = intensity;
        Debug.Log($"Updated intensity of {keyName} to {intensity}");
    }

    public void StartParticleEffect()
    {
        noteBubble?.EnableParticleEffect();
    }

    // Stop the particle effect when the note is released
    public void StopParticleEffect()
    {
        noteBubble?.DisableParticleEffect();
    }
}
