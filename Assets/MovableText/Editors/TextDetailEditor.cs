using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TextDetailEditor : EditorWindow
{
    private MovableText mUnstable;

    private string mMessage;

    private float mLetterSpace;
    private float mInterval;

    private bool mIsPrinOnebyOne;

    private FadeCInfo mFadeInfo;

    [MenuItem("Tools/Movable Text/Edit Text Detail")]
    private static void Init()
    {
        TextDetailEditor window = EditorWindow.GetWindow(typeof(TextDetailEditor)) as TextDetailEditor;

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(8f);
        mUnstable = (MovableText)EditorGUILayout.ObjectField("Edit Target", mUnstable, typeof(MovableText), true);
        GUILayout.Space(2f);

        if (GUILayout.Button("Extraction"))
        {
            if (mUnstable != null)
            {
                mMessage = mUnstable.Message;

                mLetterSpace = mUnstable.LetterSpace;
                mFadeInfo = mUnstable.GetFadeInfo;
            }
            else
            {
                Debug.Log("You should be selection to edit target!");
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Print-out Option", EditorStyles.boldLabel);
        GUILayout.Space(2.5f);

        GUILayout.Label("Message", EditorStyles.label);
        mMessage = EditorGUI.TextField(new Rect(2.5f, 102f, EditorGUIUtility.currentViewWidth - 7f, 18f), mMessage);
        GUILayout.Space(21f);

        GUILayout.Label("Letter Space", EditorStyles.label);
        mLetterSpace = EditorGUILayout.FloatField(mLetterSpace);

        GUILayout.Label("Interval", EditorStyles.label);
        mInterval = EditorGUILayout.FloatField(mInterval);

        mIsPrinOnebyOne = EditorGUILayout.Toggle("Print-OnebyOne", mIsPrinOnebyOne);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Fading Option", EditorStyles.boldLabel);
        GUILayout.Space(2.5f);

        GUILayout.Label("Fade Type", EditorStyles.label);
        mFadeInfo.FadeType = (FadeType)EditorGUILayout.EnumPopup(mFadeInfo.FadeType);

        GUILayout.Label("Fade Time", EditorStyles.label);
        mFadeInfo.FadeTime = EditorGUILayout.FloatField(mFadeInfo.FadeTime);

        mFadeInfo.IsUsingTimeScale = 
        EditorGUILayout.Toggle("Is Using Time Scale", mFadeInfo.IsUsingTimeScale);

        mFadeInfo.IsFadedDisable =
        EditorGUILayout.Toggle("Is Faded Disable", mFadeInfo.IsFadedDisable);

        GUILayout.Space(3f);

        if (GUILayout.Button("Apply") && mUnstable != null)
        {
            Undo.RecordObject(mUnstable, "Apply");

            mUnstable.Setting(mMessage, mLetterSpace);

            mUnstable.Setting(mFadeInfo);
        }
    }
}
