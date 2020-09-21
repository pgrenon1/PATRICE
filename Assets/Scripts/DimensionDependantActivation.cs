using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionDependantActivation : MonoBehaviour
{
    public bool checkConstantly = false;
    public DimensionDependantVisuals dimensionDependantVisuals;

    private void Start()
    {
        GameManager.Instance.DimensionSwitched += Instance_DimensionSwitched;
    }

    private void Update()
    {
        if (checkConstantly)
        {
            dimensionDependantVisuals.fireVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Fire);
            dimensionDependantVisuals.iceVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Ice);
        }
    }

    private void Instance_DimensionSwitched(Dimension newActiveDimension)
    {
        dimensionDependantVisuals.SwitchVisuals(newActiveDimension);
    }

}
