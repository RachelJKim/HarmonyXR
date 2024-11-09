using UnityEngine;
using UnityEngine.Events;

public class PianoTile : MonoBehaviour
{
    public string keyName;


    private void Start()
    {
    }

    public void PressTile()
    {
        Debug.Log("Tile Pressed!");
        Debug.Log("Key: " + keyName);

        // (TODO: play the audio)
    }

}
