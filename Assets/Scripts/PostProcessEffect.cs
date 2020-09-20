using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
public class PostProcessEffect
{
    public float duration;
    public AnimationCurve bloomOverTime;
    public AnimationCurve chromaticAberrationOverTime;
    public AnimationCurve grainOverTime;
    public float lensDistortionStrength;
    public AnimationCurve lensDistortionOverTime;

    public IEnumerator DoEffect(PostProcessVolume volume)
    {
        Bloom bloomSetting = null;
        ChromaticAberration chromaticAberrationSetting = null;
        Grain grainSetting = null;
        LensDistortion lensDistortionSetting = null;

        volume.profile.TryGetSettings(out bloomSetting);
        volume.profile.TryGetSettings(out chromaticAberrationSetting);
        volume.profile.TryGetSettings(out grainSetting);
        volume.profile.TryGetSettings(out lensDistortionSetting);

        float cachedLensDistortion = lensDistortionSetting.intensity.value;

        float t = 0;
        while (t < duration)
        {
            float ratio = t / duration;
            lensDistortionSetting.intensity.value = cachedLensDistortion + lensDistortionOverTime.Evaluate(ratio) * lensDistortionStrength;

            t += Time.deltaTime;
            yield return 0;
        }

        //return to normal
        t = 0;
        while (t < 0.5f)
        {

            t += Time.deltaTime;
            yield return 0;
        }
    }
}