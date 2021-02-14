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
    public  uint WaitFrame => mWaitFrame;
    public float Rotation  => mRotation;
    public float Vibration => mVibration;
    public MovableStyle STYLE => mSTYLE;

    [SerializeField] private  uint mWaitFrame;
    [SerializeField] private float mRotation;
    [SerializeField] private float mVibration;
    [SerializeField] private MovableStyle mSTYLE;

    public Vector2 PivotPoint;

    private IEnumerator mEUpdate;

    public MovableObject(uint waitFrame, float rotation, float vibration, MovableStyle style) {
        mWaitFrame = waitFrame; mVibration = vibration; mRotation = rotation; mSTYLE = style;
    }

    public void Setting(uint waitFrame, float vibration, float rotation, MovableStyle style) {
        mWaitFrame = waitFrame; mVibration = vibration; mRotation = rotation; mSTYLE = style;
    }

    public void Setting(MovCharInfo movCInfo)
    {
        mWaitFrame = movCInfo.waitFrame;

        mSTYLE = movCInfo.movableStyle;

        mRotation  = movCInfo.rotation;
        mVibration = movCInfo.vibration;

        if (gameObject.TryGetComponent(out Text text))
        {
            text.color = movCInfo.color;
            text.font  = movCInfo.font;

            text.fontStyle = movCInfo.fontStyle;
            text.fontSize  = movCInfo.fontSize;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(mEUpdate = EUpdate());

        PivotPoint = transform.localPosition;
    }
    private void OnDisable()
    {
        if (mEUpdate != null) {
            StopCoroutine(mEUpdate);
        }
        mEUpdate = null;
    }
    private IEnumerator EUpdate()
    {
        while (gameObject.activeSelf)
        {
            for (uint i = 0; i < mWaitFrame; i++) { yield return null; }

            switch (mSTYLE)
            {
                case MovableStyle.Rotation:
                    transform.localRotation = Quaternion.Euler(Vector3.forward * mRotation * Random.Range(-1f, 1f));
                    break;

                case MovableStyle.Vibration:
                    transform.localPosition = PivotPoint + Random.insideUnitCircle * mVibration;
                    break;

                case MovableStyle.RotationAndVibration:
                    transform.localPosition = PivotPoint + Random.insideUnitCircle * mVibration;
                    transform.localRotation = Quaternion.Euler(Vector3.forward * mRotation * Random.Range(-1f, 1f));
                    break;
                default:
                    break;
            }
            
        }
    }
}
