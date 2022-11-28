using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MeshRenderer))]
public class SaveMaterialAsAsset : UnityEditor.Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Save Material as File")) {
            Renderer renderer = (Renderer)target;
            Material mat = renderer.sharedMaterial;
            Texture2D tex = (Texture2D)mat.mainTexture;
            byte[] data = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Assets/Materials" + System.DateTime.Now.ToString("yyyymmdd") + ".png", data);
        }
    }
}

