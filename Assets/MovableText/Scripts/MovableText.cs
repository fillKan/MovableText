using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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

[CustomEditor(typeof(MovableText))]
public class MessageEditButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(8f);

        if (!EditorApplication.isPlaying && 
             GUILayout.Button("Apply to changed", GUILayout.Height(25f)))
        {
            MovableText movableText = target as MovableText;

            int       messageLength = movableText.Message.Length;
            Transform movTransform  = movableText.transform;

            #region Summary
            /*=================================================================================
             * If amount of unstableChar is more than messageLength, removal unstableChar at the last.
             * 만약 불안정문자의 수가 메시지의 길이보다 많다면, 제일 뒤에있는 불안정문자를 제거한다. 
             *=================================================================================*/
            #endregion
            while (messageLength < movTransform.childCount) {

               Undo.DestroyObjectImmediate(movTransform.GetChild(movTransform.childCount - 1).gameObject);
            }
            #region Summary
            /*=================================================================================
            * Rearrange unstableChar based on length of changed message and change the each unstableChar by changed message.
            * 변경된 메시지에따라 불안정문자를 재배치하고, 각 불안정문자들이 나타내는 글자를 변경한다.
            *=================================================================================*/
            #endregion
            for (int i = 0; i < messageLength; i++)
            {
                if (i >= movTransform.childCount)
                {
                    Movable.CreateMovableChar(i, movableText.Message[i], movableText.GetTextInfo)
                        .transform.parent = movTransform;
                }

                Undo.RecordObject(movTransform.GetChild(i), movableText.Message);

                if (movableText.transform.GetChild(i).TryGetComponent(out MovableObject movable))
                {
                    movable.Setting(movableText.GetTextInfo);
                }
                movTransform.GetChild(i).SetLetterSpace(messageLength, movableText.LetterSpace, i);

                if (movTransform.GetChild(i).TryGetComponent(out Text text)) 
                {
                    Undo.RecordObject(text, "Apply to changed text");

                    text.text = movableText.Message[i].ToString();
                }
            }
        }
    }
}