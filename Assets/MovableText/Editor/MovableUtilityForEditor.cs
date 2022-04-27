using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MovableUtilityForEditor
{
    private const string CHAR_OBJECT_NAME_FORMAT = "Character : {0}";
    private const string UNDO_PREFIX = "MovableUtilityForEditor : ";

    /// <summary>
    /// This method create a "MovableText". It's only works on unity-editor.
    /// </summary>
    /// <param name="name">Set the name of the object to be created</param>
    /// <param name="canvas">The parents object. Reason for using canvas, cuz this object has ui element children.</param>
    /// <param name="position">Set the position of an object. Exactly, localPosition on the canvas.</param>
    public static MovableText CreateMovableText(string name, Canvas canvas, Vector3 position)
    {
        // 변경사항들을 그룹으로 저장하기 시작.
        Undo.IncrementCurrentGroup();

        string undoCreate = string.Concat(UNDO_PREFIX, $"Create MovableText ({name})");
        string undoSetParent = string.Concat(UNDO_PREFIX, $"Set MovableText parent");
        string undoHierarchy = string.Concat(UNDO_PREFIX, "Register to hierarchy");
        string undoGroupName = string.Concat(UNDO_PREFIX, $"Create MovableText ({name}) completely");

        // 오브젝트 생성.
        var mov = CreateUIObject(name, undoCreate);
        Undo.SetTransformParent(mov.transform, canvas.transform, undoSetParent);
        
        // 객체의 계층 상태를 실행 취소 스택에 복사.
        Undo.RegisterFullObjectHierarchyUndo(mov, undoHierarchy);
        mov.transform.localPosition = position;

        // 지금까지의 변경사항을 그룹으로 저장.
        Undo.SetCurrentGroupName(undoGroupName);

        return mov.AddComponent<MovableText>();
    }

    public static MovableObject CreateMovableChar(int index, char letter, MovableStyle style)
    {
        var movName = string.Format(CHAR_OBJECT_NAME_FORMAT, index.ToString("00"));
        var undoName = string.Concat(UNDO_PREFIX, $"Create MovableObject ({movName})");

        // 오브젝트 생성.
        var mov = CreateUIObject(movName, undoName);
        
        // 컴포넌트 설정 1.
        var text = mov.AddComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.text = letter.ToString();
        style.ApplyStyle(text);

        // 컴포넌트 설정 2.
        var movable = mov.AddComponent<MovableObject>();
        movable.Setting(style);

        return movable;
    }

    private static GameObject CreateUIObject(string name, string undoName)
    {
        var createObject = new GameObject(name, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(createObject, undoName);

        return createObject;
    }
}