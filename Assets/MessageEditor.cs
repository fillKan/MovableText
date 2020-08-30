using UnityEngine;
using UnityEditor;

public class MessageEditor : EditorWindow
{
    [MenuItem("MessageEditor/Create Unsettled")]
    private static void Init()
    {
        MessageEditor window = EditorWindow.GetWindow(typeof(MessageEditor)) as MessageEditor;

        window.Show();
        window.minSize = Vector2.one * 200f;
    }

    private void OnGUI()
    {
        Debug.Log("Open!");
    }
}
