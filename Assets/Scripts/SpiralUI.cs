using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiralUI : MonoBehaviour {

    public float fillAmount;

    Material spiralFillMaterial;

    void Awake() {
        spiralFillMaterial = transform.GetChild(0).GetComponent<Image>().material;
    }

    void Update () {
        spiralFillMaterial.SetFloat("_Step", fillAmount);
    }
}
