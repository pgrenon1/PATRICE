using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class PostProcessEffectData
{
    public float duration;
    [Space]
    public float bloomStrength = 1f;
    public AnimationCurve bloomOverTime;
    public float chromaticAberrationStrength = 1f;
    public AnimationCurve chromaticAberrationOverTime;
    public float grainStrength = 1f;
    public AnimationCurve grainOverTime;
    public float lensDistortionStrength = 1f;
    public AnimationCurve lensDistortionOverTime;
}