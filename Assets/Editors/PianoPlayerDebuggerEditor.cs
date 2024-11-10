using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PianoPlayerDebugger))]
public class PianoReplayerDebuggerEditor : Editor
{
    private List<PianoTile> pianoTiles = new List<PianoTile>();
    private PianoSequence sequence;

    private void OnEnable()
    {
        PianoPlayerDebugger replayer = (PianoPlayerDebugger)target;
        sequence = replayer.GetComponent<PianoSequence>();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PianoPlayerDebugger replayer = (PianoPlayerDebugger)target;

        // Button to find all PianoTile components in children
        if (GUILayout.Button("Find Piano Tiles"))
        {
            pianoTiles.Clear();
            pianoTiles.AddRange(replayer.GetComponentsInChildren<PianoTile>());
            Debug.Log("Piano Tiles Found: " + pianoTiles.Count);
        }

        // Display buttons for each tile
        foreach (var tile in pianoTiles)
        {
            if (tile != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Press " + tile.keyName)) tile.PressTile();
                if (GUILayout.Button("Release " + tile.keyName)) tile.ReleaseTile();
                EditorGUILayout.EndHorizontal();
            }
        }

        // Recording buttons
        if (GUILayout.Button("Start Recording")) sequence.StartRecording();
        if (GUILayout.Button("Stop Recording")) sequence.StopRecording();

        // Playback button
        if (GUILayout.Button("Start Playback")) sequence.StartPlayback(sequence);
    }
}
