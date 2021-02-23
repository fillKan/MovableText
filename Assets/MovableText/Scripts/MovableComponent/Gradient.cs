using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gradient : MonoBehaviour
{
    public bool InvokeOnAwake;
    
    [Header("Gradient Property")]
    public float ProgressingTime;

    [SerializeField, Tooltip("value range : 0 ~ 1")]
    private AnimationCurve GradientCurve;

    public Color GradientColor = Color.white;
    public AnimatorUpdateMode UpdateMode;

    [Header("User Property")]
    public MovableText Owner;

    public float GradientPercent
    {
        get;
        private set;
    }
    public bool IsAlreadyInit
    {
        get;
        private set;
    }

    private IEnumerator _GradientRoutine;
    private Text[] _TextArray;

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
        Initialize();

        if (_GradientRoutine != null)
        {
            StopCoroutine(_GradientRoutine);
        }
        GradientPercent = 0f;
        StartCoroutine(_GradientRoutine = GradientRoutine());
    }
    public void Initialize(bool forceInit = false)
    {
        if (!IsAlreadyInit || forceInit)
        {
            _TextArray = new Text[Owner.GetMovableObjects().Length];

            for (int i = 0; i < _TextArray.Length; i++)
            {
                Owner[i].TryGetComponent(out _TextArray[i]);
            }
            IsAlreadyInit = true;
        }
    }
    private IEnumerator GradientRoutine()
    {
        var textColor = new Color[_TextArray.Length];

        for (int i = 0; i < _TextArray.Length; i++)
        {
            textColor[i] = _TextArray[i].color;
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
        for (float i = 0; GradientPercent < 1f; i += DeltaTime())
        {
            GradientPercent = Mathf.Clamp(i / ProgressingTime, 0f, 1f);
            
            float ratio = GradientCurve.Evaluate(GradientPercent);
            
            for (int index = 0; index < _TextArray.Length; index++)
            {
                _TextArray[index].color = Color.Lerp(textColor[index], GradientColor, ratio);
            }
            yield return null;
        }
        _GradientRoutine = null;
    }
}
