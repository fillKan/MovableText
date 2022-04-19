using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovCharInfo
{
    public uint waitFrame;

    public Color color;

    public FontStyle fontStyle;

    public MovableStyle movableStyle;

    public Font font;

    public int   fontSize;
    public float rotation;
    public float vibration;

    public MovCharInfo(Color color, FontStyle fontStyle, Font font, MovableObject movableObject, int fontSize)
    {
        this.color     = color;
        this.fontStyle = fontStyle;
        this.font      = font;
        this.fontSize  = fontSize;

        rotation  = movableObject.Rotation;
        vibration = movableObject.Vibration;
        waitFrame = movableObject.WaitFrame;

        movableStyle = movableObject.Style;
    }
}

public class MovableText : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _Message;
    [SerializeField] private float _LetterSpace;
    [SerializeField] private MovCharInfo _TextInfo;

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
    public MovCharInfo GetTextInfo
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
    public void Setting(MovCharInfo info) => _TextInfo = info;
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