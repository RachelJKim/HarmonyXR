using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class NoteEvent
{
    public string keyName;
    public float pressTime;
    public float releaseTime;
    public GameObject noteBubble;

     public NoteEvent(string keyName, float pressTime, Color color, float spacingMultiplier, GameObject noteBubblePrefab, Transform parent, float lineY, float lineZ)
    {
        this.keyName = keyName;
        this.pressTime = pressTime;
        this.releaseTime = -1f;

        // Calculate position based on pressTime and spacingMultiplier
        float xPosition = pressTime * spacingMultiplier;
        Vector3 position = new Vector3(xPosition, lineY, lineZ);  // Place on the predefined line at y = lineY, z = lineZ

        // Find the parent GameObject with the VisualMusic script
        VisualMusic parentScript = GameObject.FindObjectOfType<VisualMusic>();
        if (parentScript == null)
        {
            Debug.LogError("No GameObject with VisualMusic script found.");
            return;
        }

        // Instantiate the noteBubble prefab under the specified parent
        noteBubble = GameObject.Instantiate(noteBubblePrefab, position, Quaternion.identity, parentScript.transform);
        noteBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        noteBubble.GetComponent<Renderer>().material.color = color;
    }


    public void SetReleaseTime(float time)
    {
        releaseTime = time;
        Debug.Log(releaseTime - pressTime);
    }
}
