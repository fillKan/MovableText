using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsettledChar : MonoBehaviour
{
    [SerializeField] private  uint mWaitFrame;
    [SerializeField] private float mVibrato;

    private IEnumerator mEUpdate;

    public void Setting(uint waitFrame, float vibrato) {
        mWaitFrame = waitFrame; mVibrato = vibrato;
    }

    private void OnEnable()
    {
        StartCoroutine(mEUpdate = EUpdate());
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

            transform.localRotation = Quaternion.Euler(Vector3.forward * mVibrato * Random.Range(-1f, 1f));
        }
    }
}
