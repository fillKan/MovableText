using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnsetableText : MonoBehaviour
{
    [SerializeField][TextArea]
    private string mMessage;

    private UnsetableObject[] mUnsetables;

    #region Output One by One variables

    public bool IsOutputOnebyOne;

    private IEnumerator mEOutputOnebyOne;

    [SerializeField] private float mLetterSpace;
    [SerializeField] private float mInterval;
    #endregion

    public void SetMessage(string message) => mMessage = message;

    private void OnEnable()
    {
        if (IsOutputOnebyOne) {
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
            mUnsetables[iteration++].gameObject.SetActive(true);

            for (int i = 0; i < iteration; i++)
            {
                if (i == iteration - 1)
                {
                    mUnsetables[i].transform.Translate(Vector2.right * i * mLetterSpace * 0.5f, Space.Self);
                }
                else mUnsetables[i].transform.Translate(Vector2.left * mLetterSpace*0.5f, Space.Self);
            }
        }        
        yield break;
    }

    private void Start()
    {
        if (IsOutputOnebyOne)
        {
            mUnsetables = new UnsetableObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out UnsetableObject unsetable))
                {
                    mUnsetables[i] = unsetable;
                }
                transform.GetChild(i).localPosition = Vector2.zero;
            }
        }
    }
}
