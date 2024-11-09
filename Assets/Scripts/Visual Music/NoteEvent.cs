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

    public NoteEvent(string keyName, float pressTime, Color color, Vector3 _position, GameObject noteBubblePrefab)
    {
        this.keyName = keyName;
        this.pressTime = pressTime;
        this.releaseTime = -1f;


        // Find the parent GameObject with the VisualMusic script
        VisualMusic parentScript = GameObject.FindObjectOfType<VisualMusic>();
        if (parentScript == null)
        {
            Debug.LogError("No GameObject with VisualMusic script found.");
            return;
        }

        GameObject parent = parentScript.gameObject;

        // Instantiate the noteBubblePrefab under the parent
        noteBubble = GameObject.Instantiate(noteBubblePrefab, _position, Quaternion.identity, parent.transform);
        noteBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        noteBubble.GetComponent<Renderer>().material.color = color;
    }

    public void SetReleaseTime(float time)
    {
        releaseTime = time;
        Debug.Log(releaseTime - pressTime);
    }
}
