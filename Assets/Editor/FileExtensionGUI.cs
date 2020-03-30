// https://github.com/pointcache/unity-file-extension

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;

[InitializeOnLoad]
public class FileExtensionGUI
{
    static float offset = 12;
    static GUIStyle style;
    static StringBuilder stringBuilder = new StringBuilder();
    static string selectedGuid;

    static FileExtensionGUI()
    {
        EditorApplication.projectWindowItemOnGUI += HandleOnGUI;
        Selection.selectionChanged += () =>
        {
            if (Selection.activeObject != null)
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out selectedGuid, out long id);
        };
    }

    static bool ValidString(string str)
    {
        return !string.IsNullOrEmpty(str) && str.Length > 7;
    }

    static void HandleOnGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        string extRaw = Path.GetExtension(path);

        bool selected = false;
        if (ValidString(guid) && ValidString(selectedGuid))
            selected = String.Compare(guid, 0, selectedGuid, 0, 6) == 0;

        stringBuilder.Clear().Append(extRaw);
        if (stringBuilder.Length > 0)
            stringBuilder.Remove(0, 1);

        string ext = stringBuilder.ToString();

        if (style == null)
            style = new GUIStyle(EditorStyles.label);

        style.normal.textColor = selected ? new Color32(255, 255, 255, 255) : new Color32(120, 120, 120, 160);
        var size = style.CalcSize(new GUIContent(ext));
        selectionRect.x -= size.x + offset;

        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
        EditorGUI.LabelField(offsetRect, ext, style);
    }
}
