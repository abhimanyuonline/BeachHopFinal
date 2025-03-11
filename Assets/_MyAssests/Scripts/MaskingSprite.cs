using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaskingSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject waterRipple = default;
    [SerializeField]
    Vector2 playerContactPos = default;
    [SerializeField]
    Vector2 currentPos = default;
    [SerializeField] SoundManager soundManger;
    // Start is called before the first frame update
    void Awake()
    {
        currentPos = this.gameObject.transform.position;
    }
    void Start()
    {
      //  Invoke(nameof(CreateWaterRipple), 4.0f);
    }

    void CreateWaterRipple()
    {
        var randomY = Random.Range(-1.0f, 1.0f);
        this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, currentPos.y + randomY);
        Invoke(nameof(CreateWaterRipple), 4.0f);
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            playerContactPos = new Vector2();
            {
                soundManger.PlayMusic("GameOver",false);
                playerContactPos.x = obj.transform.position.x;
                playerContactPos.y = transform.position.y + this.gameObject.transform.localScale.y / 2;
                var waterSplash = Instantiate(waterRipple, playerContactPos, Quaternion.identity);
                Destroy(waterSplash, 2.0f);

            }

        }
    }

}

