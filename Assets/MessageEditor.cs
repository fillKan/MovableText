using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MessageEditor : EditorWindow
{
    private static Vector2 WindowFixSize = new Vector2(415, 290);

    private readonly Rect OneBlockRECT = new Rect(2.5f, 63.5f, 409f, 18f);

    private string mName;
    private string mMessage;
    private Font  mFont;
    private Color mColor = Color.white;

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
        mMessage = EditorGUI.TextField(OneBlockRECT, mMessage);

        GUILayout.Space(21f);

        GUILayout.Label("Font", EditorStyles.label);
        mFont = (Font)EditorGUILayout.ObjectField(mFont, typeof(Font), true);

        GUILayout.Label("Color", EditorStyles.label);
        mColor = EditorGUILayout.ColorField(mColor);

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
        for (int i = 0; i < mMessage.Length; i++)
        {
            GameObject createChar = CreateUnSettledChar(mMessage[i]);

            createChar.transform.localPosition = new Vector2(i * 20f, 0);

            createChar.transform.parent = newObject.transform;
        }
    }
    private GameObject CreateUnSettledChar(char letter)
    {
        string name = $"Character[{letter}]";

        GameObject newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(UnsettledChar));

        Undo.RegisterCreatedObjectUndo(newObject, name);

        if (newObject.TryGetComponent(out Text text)) 
        {
            text.text = letter.ToString();

            text.alignment = TextAnchor.MiddleCenter;

            text.fontSize = 26; text.font = mFont;

            text.color = mColor;
        }
        if (newObject.TryGetComponent(out UnsettledChar unsettled)) {
            unsettled.Setting(6, 5f);
        }
        return newObject;
    }
}
