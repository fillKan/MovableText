using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public enum FadeType 
{
    None, In, Out 
}
[System.Serializable]
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

[System.Serializable]
public struct FadeCInfo
{
    public FadeType FadeType;

    public float FadeTime;

    public bool IsUsingTimeScale;
    public bool IsFadedDisable;
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
    [SerializeField]
    private UnstCInfo mTextInfo;

    public  FadeCInfo GetFadeInfo => mFadeInfo;
    [SerializeField]
    private FadeCInfo mFadeInfo;

    private IEnumerator mEFading;

    public void Setting(string message) => mMessage = message;

    public void Setting(UnstCInfo info) => mTextInfo = info;
    public void Setting(FadeCInfo info) => mFadeInfo = info;

    public void Setting(string message, float letterSpace, float interval)
    {
        mMessage = message; mLetterSpace = letterSpace; mInterval = interval;
    }

    private void OnEnable()
    {
        CheckUnstArray();

        if (IsPrintOnebyOne) {

            for (int i = 0; i < mUnstables.Length; i++)
            {
                mUnstables[i].gameObject.SetActive(false);

                mUnstables[i].transform.localPosition = Vector2.zero;
            }
            StartCoroutine(mEOutputOnebyOne = EOutputOnebyOne());           
        }
        CastFading();
    }

    private void OnDisable()
    {
        if (mEOutputOnebyOne != null) {
            StopCoroutine(mEOutputOnebyOne);
        }
        mEOutputOnebyOne = null;

    }

    private void CastFading()
    {
        if (mEFading != null) {
            StopCoroutine(mEFading); mEFading = null;
        }
        StartCoroutine(mEFading = EFading(mFadeInfo.FadeType));
    }
    private IEnumerator EFading(FadeType fadeType)
    {
        float sumTime = 0f;

        while (sumTime / mFadeInfo.FadeTime < 1f)
        {
            sumTime += Time.deltaTime * (mFadeInfo.IsUsingTimeScale ? Time.timeScale : 1f);

            Color lerpColor = mTextInfo.color;

            switch (fadeType)
            {
                case FadeType.In:
                    lerpColor.a = Mathf.Lerp(0f, 1f, sumTime / mFadeInfo.FadeTime);
                    break;

                case FadeType.Out:
                    lerpColor.a = Mathf.Lerp(1f, 0f, sumTime / mFadeInfo.FadeTime);
                    break;
            }
            for (int i = 0; i < mUnstables.Length; i++)
            {
                if (mUnstables[i].TryGetComponent(out Text text))
                {
                    text.color = lerpColor;
                }
            }
            yield return null;
        }
        if (fadeType.Equals(FadeType.Out) && mFadeInfo.IsFadedDisable)
        {
            gameObject.SetActive(false);
        }
        yield break;
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
    private void CheckUnstArray()
    {
        if (mUnstables == null)
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

        if (!EditorApplication.isPlaying && 
             GUILayout.Button("Apply to changed", GUILayout.Height(25f)))
        {
            UnstableText unstableText = target as UnstableText;

            int       messageLength = unstableText.Message.Length;
            Transform unstTransform = unstableText.transform;

            #region Summary
            /*=================================================================================
             * If amount of unstableChar is more than messageLength, removal unstableChar at the last.
             * 만약 불안정문자의 수가 메시지의 길이보다 많다면, 제일 뒤에있는 불안정문자를 제거한다. 
             *=================================================================================*/
            #endregion
            while (messageLength < unstTransform.childCount) {

               Undo.DestroyObjectImmediate(unstTransform.GetChild(unstTransform.childCount - 1).gameObject);
            }
            #region Summary
            /*=================================================================================
            * Rearrange unstableChar based on length of changed message and change the each unstableChar by changed message.
            * 변경된 메시지에따라 불안정문자를 재배치하고, 각 불안정문자들이 나타내는 글자를 변경한다.
            *=================================================================================*/
            #endregion
            for (int i = 0; i < messageLength; i++)
            {
                if (i >= unstTransform.childCount)
                {
                    Unst.RegisterCharObject(i, unstableText.Message[i], unstableText.GetTextInfo)
                        .transform.parent = unstTransform;
                }

                Undo.RecordObject(unstTransform.GetChild(i), unstableText.Message);

                if (unstableText.transform.GetChild(i).TryGetComponent(out UnstableObject unstable))
                {
                    unstable.Setting(unstableText.GetTextInfo);
                }
                unstTransform.GetChild(i).SetLetterSpace(messageLength, unstableText.LetterSpace, i);

                if (unstTransform.GetChild(i).TryGetComponent(out Text text)) 
                {
                    Undo.RecordObject(text, "Apply to changed text");

                    text.text = unstableText.Message[i].ToString();
                }
            }
        }
    }
}