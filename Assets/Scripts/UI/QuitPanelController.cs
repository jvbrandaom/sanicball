using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class QuitPanelController : NetworkBehaviour {

    private float timePressed = -1f;
    float timeElapsed = 0f;
    float percent = 0f;
    [SerializeField]
    private float timeToQuit = 3f;

    [SerializeField]
    private Text quitText;
    [SerializeField]
    private Image quitImage;
    [SerializeField]
    private GameObject panel;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            timePressed = Time.realtimeSinceStartup;
            timeElapsed = 0f;
            percent = 0f;
            panel.SetActive(true);
            //Debug.Log("pressed");
        }

        if (Input.GetKeyUp(KeyCode.Escape)) {
            timePressed = -1f;
            panel.SetActive(false);
            //Debug.Log("released");
        }

        //Means we should be holding
        if(timePressed > 0) {
            timeElapsed = (Time.realtimeSinceStartup - timePressed);
            percent = (Time.realtimeSinceStartup-timePressed) / timeToQuit;
            //Debug.Log("percent: "+ percent);
            //Debug.Log("time: " + timeElapsed);

            //Set fill
            if (quitImage) {
                quitImage.fillAmount = percent;
            }

            //Set text
            if (quitText) {
                quitText.text = (percent < 0.5f ? "Quit to menu?" : "Sure?");
            }

            //Leave game
            if(percent >= 1f) {
                if (isServer)
                    SanicNetworkManager.instance.StopHost();
                else
                    SanicNetworkManager.instance.StopClient();
            }
        }
	}
}
