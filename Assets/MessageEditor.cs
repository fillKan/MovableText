using UnityEngine;
using UnityEditor;

public class MessageEditor : EditorWindow
{
    private static Vector2 WindowFixSize = new Vector2(415, 290);

    private string mName;
    private string mMessage;

    [MenuItem("MessageEditor/Create Unsettled")]
    private static void Init()
    {
        MessageEditor window = EditorWindow.GetWindow(typeof(MessageEditor)) as MessageEditor;

        window.Show();
        window.minSize = WindowFixSize;
        window.maxSize = WindowFixSize;
    }

    private void OnGUI()
    {
        GUILayout.Label("Object Name", EditorStyles.label);
        mName = GUILayout.TextField(mName);

        GUILayout.Space(8f);

        GUILayout.Label("Message", EditorStyles.label);
        mMessage = GUILayout.TextField(mMessage);

        if (GUILayout.Button("Create!")) {
            Create();
        }
    }
    private void Create()
    {
        GameObject newObject = new GameObject(mName, typeof(RectTransform), typeof(UnsettledText));

        Undo.RegisterCreatedObjectUndo(newObject, mName);

        if (newObject.TryGetComponent(out UnsettledText text)) {
            text.SetMessage(mMessage);
        }
    }
}
