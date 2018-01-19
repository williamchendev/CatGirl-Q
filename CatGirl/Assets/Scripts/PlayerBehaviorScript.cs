using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorScript : MonoBehaviour {

    //Settings
    private bool canmove;
    private Animator p_animation;
    public LayerMask ground_layer;
    private Rigidbody2D rb2d;

    //Movement
    private float spd;
    private float jump_spd;
    private int image_xscale;
    
    private float hspd;
    private bool jump;
    private bool grounded;
    private bool pick_up;
    private bool door_touch;
    private bool sleep;

    //QUEUE
    private Queue<int> q_bot;

    private bool shoot_mode;
    private int cats;

    //Controls
    private bool z_key;
    private bool x_key;
    private bool r_key;
    private bool up_key;
    private bool down_key;
    private bool left_key;
    private bool right_key;

    private bool up_keyPress;

	// Use this for initialization
	void Start () {
		//Settings
        canmove = true;
        p_animation = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9, 10, true);

        //Movement
        spd = 2f;
        jump_spd = 250f;
        image_xscale = 1;

        hspd = 0;
        jump = false;
        grounded = false;
        pick_up = false;
        door_touch = false;
        sleep = false;

        //Queue
        q_bot = new Queue<int>();

        shoot_mode = false;
        cats = 0;

        //Controls
        z_key = false;
        x_key = false;
        r_key = false;
        up_key = false;
        down_key = false;
        left_key = false;
        right_key = false;

        up_keyPress = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Controls
        Controller();

        //Player
        hspd = 0;
        if (canmove){
            if (left_key){
                hspd = -spd;
                image_xscale = -1;
            }
            else if (right_key){
                hspd = spd;
                image_xscale = 1;
            }

            if (up_keyPress){
                pick_up = true;
            }
            
            if (z_key){
                jump = true;
            }

            if (x_key){
                if (q_bot.Count != 0){
                    shoot_mode = true;
                    canmove = false;
                }
            }
        }
        else {
            if (shoot_mode){
                if (left_key){
                    image_xscale = -1;
                }
                else if (right_key){
                    image_xscale = 1;
                }

                if (!x_key){
                    shoot_mode = false;
                    canmove = true;

                    shootQbot(q_bot.Dequeue());
                }
            }
        }

        transform.localScale = new Vector3(image_xscale * 1f, 1f, 1f);

        //Animations
        string anim = "aCatGirl_Idle";

        if (!grounded){
            if (rb2d.velocity.y < 0){
                anim = "aCatGirl_JumpDown";
            }
            else {
                anim = "aCatGirl_JumpUp";
            }
        }
        else {
            if (hspd != 0){
                anim = "aCatGirl_Run";
            }
        }

        if (shoot_mode){
            if (grounded){
                if (up_key && (left_key || right_key)){
                    anim = "aCatGirl_AimUS";
                }
                else if (up_key){
                    anim = "aCatGirl_AimU";
                }
                else {
                    anim = "aCatGirl_AimS";
                }
            }
            else {
                if (down_key && (left_key || right_key)){
                    anim = "aCatGirl_AimAirDS";
                }
                else if (up_key && (left_key || right_key)){
                    anim = "aCatGirl_AimAirUS";
                }
                else if (down_key){
                    anim = "aCatGirl_AimAirD";
                }
                else if (up_key){
                    anim = "aCatGirl_AimAirU";
                }
                else {
                    anim = "aCatGirl_AimAirS";
                }
            }
        }

        if (sleep){
            anim = "aCatGirl_Sleep";
        }

        p_animation.Play(anim);

        //Door
        if (door_touch){
            if (cats >= LevelManager.Instance.catsInLevel()){
                if (up_keyPress){
                    LevelManager.Instance.nextLevel();
                }
            }
        }

        //Reset Level
        if (r_key){
            LevelManager.Instance.resetLevel();
        }
	}

    void FixedUpdate() {
        //Physics
        transform.position.Set(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);

        grounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.28125f), new Vector2(0.1875f, 0.5f), 0f, ground_layer);

        if (jump){
            if (grounded){
                rb2d.AddForce(new Vector2(0f, jump_spd));
            }
            jump = false;
        }
        rb2d.velocity = new Vector2(hspd, rb2d.velocity.y);

        //Items
        if (pick_up){
            bool picked = false;
            Rect item_check = new Rect(transform.position.x - 0.1875f, transform.position.y + 0.5f, 0.375f, 1f);
            GameObject[] temp_item_array = GameObject.FindGameObjectsWithTag("Item");
            for (int t = 0; t < temp_item_array.Length; t++){
                GameObject temp_item = temp_item_array[t];
                Rect temp_item_rect = new Rect(temp_item.transform.position.x - 0.25f, temp_item.transform.position.y + 0.25f, 0.5f, 0.5f);
                if (item_check.Overlaps(temp_item_rect)){
                    int temp_item_type = temp_item.GetComponent<ItemScript>().getType();

                    addQbot(temp_item_type);
                    Destroy(temp_item);
                    picked = true;
                    break;
                }
            }
            if (!picked){
                GameObject[] temp_treasure_array = GameObject.FindGameObjectsWithTag("Treasure");
                for (int t = 0; t < temp_treasure_array.Length; t++){
                    GameObject temp_treasure = temp_treasure_array[t];
                    Rect temp_treasure_rect = new Rect(temp_treasure.transform.position.x - 0.5f, temp_treasure.transform.position.y + 0.5f, 1f, 1f);
                    if (item_check.Overlaps(temp_treasure_rect)){
                        temp_treasure.GetComponent<TreasureBehaviorScript>().openChest();
                        picked = true;
                        break;
                    }
                }
            }
            pick_up = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Door"){
            door_touch = true;
        }
        else if (other.gameObject.tag == "Cat"){
            cats++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy"){
            canmove = false;
            sleep = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag == "Door"){
            door_touch = false;
        }
    }

    private void addQbot(int type){
        q_bot.Enqueue(type);
        LevelManager.Instance.updateGUI(q_bot);
    }

    private void shootQbot(int type){
        float s_angle = ((aimAngle() / 360f) * 2f * Mathf.PI);
        float x_displace = Mathf.Cos(s_angle) * 0.35f;
        float y_displace = Mathf.Sin(s_angle) * 0.35f;

        if (type == 0){
            GameObject bullet = Instantiate(LevelManager.Instance.getBullet(type), new Vector3(transform.position.x + x_displace, transform.position.y + y_displace, -3f), transform.rotation);
            bullet.GetComponent<ShovelBehaviorScript>().setAngle(aimAngle());
        }
        else if (type == 4){
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(new Vector2(0f, jump_spd + 25));
        }
        LevelManager.Instance.updateGUI(q_bot);
    }

    private int aimAngle(){
        int return_angle = -1;
        bool no_ground_angle = false;

        if (up_key && right_key){
            return_angle = 45;
        }
        else if (up_key && left_key){
            return_angle = 135;
        }
        else if (up_key){
            return_angle = 90;
        }
        else if (left_key){
            return_angle = 180;
        }
        else if (right_key){
            return_angle = 360;
        }

        if (down_key && right_key){
            no_ground_angle = true;
            return_angle = 315;
        }
        else if (down_key && left_key){
            no_ground_angle = true;
            return_angle = 225;
        }
        else if (down_key){
            no_ground_angle = true;
            return_angle = 270;
        }

        if (no_ground_angle){
            if (grounded){
                return_angle = -1;
            }
        }

        if (return_angle == -1){
            return_angle = (90 + (-90 * image_xscale));
        }

        return return_angle;
    }

    private void Controller (){
        z_key = false;
        x_key = false;
        r_key = false;
        up_key = false;
        down_key = false;
        left_key = false;
        right_key = false;

        z_key = Input.GetKeyDown(KeyCode.Z);
        x_key = Input.GetKey(KeyCode.X);
        r_key = Input.GetKeyDown(KeyCode.R);

        up_key = Input.GetKey(KeyCode.UpArrow);
        down_key = Input.GetKey(KeyCode.DownArrow);
        left_key = Input.GetKey(KeyCode.LeftArrow);
        right_key = Input.GetKey(KeyCode.RightArrow);

        up_keyPress = Input.GetKeyDown(KeyCode.UpArrow);
    }
}
