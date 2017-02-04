using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Util;

/// <summary>
/// A simple scene fade in/out script.
/// </summary>
public class FadeInOut : MonoBehaviour {

    public AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0f,1f,1f,0f);
    public AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private SimpleTimer timer = new SimpleTimer();

    [Tooltip("The time it takes to fade the scene in.")]
    public float fadeInTime = 1.5f;
    [Tooltip("The time it takes to fade the scene out.")]
    public float fadeOutTime = 1.5f;

    // Whether or not the scene is still fading in
    private bool sceneStarting = true;
    [SerializeField]
    [Tooltip("Best is to use a solid color or white texture.")]
    private Texture fader;
    private Rect area;
    [SerializeField]
    private Color color = Color.white;
    private Rect sourceRect = new Rect(0,0,1f,1f);
    private bool draw = false;

    [SerializeField]
    private bool fadeInFromStart = true;

    void Awake() {
        // Set the texture so that it is the the size of the screen and covers it.
        area = new Rect(0f, 0f, Screen.width, Screen.height);
        color.a = 1f;

        //Only fade in if set
        sceneStarting = fadeInFromStart;

        if (!fadeInFromStart) {
            color.a = 0f;
        }else {
            timer.Start();
            StartCoroutine(StartSceneCoroutine());
        }
    }

    void FadeToTransparent() {
        // Lerp the colour of the texture between itself and transparent.
        float percent = Mathf.Clamp(timer.GetTime() / fadeInTime, 0f, 1f);
        color.a = fadeInCurve.Evaluate(percent);
    }


    void FadeToOpaque() {
        // Lerp the colour of the texture between itself and black.
        float percent = Mathf.Clamp(timer.GetTime() / fadeOutTime, 0f, 1f);
        color.a = fadeOutCurve.Evaluate(percent);
    }
    
    //This is called repeatedly until fading is done.
    void StartScene() {
        if(!draw)
            draw = true;

        // Fade the texture to clear.
        FadeToTransparent();

        // If the texture is almost clear...
        if (color.a <= 0.05f) {
            // ... set the colour to clear and disable the GUITexture.
            color.a = 0f;

            // The scene is no longer starting.
            sceneStarting = false;

            draw = false;
        }
    }

    //This calls start scene until scene is started, then this ends.
    private IEnumerator StartSceneCoroutine() {
        while (sceneStarting) {
            StartScene();
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Fades out to the specified scene.
    /// This can be called once and will fade until scene changes.
    /// </summary>
    /// <param name="sceneToGo"></param>
    public void FadeOutToScene(string sceneToGo) {
        timer.Reset();
        StartCoroutine(EndSceneCoroutine(sceneToGo));
    }

    private IEnumerator EndSceneCoroutine(string sceneToGo) {
        while (true) {
            EndScene(sceneToGo);
            yield return new WaitForEndOfFrame();
        }
    }

    private void EndScene(string sceneToGo) {
        draw = true;

        // Start fading
        FadeToOpaque();

        // If the screen is almost faded (opaque...)
        if (color.a >= 0.99f)
            // ... reload the level.
            SceneManager.LoadScene(sceneToGo);
    }

    /// <summary>
    /// Delegate to fade out with an action
    /// </summary>
    public delegate void WhatNext();

    /// <summary>
    /// Fades out to the specified scene.
    /// This can be called once and will fade until scene changes.
    /// </summary>
    /// <param name="sceneToGo"></param>
    public void FadeOutAction(WhatNext next) {
        timer.Reset();
        StartCoroutine(EndSceneAndDoSomethingCoroutine(next));
    }

    private IEnumerator EndSceneAndDoSomethingCoroutine(WhatNext next) {
        while (color.a < 0.99f) {
            FadeOut();
            yield return new WaitForEndOfFrame();
        }

        if(next != null)
            next();
    }

    //Fade out and do something
    private void FadeOut() {
        draw = true;

        // Start fading
        FadeToOpaque();
    }

    void OnGUI() {
        if (Event.current.type != EventType.Repaint) return;

        if (draw)
            Graphics.DrawTexture(area, fader, sourceRect, 0, 0, 0, 0, color);
    }
}
