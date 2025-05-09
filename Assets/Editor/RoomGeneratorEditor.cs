#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
    SerializedProperty roomHeight;
    SerializedProperty roomWidth;
    SerializedProperty tileSize;
    SerializedProperty wallSize;
    SerializedProperty tileOffset;
    SerializedProperty doorPosX;
    SerializedProperty doorPosY;
    SerializedProperty floorCount;
    SerializedProperty makeFloor;
    SerializedProperty makeWall;
    SerializedProperty makeFoundation;

    SerializedProperty tiles;
    SerializedProperty walls;
    SerializedProperty firstWalls;
    SerializedProperty wallCorners;
    SerializedProperty wallHalves;
    SerializedProperty doors;

    SerializedProperty floorFoundation;

    bool showRoomSettings = true;
    bool showPrefabLists = true;
    bool showFoundation = true;

    private void OnEnable()
    {
        roomHeight = serializedObject.FindProperty("roomHeight");
        roomWidth = serializedObject.FindProperty("roomWidth");

        tileSize = serializedObject.FindProperty("tileSize");
        wallSize = serializedObject.FindProperty("wallSize");
        tileOffset = serializedObject.FindProperty("tileOffset");
        doorPosX = serializedObject.FindProperty("doorPosX");
        doorPosY = serializedObject.FindProperty("doorPosY");
        floorCount = serializedObject.FindProperty("floorCount");
        makeFloor = serializedObject.FindProperty("makeFloor");
        makeWall = serializedObject.FindProperty("makeWall");
        makeFoundation = serializedObject.FindProperty("makeFoundation");

        tiles = serializedObject.FindProperty("tiles");
        walls = serializedObject.FindProperty("walls");
        firstWalls = serializedObject.FindProperty("firstWalls");
        wallCorners = serializedObject.FindProperty("wallCorners");
        wallHalves = serializedObject.FindProperty("wallHalves");
        doors = serializedObject.FindProperty("doors");

        floorFoundation = serializedObject.FindProperty("floorFoundation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Room Dimensions", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(roomHeight);
        EditorGUILayout.PropertyField(roomWidth);

        EditorGUILayout.Space();

        showRoomSettings = EditorGUILayout.Foldout(showRoomSettings, "Room Settings");
        if (showRoomSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(tileSize);
            EditorGUILayout.PropertyField(wallSize);
            EditorGUILayout.PropertyField(tileOffset);
            EditorGUILayout.PropertyField(doorPosX);
            EditorGUILayout.PropertyField(doorPosY);
            EditorGUILayout.PropertyField(floorCount);
            EditorGUILayout.PropertyField(makeFloor);
            EditorGUILayout.PropertyField(makeWall);
            EditorGUILayout.PropertyField(makeFoundation);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        showPrefabLists = EditorGUILayout.Foldout(showPrefabLists, "Prefab Lists (Randomized)");
        if (showPrefabLists)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(tiles, true);
            EditorGUILayout.PropertyField(walls, true);
            EditorGUILayout.PropertyField(firstWalls, true);
            EditorGUILayout.PropertyField(wallCorners, true);
            EditorGUILayout.PropertyField(wallHalves, true);
            EditorGUILayout.PropertyField(doors, true);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        showFoundation = EditorGUILayout.Foldout(showFoundation, "Foundation Prefab");
        if (showFoundation)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(floorFoundation);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        RoomGenerator generator = (RoomGenerator)target;

        if (GUILayout.Button("Generate Room"))
        {
            generator.GenerateRoom();
        }

        if (GUILayout.Button("Clear Room"))
        {
            ClearChildren(generator.transform);
        }

        serializedObject.ApplyModifiedProperties();
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
