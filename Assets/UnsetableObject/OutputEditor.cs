using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class OutputEditor : EditorWindow
{
    [MenuItem("Tools/Unstable Text/Output Edit")]
    private static void Init()
    {
        OutputEditor window = EditorWindow.GetWindow(typeof(OutputEditor)) as OutputEditor;

        window.Show();
    }

    private void OnGUI()
    {
        
    }
}
