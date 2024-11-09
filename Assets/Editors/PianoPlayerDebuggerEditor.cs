using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PianoPlayerDebugger))]
public class PianoReplayerDebuggerEditor : Editor
{
    // List to store references to PianoTile components
    private List<PianoTile> pianoTiles = new List<PianoTile>();

    public override void OnInspectorGUI()
    {
        // Draw the default inspector layout for other fields in the PianoPlayer
        DrawDefaultInspector();

        PianoPlayerDebugger replayer = (PianoPlayerDebugger)target;

        // Button to find all PianoTile components in the children of PianoPlayer
        if (GUILayout.Button("Find Piano Tiles"))
        {
            pianoTiles.Clear();
            pianoTiles.AddRange(replayer.GetComponentsInChildren<PianoTile>());
            Debug.Log("Piano Tiles Found: " + pianoTiles.Count);
        }

        // Display a button for each tile in the list
        foreach (var tile in pianoTiles)
        {
            if (tile != null)
            {
                if (GUILayout.Button("Play " + tile.keyName))
                {
                    tile.PressTile(); // Play sound and print debug log for this tile
                }
            }
        }

        // Button to play all tiles in the list
        if (GUILayout.Button("Play All Tiles"))
        {
            foreach (var tile in pianoTiles)
            {
                if (tile != null)
                {
                    tile.PressTile(); // Play sound and print debug log for each tile
                }
            }
        }
    }
}
