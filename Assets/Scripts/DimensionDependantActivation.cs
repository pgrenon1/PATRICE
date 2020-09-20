using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionDependantActivation : MonoBehaviour
{
    public DimensionDependantVisuals dimensionDependantVisuals;

    private void Start()
    {
        GameManager.Instance.DimensionSwitched += Instance_DimensionSwitched;
    }

    private void Instance_DimensionSwitched(Dimension newActiveDimension)
    {
        dimensionDependantVisuals.SwitchVisuals(newActiveDimension);
    }

}
