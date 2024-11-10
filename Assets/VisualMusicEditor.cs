//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(VisualMusic))]
//public class VisualMusicEditor : Editor
//{
//    private PianoSequence sequence;

//    private void OnEnable()
//    {
//        VisualMusic vmusic = (VisualMusic)target;
//        try { sequence = vmusic.GetComponent<PianoSequence>(); }
//        catch { sequence = vmusic.transform.parent.GetComponentInChildren<PianoSequence>(); }
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        VisualMusic vmusic = (VisualMusic)target;

//        if (GUILayout.Button("Start Playback")) sequence.StartPlayback(sequence);
//    }
//}
