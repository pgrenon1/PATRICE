using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionDependantActivation : OdinSerializedBehaviour
{
    [NonSerialized, OdinSerialize]
    public Dictionary<Dimension, List<GameObject>> targets = new Dictionary<Dimension, List<GameObject>>();

    private void Start()
    {
        GameManager.Instance.DimensionSwitched += Instance_DimensionSwitched;
    }

    private void Instance_DimensionSwitched(Dimension newActiveDimension)
    {
        foreach (var targetList in targets)
        {
            Dimension dimension = targetList.Key;
            bool isOnDimension = dimension == newActiveDimension;
            foreach (var target in targetList.Value)
            {
                target.SetActive(isOnDimension);
            }
        }
    }
}
