using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class MovableExtension
{
    public static bool IsEven(this int num)
    {
        return (Mathf.Abs(num) & 1) == 0;
    }
    #region COMMENT
    /// <summary>
    /// 메시지의 길이, 자간, 이 객체가 몇번째 글자인지를 고려하여 (0,0)을 기준으로 자간을 설정합니다.
    /// </summary>
    /// <param name="messageLength">메시지의 총 길이를 지정합니다</param>
    /// <param name="space">각 글자간의 간격을 지정합니다</param>
    /// <param name="index">이 글자가 몇번째 글자인지를 지정합니다</param>
    #endregion
    public static void SetLetterSpace(this Transform transform, int messageLength, float space, int index)
    {
        float charOffset = messageLength.IsEven() ? space / 2 : 0f;

        transform.localPosition = Vector2.right * ((-messageLength / 2 + index) * space + charOffset);
    }
}
public class Movable
{
    public static MovableText CreateMovableText(string name, Canvas parent, Vector3 position)
    {
        GameObject @object = new GameObject(name, typeof(RectTransform), typeof(MovableText));

        Undo.RegisterCreatedObjectUndo(@object, name);

                   @object.transform.parent        = parent.transform;
                   @object.transform.localPosition = position;

        Debug.Assert(@object.TryGetComponent(out MovableText movable));

        return movable;
    }

    public static MovableObject CreateMovableChar(int index, char letter, MovCharInfo cInfo)
    {
        string name = $"Character[{index}]";

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
