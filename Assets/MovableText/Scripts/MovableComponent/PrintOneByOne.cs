using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOneByOne : MonoBehaviour
{
    public bool InvokeOnAwake;
    
    [Header("PrintOut Property")]
    public float IntervalTime;
    public float LetterSpace;
    public AnimatorUpdateMode UpdateMode;

    [Header("User Property")]
    public MovableText Owner;

    private IEnumerator _PrintRoutine;

    private void Awake()
    {
        if (InvokeOnAwake)
        {
            Invoke();
        }
    }
    public void Invoke()
    {
        if (_PrintRoutine != null)
        {
            StopCoroutine(_PrintRoutine);
        }
        StartCoroutine(_PrintRoutine = PrintRoutine());
    }
    private IEnumerator PrintRoutine()
    {
        var array = Owner.GetMovableObjects();

        for (int i = 0; i < array.Length; i++)
        {
            array[i].gameObject.SetActive(false);
        }
        float DeltaTime()
        {
            switch (UpdateMode)
            {
                case AnimatorUpdateMode.Normal:
                    {
                        return Time.deltaTime;
                    }
                case AnimatorUpdateMode.AnimatePhysics:
                    {
                        return Time.fixedDeltaTime;
                    }
                case AnimatorUpdateMode.UnscaledTime:
                    {
                        return Time.unscaledDeltaTime;
                    }
                default:
                    return Time.deltaTime;
            }
        }
        for (int i = 0; i < array.Length; ++i)
        {
            for (float time = 0; time < IntervalTime; time += DeltaTime())
            {
                array[i].gameObject.SetActive(true);
                yield return null;
            }
        }
    }
}
