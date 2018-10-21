using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpiralUI : MonoBehaviour {

    GameManager gameManager;

    public Image fillImage;
    [Range(0f, 1f)]
    public float fillAmount;

    Material spiralFillMaterial;


    void Awake() {
        spiralFillMaterial = fillImage.material;
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update () {
        //Update fillAmount based on globaly set value
        fillAmount = gameManager.meterPercent;

        spiralFillMaterial.SetFloat("_Step", 1 - fillAmount);
    }
}
