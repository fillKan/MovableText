using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalityChar : MonoBehaviour
{
    [SerializeField] private  uint mWaitFrame;
    [SerializeField] private float mVibrato;

    private readonly Vector3 NormalRotation = new Vector3(0f, 0f, 1f);

    private IEnumerator mEUpdate;

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

            transform.localRotation = Quaternion.Euler(NormalRotation * mVibrato * Random.Range(-1f, 1f));
        }
    }
}
