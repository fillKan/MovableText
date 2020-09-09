using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class UnstExtension
{
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
        float charOffset = (messageLength & 1).Equals(1) ? 0f : space * 0.5f;

        transform.localPosition = Vector2.right * ((-messageLength / 2 + index) * space + charOffset);
    }
}
public class Unst
{
    public static UnstableText RegisterTextObject(string name, Canvas parent, Vector3 position)
    {
        GameObject @object = new GameObject(name, typeof(RectTransform), typeof(UnstableText));

        Undo.RegisterCreatedObjectUndo(@object, name);

                   @object.transform.parent        = parent.transform;
                   @object.transform.localPosition = position;

        Debug.Assert(@object.TryGetComponent(out UnstableText unstable));

        return unstable;
    }

    public static UnstableObject RegisterCharObject(int index, char letter, UnstCInfo cInfo)
    {
        string name = $"Character[{index}]";

        GameObject newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(UnstableObject));

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
        if (newObject.TryGetComponent(out UnstableObject unstable)) 
        {
            unstable.Setting(cInfo.waitFrame, cInfo.vibration, cInfo.rotation, cInfo.unstableStyle);

            return unstable;
        }
        return null;
    }
}
