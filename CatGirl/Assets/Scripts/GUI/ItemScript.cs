using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    //Settings
	public int type;
    private Animator anim;

    private float y_pos;

    //Draw
    private float draw_sin;

	void Awake () {
        anim = GetComponent<Animator>();
        draw_sin = 0;

        y_pos = transform.position.y;

        updateAnim();
	}
	
	void Update () {
		draw_sin += 0.005f;
        if (draw_sin > 1){
            draw_sin = 0;
        }

        float temp_sin = Mathf.Sin(draw_sin * 2 * Mathf.PI);

        transform.position = new Vector3(transform.position.x, y_pos + (temp_sin * 0.15f), transform.position.z);
	}

    public void setType(int val) {
        type = val;
        updateAnim();
    }

    private void updateAnim() {
        string temp_anim = "aItem_Shovel";

        if (type == 1){
            temp_anim = "aItem_Ladder";
        }
        else if (type == 2){
            temp_anim = "aItem_Fish";
        }
        else if (type == 3){
            temp_anim = "aItem_Ice";
        }
        else if (type == 4){
            temp_anim = "aItem_Jump";
        }
        else if (type == 5){
            temp_anim = "aItem_Key";
        }
        else if (type == 6){
            temp_anim = "aItem_Claw";
        }
        else if (type == 7){
            temp_anim = "aItem_Light";
        }
        else if (type == 8){
            temp_anim = "aItem_Teleport";
        }
        else if (type == 9){
            temp_anim = "aItem_Electricity";
        }

        anim.Play(temp_anim);
    }

    public int getType(){
        return type;
    }
}
