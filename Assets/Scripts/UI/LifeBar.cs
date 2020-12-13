using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] Sprite fullHeathSprite;
    [SerializeField] Sprite sixtyHealthSprite;
    [SerializeField] Sprite thirtyHealthSprite;

    Image image;

    public int life;
    
    void Awake()
    {
        image = GetComponent<Image>();
        life = Player.instance.life;
    }

    void Update()
    {
        switch(life)
        {
            case 3:
                image.sprite = fullHeathSprite;
                break;
            case 2:
                image.sprite = sixtyHealthSprite;
                break;
            case 1:
                image.sprite = thirtyHealthSprite;
                break;
        }
    }
}
