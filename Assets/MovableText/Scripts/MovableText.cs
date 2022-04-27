using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableText : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _Message;
    [SerializeField] private float _LetterSpace;
    [SerializeField] private MovableStyle _TextInfo;

    /* ====== ====== public property ====== ====== */
    public MovableObject this[int index]
    {
        get
        {
            CheckMovObjectArray();

            return _MovObjectArray[Mathf.Clamp(index, 0, _MovObjectArray.Length - 1)];
        }
    }
    public string Message 
    {
        get => _Message; 
    }
    public float LetterSpace 
    {
        get => _LetterSpace; 
    }
    public MovableStyle GetTextInfo
    {
        get => _TextInfo;
    }
    /* ====== ====== public property ====== ====== */

    private MovableObject[] _MovObjectArray;

    private void OnEnable()
    {
        CheckMovObjectArray();
    }
    private void Update()
    {
        for (int i = 0; i < _MovObjectArray.Length; ++i)
        {
            if (_MovObjectArray[i].gameObject.activeSelf)
            {
                _MovObjectArray[i].UpdateMe();
            }
        }
    }
    public MovableObject[] GetMovableObjects()
    {
        CheckMovObjectArray();

        return _MovObjectArray;
    }
    public void Setting(MovableStyle info) => _TextInfo = info;
    public void Setting(string message, float letterSpace)
    {
        _Message = message; _LetterSpace = letterSpace;
    }
    private void CheckMovObjectArray()
    {
        if (_MovObjectArray == null)
        {
            _MovObjectArray = new MovableObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out MovableObject unstable))
                {
                    _MovObjectArray[i] = unstable;
                }
            }
        }
    }
}