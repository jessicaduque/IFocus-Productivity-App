using UnityEngine;
using UnityEditor;
using TMPro;

public class ChangeFonts : EditorWindow
{
    private TMP_FontAsset newFont;
    private TMP_FontAsset fontToSubstitute;

    [MenuItem("Tools/Change All Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ChangeFonts>("Change All Fonts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change All TextMeshPro Fonts", EditorStyles.boldLabel);

        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFont, typeof(TMP_FontAsset), false);
        fontToSubstitute = (TMP_FontAsset)EditorGUILayout.ObjectField("Old Font Asset To Substitute", fontToSubstitute, typeof(TMP_FontAsset), false);
        
        if (GUILayout.Button("Change Fonts"))
        {
            if (newFont != null)
            {
                ChangeAllFonts();
            }
            else
            {
                Debug.LogError("Please assign a new font asset.");
            }
        }
    }

    private void ChangeAllFonts()
    {
        var allTextObjects = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var textObj in allTextObjects)
        {
            Undo.RecordObject(textObj, "Change Font");
            if(textObj.font == fontToSubstitute)
                textObj.font = newFont;
            EditorUtility.SetDirty(textObj);
        }

        Debug.Log($"Updated {allTextObjects.Length} TextMeshPro components.");
    }
}