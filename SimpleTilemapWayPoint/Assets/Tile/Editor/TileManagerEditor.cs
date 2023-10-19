using UnityEngine;
using UnityEditor;

#region Custom Button Editor
#if UNITY_EDITOR
[CustomEditor(typeof(TileManager))]
public class TileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileManager component = (TileManager)target;

        EditorGUILayout.Space(); // Add some spacing
        
        if (GUILayout.Button("CreateWayPoints"))
        {
            component.GetWayPoints();
        }
        else if (GUILayout.Button("DeleteWayPoints"))
        {
            component.RemoveWayPoints();
        }

    }
}
#endif
#endregion
