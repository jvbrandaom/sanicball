using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour {

    [SerializeField]
    private Image fillImage;
		
	// Update is called once per frame
	void Update () {
        fillImage.fillAmount = Mathf.Clamp(PlayerController.localPlayer.Stamina, 0, 1);
	}
}
