using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MessageEditor : EditorWindow
{
    private const int ODD = 1;

    private float mVibration = 5f;
    private float mLetterSpacing = 20f;
    
    private uint mWaitFrame =  6;
    private  int mFontSize  = 26;

    private string mName;
    private string mMessage;
    
    private Canvas  mCanvas;
    private Vector3 mPosition;

    private Font  mFont;
    private Color mColor = Color.white;

    private FontStyle mFontStyle = FontStyle.Normal;

    [MenuItem("MessageEditor/Create Unsettled Text")]
    private static void Init()
    {
        MessageEditor window = EditorWindow.GetWindow(typeof(MessageEditor)) as MessageEditor;

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Object Name", EditorStyles.label);
        mName = GUILayout.TextField(mName);

        GUILayout.Label("Message", EditorStyles.label);
        mMessage = EditorGUI.TextField(new Rect(2.5f, 55f, EditorGUIUtility.currentViewWidth - 7f, 18f), mMessage);

        GUILayout.Space(21f);

        GUILayout.Label("Font", EditorStyles.label);
        mFont = (Font)EditorGUILayout.ObjectField(mFont, typeof(Font), true);

        GUILayout.Label("Parent Canvas", EditorStyles.label);
        mCanvas = (Canvas)EditorGUILayout.ObjectField(mCanvas, typeof(Canvas), true);

        GUILayout.Space(16f);

        GUILayout.Label("Letter Spacing", EditorStyles.label);
        mLetterSpacing = EditorGUILayout.FloatField(mLetterSpacing);

        GUILayout.Label("Font Size", EditorStyles.label);
        mFontSize = EditorGUILayout.IntField(mFontSize);

        mFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", mFontStyle);

        GUILayout.Label("Vibration", EditorStyles.label);
        mVibration = EditorGUILayout.Slider(mVibration, 0f, 180f);

        GUILayout.Label("Wait Frame", EditorStyles.label);
        mWaitFrame = (uint)EditorGUILayout.IntSlider((int)mWaitFrame, 0, 60);

        mPosition = EditorGUILayout.Vector3Field("Position", mPosition);

        GUILayout.Label("Color", EditorStyles.label);
        mColor = EditorGUILayout.ColorField(mColor);

        if (GUILayout.Button("Create!") && !EditorApplication.isPlaying) {
            Create();
        }
    }
    private void Create()
    {
        GameObject newObject = new GameObject(mName, typeof(RectTransform), typeof(UnsetableText));

        Undo.RegisterCreatedObjectUndo(newObject, mName);

        newObject.transform.parent = mCanvas.transform;
        newObject.transform.localPosition = mPosition;

        if (newObject.TryGetComponent(out UnsetableText text)) {
            text.SetMessage(mMessage);
        }
        float charOffset = (mMessage.Length & ODD).Equals(ODD) ? 0f : mLetterSpacing * 0.5f;

        for (int i = 0; i < mMessage.Length; i++)
        {
            GameObject createChar = CreateUnSettledChar(mMessage[i]);

            createChar.transform.parent = newObject.transform;
            createChar.transform.localPosition = new Vector2((-mMessage.Length / 2 + i) * mLetterSpacing + charOffset, 0);
        }
    }
    private GameObject CreateUnSettledChar(char letter)
    {
        string name = $"Character[{letter}]";

        GameObject newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(UnsetableObject));

        Undo.RegisterCreatedObjectUndo(newObject, name);

        if (newObject.TryGetComponent(out Text text)) 
        {
            text.text = letter.ToString();

            text.alignment = TextAnchor.MiddleCenter;

            text.fontSize = mFontSize; text.font = mFont;

            text.color = mColor; 
            
            text.fontStyle = mFontStyle;
        }
        if (newObject.TryGetComponent(out UnsetableObject unsettled)) 
        {
            unsettled.Setting(mWaitFrame, mVibration);
        }
        return newObject;
    }
}
