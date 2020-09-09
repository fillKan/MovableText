using UnityEngine;
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
}
