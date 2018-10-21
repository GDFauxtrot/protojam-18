using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpiralUI : MonoBehaviour {

    public Image fillImage;
    [Range(0f, 1f)]
    public float fillAmount;

    Material spiralFillMaterial;

    void Awake() {
        spiralFillMaterial = fillImage.material;
    }

    void Update () {
        spiralFillMaterial.SetFloat("_Step", 1 - fillAmount);
    }
}
