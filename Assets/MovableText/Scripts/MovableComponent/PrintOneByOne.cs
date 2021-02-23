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

    private void Reset()
    {
        TryGetComponent(out Owner);
    }
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
        string message = Owner.Message;

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
        int iteration = 0;

        while (iteration < array.Length)
        {
            for (float i = 0f; i < IntervalTime; i += DeltaTime())
            {
                yield return null;
            }
            array[iteration++].gameObject.SetActive(true);

            for (int i = 0; i < iteration; i++)
            {
                Vector2 translate = ((i == iteration - 1) ? Vector2.right * i : Vector2.left) * LetterSpace * 0.5f;

                array[i].PivotPoint += translate;

                array[i].transform.localPosition += (Vector3)translate;
            }
        }
    }
}
