using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class OutputEditor : EditorWindow
{
    private UnstableText mUnstable;

    private string mMessage;

    private float mLetterSpace;
    private float mInterval;

    private bool mIsOutputOnebyOne;

    [MenuItem("Tools/Unstable Text/Output Edit")]
    private static void Init()
    {
        OutputEditor window = EditorWindow.GetWindow(typeof(OutputEditor)) as OutputEditor;

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(8f);
        mUnstable = (UnstableText)EditorGUILayout.ObjectField("Edit Target", mUnstable, typeof(UnstableText), true);
        GUILayout.Space(2f);

        if (GUILayout.Button("Extraction"))
        {
            if (mUnstable != null)
            {
                mMessage = mUnstable.Message;

                mLetterSpace = mUnstable.LetterSpace;
                mInterval    = mUnstable.Interval;

                mIsOutputOnebyOne = mUnstable.IsOutputOnebyOne;
            }
            else
            {
                Debug.Log("You should be selection to edit target!");
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Message", EditorStyles.label);
        mMessage = EditorGUI.TextField(new Rect(2.5f, 86f, EditorGUIUtility.currentViewWidth - 7f, 18f), mMessage);
        GUILayout.Space(21f);

        GUILayout.Label("Letter Space", EditorStyles.label);
        mLetterSpace = EditorGUILayout.FloatField(mLetterSpace);

        GUILayout.Label("Interval", EditorStyles.label);
        mInterval = EditorGUILayout.FloatField(mInterval);

        mIsOutputOnebyOne = EditorGUILayout.Toggle("Print Out-OnebyOne",mIsOutputOnebyOne);
        
        GUILayout.Space(2f);

        if (GUILayout.Button("Apply") && mUnstable != null)
        {
            Undo.RecordObject(mUnstable, "Apply");

            mUnstable.Setting(mMessage, mLetterSpace, mInterval);

            mUnstable.IsOutputOnebyOne = mIsOutputOnebyOne;
        }
    }
}
