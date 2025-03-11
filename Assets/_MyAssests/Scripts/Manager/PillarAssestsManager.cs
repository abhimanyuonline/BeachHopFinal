using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillarAssestsManager : MonoBehaviour
{
    [SerializeField]
    List<Sprite> pillarSprites = new();
    [SerializeField]
    List<Sprite> shadowSprite = new();
    [SerializeField]
    List<Sprite> rippleSprite = new();


    [SerializeField]
    SpriteRenderer PillarImage = null;
    [SerializeField]
    SpriteRenderer shadowImage = null;
    [SerializeField]
    SpriteRenderer rippleImage = null;



    // Start is called before the first frame update
    void Start()
    {
        var val = Random.Range(0, 3);
        PillarImage.sprite = pillarSprites[val];
        shadowImage.sprite = shadowSprite[val];
        rippleImage.sprite = rippleSprite[val];
        this.gameObject.name = "Pillar_" + val;
    }
    
}