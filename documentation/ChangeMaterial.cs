using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] materials;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];
    }

    public void Changed(double value, double v2h, double v2l)
    {
        if (value > v2h) { rend.sharedMaterial = materials[1]; }
        else if (value < v2l) { rend.sharedMaterial = materials[2]; }
        else rend.sharedMaterial = materials[0];
    }
}
