using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueGUIScript : MonoBehaviour {

	//Settings
    private int[] queue;
    private string gun_name;

    private Animator anim;
    private GameObject main_camera;

    //Objects
    public GameObject item_gui;
    public Sprite textbox;
    public Font gui_font;

    private GUIStyle style;

	void Start () {
		anim = GetComponent<Animator>();
        main_camera = GameObject.FindWithTag("MainCamera");

        transform.position = new Vector3(main_camera.transform.position.x - 6.25f, main_camera.transform.position.y - 3f, -5);

        style = new GUIStyle();
        style.font = gui_font;
        style.normal.textColor = Color.white;

        queue = new int[1];
        queue[0] = -1;
        gun_name = "Empty";
	}

    private void Update() {
        transform.position = new Vector3(main_camera.transform.position.x - 6.25f, main_camera.transform.position.y - 3f, -5);
    }

    public void updateQueue(Queue<int> temp_queue){
        if (temp_queue.Count != 0){
            queue = temp_queue.ToArray();
        }
        else {
            queue = new int[1];
            queue[0] = -1;
        }

        GameObject[] allItemGUI = GameObject.FindGameObjectsWithTag("ItemGUI");
        for (int g = 0; g < allItemGUI.Length; g++){
            Destroy(allItemGUI[g]);
        }
        graphicUpdate();
    }

    private void graphicUpdate(){
        string temp_anim = "aPortrait_Empty";
        string temp_name = "Empty";

        if (queue[0] == 0){
            temp_anim = "aPortrait_Shovel";
            temp_name = "Shovel";
        }
        else if (queue[0] == 1){
            temp_anim = "aPortrait_Ladder";
            temp_name = "Ladder";
        }
        else if (queue[0] == 2){
            temp_anim = "aPortrait_Fish";
            temp_name = "Fish";
        }
        else if (queue[0] == 3){
            temp_anim = "aPortrait_Ice";
            temp_name = "Frost";
        }
        else if (queue[0] == 4){
            temp_anim = "aPortrait_Jump";
            temp_name = "D.Jump";
        }
        else if (queue[0] == 5){
            temp_anim = "aPortrait_Key";
            temp_name = "Key";
        }
        else if (queue[0] == 6){
            temp_anim = "aPortrait_Claw";
            temp_name = "Slash";
        }
        else if (queue[0] == 7){
            temp_anim = "aPortrait_Light";
            temp_name = "Lights";
        }
        else if (queue[0] == 8){
            temp_anim = "aPortrait_Teleport";
            temp_name = "Tele.Gun";
        }
        else if (queue[0] == 9){
            temp_anim = "aPortrait_Electricity";
            temp_name = "Elec.Gun";
        }

        anim.Play(temp_anim);
        gun_name = temp_name;
        if (queue[0] != -1){
            genGUIQueue(queue);
        }
    }

    private void genGUIQueue(int[] temp_queue){
        for (int k = 0; k < temp_queue.Length; k++){
            Vector3 v_position = new Vector3(transform.position.x + (k * 0.35f) + 1.13f, transform.position.y - 0.3f, -8 + (k * 0.05f));
            GameObject item_clone = Instantiate(item_gui, v_position, transform.rotation);
            
            Animator item_anim = item_clone.GetComponent<Animator>();
            item_anim.Play(getItemAnimName(temp_queue[k]));
        }
    }

    private string getItemAnimName(int type){
        string item_anim_name = "aItem_Shovel";

        if (type == 1){
            item_anim_name = "aItem_Ladder";
        }
        else if (type == 2){
            item_anim_name = "aItem_Fish";
        }
        else if (type == 3){
            item_anim_name = "aItem_Ice";
        }
        else if (type == 4){
            item_anim_name = "aItem_Jump";
        }
        else if (type == 5){
            item_anim_name = "aItem_Key";
        }
        else if (type == 6){
            item_anim_name = "aItem_Claw";
        }
        else if (type == 7){
            item_anim_name = "aItem_Light";
        }
        else if (type == 8){
            item_anim_name = "aItem_Teleport";
        }
        else if (type == 9){
            item_anim_name = "aItem_Electricity";
        }

        return item_anim_name;
    }

    void OnGUI() {
        float draw_x = (transform.position.x * 32) - ((main_camera.transform.position.x * 32) - 240);
        float draw_y = (transform.position.y * -32) - ((main_camera.transform.position.y * -32) - 135);
        GUI.DrawTexture(new Rect(draw_x + 28, draw_y - 16, 42, 14), textbox.texture);

        GUI.Label(new Rect(draw_x + 32, draw_y - 17, 42, 11), gun_name, style);
    }
}
