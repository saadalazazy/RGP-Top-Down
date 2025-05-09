#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        RoomGenerator generator = (RoomGenerator)target;
        
        if (GUILayout.Button("Generate Room"))
        {
            generator.GenerateRoom();
        }
        
        if (GUILayout.Button("Clear Room"))
        {
            ClearChildren(generator.transform);
        }
    }
    
    private void ClearChildren(Transform parent)
    {
        while (parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }
}
#endif