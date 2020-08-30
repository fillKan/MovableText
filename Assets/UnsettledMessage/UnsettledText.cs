using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnsettledText : MonoBehaviour
{
    [SerializeField][TextArea]
    private string mMessage;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Text text))  {
                text.text = mMessage[i].ToString();
            }
        }
    }
}
