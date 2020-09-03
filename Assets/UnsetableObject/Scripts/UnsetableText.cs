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

    private float mLetterSpace;
    private float mInterval;
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
        uint iteration = 0;
        
        while (iteration < mMessage.Length)
        {
            for (float i = 0f; i < mInterval; i += Time.deltaTime * Time.timeScale) {
                yield return null;
            }
            // Out of one character

            iteration++;
        }        
        yield break;
    }

    private void Start()
    {
        mUnsetables = new UnsetableObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out UnsetableObject unsetable)) {
                mUnsetables[i] = unsetable;
            }
        }
    }
}
