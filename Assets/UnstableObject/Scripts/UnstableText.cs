using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public struct UnstCInfo
{
    public uint waitFrame;

    public Color color;

    public FontStyle fontStyle;

    public UnstableStyle unstableStyle;

    public Font font;

    public int   fontSize;
    public float rotation;
    public float vibration;

    public UnstCInfo(Color color, FontStyle fontStyle, Font font, UnstableObject unstableObject, int fontSize)
    {
        this.color     = color;
        this.fontStyle = fontStyle;
        this.font      = font;
        this.fontSize  = fontSize;

        rotation  = unstableObject.Rotation;
        vibration = unstableObject.Vibration;
        waitFrame = unstableObject.WaitFrame;

        unstableStyle = unstableObject.STYLE;
    }
}

public class UnstableText : MonoBehaviour
{
    [SerializeField][TextArea]
    private string mMessage;
    public  string  Message
    { get => mMessage; }

    private UnstableObject[] mUnstables;

    #region Print OnebyOne variables

    public bool IsPrintOnebyOne;

    private IEnumerator mEOutputOnebyOne;

    public float LetterSpace 
    { get => mLetterSpace; }
    public float Interval 
    { get => mInterval; }

    [SerializeField] private float mLetterSpace;
    [SerializeField] private float mInterval;
    #endregion

    public  UnstCInfo GetTextInfo => mTextInfo;
    private UnstCInfo mTextInfo;
    public void Setting(string message) => mMessage = message;

    public void Setting(UnstCInfo info) => mTextInfo = info;

    public void Setting(string message, float letterSpace, float interval)
    {
        mMessage = message; mLetterSpace = letterSpace; mInterval = interval;
    }

    private void OnEnable()
    {
        if (IsPrintOnebyOne) {
            for (int i = 0; i < mUnstables.Length; i++)
            {
                mUnstables[i].gameObject.SetActive(false);

                mUnstables[i].transform.localPosition = Vector2.zero;
            }
            StartCoroutine(mEOutputOnebyOne = EOutputOnebyOne());
        }
    }

    private void OnDisable()
    {
        if (mEOutputOnebyOne != null) {
            StopCoroutine(mEOutputOnebyOne);
        }
        mEOutputOnebyOne = null;

    }

    private IEnumerator EOutputOnebyOne()
    {
        int iteration = 0;
        
        while (iteration < mMessage.Length)
        {
            for (float i = 0f; i < mInterval; i += Time.deltaTime * Time.timeScale) {
                yield return null;
            }
            mUnstables[iteration++].gameObject.SetActive(true);

            for (int i = 0; i < iteration; i++)
            {
                Vector2 translate = ((i == iteration - 1) ? Vector2.right * i : Vector2.left) * mLetterSpace * 0.5f;

                mUnstables[i].PivotPoint += translate;

                mUnstables[i].transform.localPosition += (Vector3)translate;
            }
        }        
        yield break;
    }

    private void Awake()
    {
        if (IsPrintOnebyOne)
        {
            mUnstables = new UnstableObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out UnstableObject unstable))
                {
                    mUnstables[i] = unstable;
                }
            }
        }
    }
}

[CustomEditor(typeof(UnstableText))]
public class MessageEditButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(8f);

        if (GUILayout.Button("Apply to changed message", GUILayout.Height(25f)))
        {
            UnstableText unstableText = target as UnstableText;

            for (int i = 0; i < unstableText.Message.Length; i++)
            {
                if (i >= unstableText.transform.childCount)
                {
                    break;
                }
                else if (unstableText.transform.GetChild(i).TryGetComponent(out Text text)) 
                {
                    Undo.RecordObject(text, "Apply to changed text");

                    text.text = unstableText.Message[i].ToString();
                }
            }
        }
    }
}