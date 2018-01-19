using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviorScript : MonoBehaviour {

	public Sprite[] catSprite;

    void Awake (){
        int rand = Random.Range(0, catSprite.Length - 1);
        Sprite temp_sprite = catSprite[rand];

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = temp_sprite;
    }

}
