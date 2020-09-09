using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
