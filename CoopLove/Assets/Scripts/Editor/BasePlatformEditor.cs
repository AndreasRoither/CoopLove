
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasePlatform))]
public class BasePlatformEditor : Editor
{
    private BasePlatform m_basePlatform;

    private void OnEnable()
    {
        m_basePlatform = target as BasePlatform;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Platform"))
        {
            m_basePlatform.linkedPlatforms.Add(null);
        }

        if (GUILayout.Button("Remove Platform"))
        {
            if (m_basePlatform.linkedPlatforms.Count > 0)
                m_basePlatform.linkedPlatforms.RemoveAt(m_basePlatform.linkedPlatforms.Count-1);
        }

        EditorGUILayout.EndHorizontal();
    }
}
