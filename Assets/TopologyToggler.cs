using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopologyToggler : MonoBehaviour
{
    public GameObject iceCollider;
    public GameObject fireCollider;

    public MeshRenderer iceMeshRenderer;
    public Material offIce;
    public Material onIce;
    public MeshRenderer fireMeshRenderer;
    public Material offFire;
    public Material onFire;

    public MeshRenderer icicle1Renderer;
    public Material icicle1Off;
    public Material icicle1On;

    public MeshRenderer icicle2Renderer;
    public Material icicle2Off;
    public Material icicle2On;

    private void Update()
    {
        if (GameManager.Instance.ActiveDimension == Dimension.Fire)
        {
            iceMeshRenderer.material = offIce;
            fireMeshRenderer.material = onFire;
            icicle1Renderer.material = icicle1Off;
            icicle2Renderer.material = icicle2Off;

            fireCollider.SetActive(true);
            iceCollider.SetActive(false);
        }   
        else
        {
            iceMeshRenderer.material = onIce;
            fireMeshRenderer.material = offFire;
            icicle1Renderer.material = icicle1Off;
            icicle2Renderer.material = icicle2Off;

            fireCollider.SetActive(false);
            iceCollider.SetActive(true);
        }
    }
}
