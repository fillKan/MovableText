using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MovableUtilityForEditor
{
    private const string CHAR_OBJECT_NAME_FORMAT = "Character[{0}]";

    public static MovableText CreateMovableText(string name, Canvas canvas, Vector3 position)
    {
        var mov = new GameObject(name, typeof(RectTransform), typeof(MovableText));

        Undo.RegisterCreatedObjectUndo(mov, name);

        mov.transform.parent        = canvas.transform;
        mov.transform.localPosition = position;

        return mov.GetComponent<MovableText>();
    }

    public static MovableObject CreateMovableChar(int index, char letter, MovCharInfo cInfo)
    {
        string name = string.Format(CHAR_OBJECT_NAME_FORMAT, index);

        GameObject newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(MovableObject));

        Undo.RegisterCreatedObjectUndo(newObject, name);

        if (newObject.TryGetComponent(out Text text))
        {
            text.fontSize  = cInfo.fontSize;          
            text.fontStyle = cInfo.fontStyle;

            text.color     = cInfo.color;
            text.alignment = TextAnchor.MiddleCenter;

            text.text = letter.ToString();
            text.font = cInfo.font; 
        }
        if (newObject.TryGetComponent(out MovableObject movable)) 
        {
            movable.Setting(cInfo.waitFrame, cInfo.vibration, cInfo.rotation, cInfo.movableStyle);

            return movable;
        }
        return null;
    }
}