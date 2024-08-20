using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Damageable))]
public class DamageableEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Damageable d = (Damageable)target;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        if (GUILayout.Button("Hit"))
        {
            d.Hit(0);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Heal"))
        {
            d.Heal(5);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Kill"))
        {
            d.Hit(1000);
        }
    }

}