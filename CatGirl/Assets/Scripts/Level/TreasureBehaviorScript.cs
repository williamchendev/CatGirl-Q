using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBehaviorScript : MonoBehaviour {

	//Settings
    private bool opened;
    private bool start_open;
    private int item;
    private int timer;

    private Animator anim;

    public GameObject itemObj;

	void Awake () {
		item = 0;
        timer = 30;
        opened = false;
        start_open = false;

        anim = GetComponent<Animator>();
	}

    void Update() {
        if (!opened){
            if (start_open){
                timer--;
                if (timer <= 0){
                    createItem();
                }
            }
        }
    }

    public void setItem(int type){
        item = type;
    }

    public void openChest(){
        if (!start_open){
            start_open = true;
            anim.Play("aTreasureUnlock");
        }
    }

    private void createItem(){
        GameObject new_item = Instantiate(itemObj, new Vector3(transform.position.x, transform.position.y, itemObj.transform.position.z), transform.rotation);
        new_item.GetComponent<ItemScript>().setType(item);
        opened = true;
    }
}
