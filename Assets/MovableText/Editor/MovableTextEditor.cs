using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(MovableText))]
public class MovableTextEditor : Editor
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
                    MovableUtilityForEditor.CreateMovableChar(i, movableText.Message[i], movableText.GetTextInfo)
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