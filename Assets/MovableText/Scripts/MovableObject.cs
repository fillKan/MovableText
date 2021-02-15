using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MovableStyle
{
    Rotation, Vibration, RotationAndVibration
}

public class MovableObject : MonoBehaviour
{
    public uint WaitFrame;
    [HideInInspector]
    public uint WaitingFrame;

    public float Rotation;
    public float Vibration;
    public MovableStyle Style;
    public Vector2 PivotPoint;

    public MovableObject(uint waitFrame, float rotation, float vibration, MovableStyle style) {
        WaitFrame = waitFrame; Vibration = vibration; Rotation = rotation; Style = style;
    }

    public void Setting(uint waitFrame, float vibration, float rotation, MovableStyle style) {
        WaitFrame = waitFrame; Vibration = vibration; Rotation = rotation; Style = style;
    }

    public void Setting(MovCharInfo movCInfo)
    {
        WaitFrame = movCInfo.waitFrame;

        Style = movCInfo.movableStyle;

        Rotation  = movCInfo.rotation;
        Vibration = movCInfo.vibration;

        if (gameObject.TryGetComponent(out Text text))
        {
            text.color = movCInfo.color;
            text.font  = movCInfo.font;

            text.fontStyle = movCInfo.fontStyle;
            text.fontSize  = movCInfo.fontSize;
        }
    }

    public void UpdateMe()
    {
        if (++WaitingFrame >= WaitFrame)
        {
            switch (Style)
            {
                case MovableStyle.Rotation:
                    transform.localRotation = Quaternion.Euler(Vector3.forward * Rotation * Random.Range(-1f, 1f));
                    break;

                case MovableStyle.Vibration:
                    transform.localPosition = PivotPoint + Random.insideUnitCircle * Vibration;
                    break;

                case MovableStyle.RotationAndVibration:
                    transform.localPosition = PivotPoint + Random.insideUnitCircle * Vibration;
                    transform.localRotation = Quaternion.Euler(Vector3.forward * Rotation * Random.Range(-1f, 1f));
                    break;
                default:
                    break;
            }
            WaitingFrame = 0;
        }
    }

    private void OnEnable()
    {
        PivotPoint = transform.localPosition;
    }
}
