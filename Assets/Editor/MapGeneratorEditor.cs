using UnityEditor;
using UnityEngine;


[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector()) //jezeli dokonano zmiany
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }

    }
}
