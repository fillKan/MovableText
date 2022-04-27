using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MovableUtilityForEditor
{
    private const string CHAR_OBJECT_NAME_FORMAT = "Character : {0}";
    private const string UNDO_PREFIX = "MovableUtilityForEditor : ";

    public static MovableText CreateMovableText(string name, Canvas canvas, Vector3 position)
    {
        // 변경사항들을 그룹으로 저장하기 시작.
        Undo.IncrementCurrentGroup();

        string undoCreate = string.Concat(UNDO_PREFIX, $"Create MovableText ({name})");
        string undoSetParent = string.Concat(UNDO_PREFIX, $"Set MovableText parent");
        string undoHierarchy = string.Concat(UNDO_PREFIX, "Register to hierarchy");
        string undoGroupName = string.Concat(UNDO_PREFIX, $"Create MovableText ({name}) completely");

        // 오브젝트 생성.
        var mov = new GameObject(name, typeof(RectTransform), typeof(MovableText));
        Undo.RegisterCreatedObjectUndo(mov, undoCreate);
        Undo.SetTransformParent(mov.transform, canvas.transform, undoSetParent);
        
        // 객체의 계층 상태를 실행 취소 스택에 복사.
        Undo.RegisterFullObjectHierarchyUndo(mov, undoHierarchy);
        mov.transform.localPosition = position;

        // 지금까지의 변경사항을 그룹으로 저장.
        Undo.SetCurrentGroupName(undoGroupName);

        return mov.GetComponent<MovableText>();
    }

    public static MovableObject CreateMovableChar(int index, char letter, MovCharInfo cInfo)
    {
        string name = string.Format(CHAR_OBJECT_NAME_FORMAT, index.ToString("00"));
        var newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(MovableObject));

        Undo.RegisterCreatedObjectUndo(newObject, name);

        if (newObject.TryGetComponent(out Text text))
        {
            text.fontSize   = cInfo.fontSize;          
            text.fontStyle  = cInfo.fontStyle;
            text.color      = cInfo.color;
            text.alignment  = TextAnchor.MiddleCenter;
            text.text       = letter.ToString();
            text.font       = cInfo.font; 
        }
        if (newObject.TryGetComponent(out MovableObject movable)) 
        {
            movable.Setting(cInfo.waitFrame, cInfo.vibration, cInfo.rotation, cInfo.movableStyle);

            return movable;
        }
        return null;
    }
}