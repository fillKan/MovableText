using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MessageEditor : EditorWindow
{
    private float mVibration = 1f;
    private float mRotation  = 5f;
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
    private UnstableStyle mUnstable = UnstableStyle.Rotation;

    [MenuItem("Tools/Unstable Text/Create")]
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
        mUnstable = (UnstableStyle)EditorGUILayout.EnumPopup("Unstable Style", mUnstable);

        GUILayout.Label("Vibration", EditorStyles.label);
        mVibration = EditorGUILayout.Slider(mVibration, 0.01f, 3f);

        GUILayout.Label("Rotation", EditorStyles.label);
        mRotation = EditorGUILayout.Slider(mRotation, 0f, 180f);

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
        MovableText unstableText = Movable.CreateMovableText(mName, mCanvas, mPosition);

        MovCharInfo unstCInfo 
            = new MovCharInfo(mColor, mFontStyle, mFont, new MovableObject(mWaitFrame, mRotation, mVibration, mUnstable), mFontSize);

        unstableText.Setting(mMessage, mLetterSpacing, 0f);
        unstableText.Setting(unstCInfo);

        for (int i = 0; i < mMessage.Length; i++)
        {
            MovableObject createChar = Movable.CreateMovableChar(i, mMessage[i], unstCInfo);

            createChar.transform.parent = unstableText.transform;
            createChar.transform.SetLetterSpace(mMessage.Length, mLetterSpacing, i);
        }
    }
}
