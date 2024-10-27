using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HorrorModelDescriptor))]
public class HorrorModelCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Add a space
        GUILayout.Space(30);

        // Create a button and when it's clicked, call the BuildAssetBundle method
        if (GUILayout.Button("Create HorrorModel"))
        {
            CreateHorrorModel.BuildAssetBundle();
        }
    }
}
