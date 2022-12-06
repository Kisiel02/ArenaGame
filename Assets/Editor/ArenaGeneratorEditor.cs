using System;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor (typeof(ArenaGenerator))]
    public class ArenaGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ArenaGenerator arena = (ArenaGenerator)target;
            
            if (GUILayout.Button("Generate"))
            {
                arena.mapGenerator.GenerateMap();
            }
            
            if (GUILayout.Button("Generate Random"))
            {
                arena.GenerateRandomArena();
            }
            
            if (GUILayout.Button("Spawnpoints"))
            {
                arena.ReplaceSpawns();
            }

            if (GUILayout.Button("GenerateArena")) {
                arena.SpawnMountains();
            }
        }
    }
}