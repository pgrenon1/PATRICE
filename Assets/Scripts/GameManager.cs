using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Dimension ActiveDimension { get; set; }
    public bool IsGodMode { get; private set; }

    public float delayBeforeEffectiveSwitch = 1f;
    public PostProcessEffect dimensionSwitchEffect;

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
        StartCoroutine(DoSwitchDimension());

        if (triggerPostProcessEffect)
        {
            _postProcessEffectCoroutine = StartCoroutine(dimensionSwitchEffect.DoEffect(_volume));
        }
    }

    private IEnumerator DoSwitchDimension()
    {
        yield return new WaitForSeconds(delayBeforeEffectiveSwitch);

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

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
