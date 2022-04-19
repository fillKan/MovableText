using UnityEngine;
using UnityEngine.UI;

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