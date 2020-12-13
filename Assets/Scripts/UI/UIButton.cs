using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] Sprite controllerButtonImage;
    [SerializeField] Sprite keyboardButtonImage;
    Image image;
    
    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if(GameController.instance.playerIndexSet)
        {
            image.sprite = controllerButtonImage;
        }
        else
        {
            image.sprite = keyboardButtonImage;
        }

        FadeImage();
    }

    private void FadeImage()
    {
        var alpha = image.color.a;
        alpha = Mathf.Sin(Time.unscaledTime * 2.5f) * 0.5f + 0.5f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}
