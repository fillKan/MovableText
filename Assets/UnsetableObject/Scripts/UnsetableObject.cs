using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnstableStyle
{
    Rotation, Vibrato, RotationAndVibrato
}

public class UnsetableObject : MonoBehaviour
{
    [SerializeField] private  uint mWaitFrame;
    [SerializeField] private float mRotate;
    [SerializeField] private float mVibrato;
    [SerializeField] private UnstableStyle mSTYLE;

    private Vector2 mOriginPosition;

    private IEnumerator mEUpdate;

    public void Setting(uint waitFrame, float vibrato) {
        mWaitFrame = waitFrame; mVibrato = vibrato;
    }

    private void OnEnable()
    {
        StartCoroutine(mEUpdate = EUpdate());

        mOriginPosition = transform.localPosition;
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
                case UnstableStyle.Rotation:
                    transform.localRotation = Quaternion.Euler(Vector3.forward * mRotate * Random.Range(-1f, 1f));
                    break;

                case UnstableStyle.Vibrato:
                    transform.localPosition = mOriginPosition + Random.insideUnitCircle * mVibrato;
                    break;

                case UnstableStyle.RotationAndVibrato:
                    transform.localPosition = mOriginPosition + Random.insideUnitCircle * mVibrato;
                    transform.localRotation = Quaternion.Euler(Vector3.forward * mRotate * Random.Range(-1f, 1f));
                    break;
                default:
                    break;
            }
            
        }
    }
}
