using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenFlasher : MonoBehaviour {

    public Image white, black;

    Coroutine flashCoroutine;

    void Awake() {
        white.gameObject.SetActive(false);
        black.gameObject.SetActive(false);
    }

    public void FlashWhiteBlack() {
        if (flashCoroutine != null) {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashWhiteBlackCoroutine());
    }

    IEnumerator FlashWhiteBlackCoroutine() {
        black.gameObject.SetActive(true);
        yield return new WaitForSeconds(Time.deltaTime);
        black.gameObject.SetActive(false);
        white.gameObject.SetActive(true);
        yield return new WaitForSeconds(Time.deltaTime);
        white.gameObject.SetActive(false);

        flashCoroutine = null;
    }
}
