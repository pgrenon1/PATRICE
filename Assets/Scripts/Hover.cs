using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float speed = 1f;
    public float strength = 1f;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = _originalScale + Vector3.one * Mathf.Cos(Time.time * speed) * strength;
    }
}
