﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Dimension ActiveDimension { get; set; }
    public bool IsGodMode { get; private set; }

    public float delayBeforeEffectiveSwitch = 1f;
    public PostProcessEffectData dimensionSwitchEffect;

    public delegate void OnSwitchDimension(Dimension newActiveDimension);
    public event OnSwitchDimension DimensionSwitched;

    private Coroutine _postProcessEffectCoroutine;
    private PostProcessVolume _volume;

    private void Start()
    {
        _volume = gameObject.GetComponentInChildren<PostProcessVolume>();

        SwitchDimension(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            IsGodMode = !IsGodMode;
    }

    public void SwitchDimension(bool triggerPostProcessEffect = true)
    {
        StartCoroutine(DoSwitchDimension(triggerPostProcessEffect));
    }

    private IEnumerator DoSwitchDimension(bool triggerPostProcessEffect)
    {
        if (triggerPostProcessEffect)
        {
            StartCoroutine(DoEffect(dimensionSwitchEffect, _volume));
            yield return new WaitForSeconds(delayBeforeEffectiveSwitch);
        }

        if (ActiveDimension == Dimension.Ice)
        {
            ActiveDimension = Dimension.Fire;
        }
        else if (ActiveDimension == Dimension.Fire)
        {
            ActiveDimension = Dimension.Ice;
        }

        if (DimensionSwitched != null)
            DimensionSwitched(ActiveDimension);
    }

    public IEnumerator DoEffect(PostProcessEffectData postProcessEffectData, PostProcessVolume volume)
    {
        Bloom bloomSetting = null;
        ChromaticAberration chromaticAberrationSetting = null;
        Grain grainSetting = null;
        LensDistortion lensDistortionSetting = null;

        volume.profile.TryGetSettings(out bloomSetting);
        volume.profile.TryGetSettings(out chromaticAberrationSetting);
        volume.profile.TryGetSettings(out grainSetting);
        volume.profile.TryGetSettings(out lensDistortionSetting);

        float cachedBloom = bloomSetting.intensity.value;
        float cachedChromaticAberration = chromaticAberrationSetting.intensity.value;
        float cachedGrain = grainSetting.intensity.value;
        float cachedLensDistortion = lensDistortionSetting.intensity.value;

        float t = 0;
        while (t < postProcessEffectData.duration)
        {
            float ratio = t / postProcessEffectData.duration;

            bloomSetting.intensity.value = cachedBloom + postProcessEffectData.bloomOverTime.Evaluate(ratio) * postProcessEffectData.bloomStrength;
            chromaticAberrationSetting.intensity.value = cachedChromaticAberration + postProcessEffectData.chromaticAberrationOverTime.Evaluate(ratio) * postProcessEffectData.chromaticAberrationStrength;
            grainSetting.intensity.value = cachedGrain + postProcessEffectData.grainOverTime.Evaluate(ratio) * postProcessEffectData.grainStrength;
            lensDistortionSetting.intensity.value = cachedLensDistortion + postProcessEffectData.lensDistortionOverTime.Evaluate(ratio) * postProcessEffectData.lensDistortionStrength;

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

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
