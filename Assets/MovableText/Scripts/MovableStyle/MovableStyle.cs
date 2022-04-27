using UnityEngine;
using UnityEngine.UI;

public enum MovableType
{
    Rotation, 
    Vibration, 
    RotationAndVibration
}

[System.Serializable]
public class MovableStyle
{
    public uint waitFrame;

    public Color color;

    public FontStyle fontStyle;

    public MovableType movableStyle;

    public Font font;

    public int   fontSize;
    public float rotation;
    public float vibration;

    public MovableStyle(Color color, FontStyle fontStyle, Font font, MovableObject movableObject, int fontSize)
    {
        this.color     = color;
        this.fontStyle = fontStyle;
        this.font      = font;
        this.fontSize  = fontSize;

        rotation  = movableObject.Rotation;
        vibration = movableObject.Vibration;
        waitFrame = movableObject.WaitFrame;

        movableStyle = movableObject.Style;
    }

    public void ApplyStyle(Text text)
    {
        text.color      = this.color;
        text.fontStyle  = this.fontStyle;
        text.font       = this.font;
        text.fontSize   = this.fontSize;
    }
}