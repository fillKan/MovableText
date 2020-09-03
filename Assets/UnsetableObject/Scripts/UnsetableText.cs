using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnsetableText : MonoBehaviour
{
    [SerializeField][TextArea]
    private string mMessage;

    private UnsetableObject[] mUnsetables;

    public void SetMessage(string message) => mMessage = message;

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
