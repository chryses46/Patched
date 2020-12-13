using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] Image backgroundFader;

    // make fadeInTarget AND fadeOutTarget
    float target;

    void OnEnable()
    {
        target = 0;
    }

    void Update()
    {
        FadeImage();
    }

   private void FadeImage()
    {
        var alpha = backgroundFader.color.a;
        alpha = Mathf.MoveTowards(alpha, target, Time.deltaTime);
        backgroundFader.color = new Color(backgroundFader.color.r, backgroundFader.color.g, backgroundFader.color.b, alpha);
    }
}
