using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void Setting(string message) => mMessage = message;

    public void Setting(string message, float letterSpace, float interval)
    {
        mMessage = message; mLetterSpace = letterSpace; mInterval = interval;
    }

    private void OnEnable()
    {
        if (IsPrintOnebyOne) {
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
                if (i == iteration - 1)
                {
                    mUnstables[i].PivotPoint += Vector2.right * i * mLetterSpace * 0.5f;

                    mUnstables[i].transform.Translate(Vector2.right * i * mLetterSpace * 0.5f, Space.World);
                }
                else
                {
                    mUnstables[i].PivotPoint += Vector2.left * mLetterSpace * 0.5f;

                    mUnstables[i].transform.Translate(Vector2.left * mLetterSpace * 0.5f, Space.World);
                }
            }
        }        
        yield break;
    }

    private void Start()
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
                transform.GetChild(i).localPosition = Vector2.zero;
            }
        }
    }
}
