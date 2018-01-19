using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    public static LevelManager Instance { get; private set; }

    //Settings
    private GameObject queue_gui;

    private int level_num;
    private string[] levels;

    private int cat_count;
    
    //Levels
    private bool level_loaded;
    private int[] level_map;

    //Assets
    public GameObject player;
    public GameObject tile_obj;
    public GameObject treasure_obj;
    public GameObject rock_obj;
    public GameObject door_obj;
    public GameObject cat_obj;
    public GameObject bat_obj;
    public GameObject queue_bot;

    public GameObject shovel_bullet;

    public Sprite[] tileset;

    //Singleton
    private void Awake() {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this.gameObject);
        }

        level_num = 0;
        levels = new string[10];
        for (int l = 0; l < levels.Length; l++){
            levels[l] = "assets/Resources/Levels/level_" + (l + 1) + ".txt";
        }
        //levels[0] = "assets/Resources/Levels/level_6.txt";
    }

	void Start () {
        //Level
        level_loaded = false;
        readTilemap(levels[level_num]);

        cat_count = 0;

        //GUI
        queue_gui = Instantiate(queue_bot);
	}
	
	void Update () {
		if (!level_loaded) {
            cat_count = 0;

            int num = 0;
            for (int h = 0; h < 8; h++) {
                for (int w = 0; w < 14; w++) {
                    if (level_map[num] == 1) {
                        setPlayer(new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), -2f));
                    }
                    else if (level_map[num] > 23 && level_map[num] < 34){
                        setTreasure(level_map[num], new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), 5f));
                    }
                    else if (level_map[num] == 34){
                        Instantiate(rock_obj, new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), 20f), transform.rotation);
                    }
                    else if (level_map[num] == 35){
                        Instantiate(door_obj, new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), 20f), transform.rotation);
                    }
                    else if (level_map[num] == 36){
                        Instantiate(cat_obj, new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), -1f), transform.rotation);
                        cat_count++;
                    }
                    else if (level_map[num] == 37){
                        Instantiate(bat_obj, new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), -1f), transform.rotation);
                    }
                    else if (level_map[num] > 1) {
                        setTile(level_map[num], new Vector3(-6.5f + (w * 1f), 3.5f - (h * 1f), 30f));
                    }
                    num++;
                }
            }
            level_loaded = true;
        }
        if (queue_gui == null){
            queue_gui = Instantiate(queue_bot);
        }
	}

    public void updateGUI (Queue<int> player_queue){
        QueueGUIScript gui_script = queue_gui.GetComponent<QueueGUIScript>();
        gui_script.updateQueue(player_queue);
    }

    public GameObject getBullet(int type){
        return shovel_bullet;
    }

    public int catsInLevel(){
        return cat_count;
    }

    //Methods
    private void readTilemap(string txt) {
        try {
            string line;
            StreamReader sr = new StreamReader(txt, Encoding.Default);

            line = sr.ReadToEnd();

            if (line != null){
                string[] entries = line.Split(',');
                if (entries.Length > 0){
                    level_map = new int[entries.Length];
                    for (int i = 0; i < entries.Length; i++){
                        level_map[i] = Int32.Parse(entries[i]);
                    }
                }
            }
        }
        catch (System.Exception e) {
            Debug.Log("error: no read");
            System.Console.WriteLine(e.Message);
        }
    }

    private void setPlayer(Vector3 v_position) {
        Instantiate(player, v_position, transform.rotation);
    }

    private void setTreasure(int treasure_type, Vector3 v_position){
        GameObject temp_treasure = Instantiate(treasure_obj, v_position, transform.rotation);
        temp_treasure.GetComponent<TreasureBehaviorScript>().setItem(treasure_type - 24);
    }

    private void setTile(int tile_type, Vector3 v_position) {
        GameObject temp_tile = Instantiate(tile_obj, v_position, transform.rotation);
        SpriteRenderer sr = temp_tile.GetComponent<SpriteRenderer>();

        Sprite set_sprite;

		if (tile_type > 1 && tile_type < 23){
            set_sprite = tileset[tile_type - 2];
        }
        else {
            set_sprite = tileset[21];
        }

        sr.sprite = set_sprite;
    }

    public void resetLevel(){
        SceneManager.LoadScene("LevelScene");
        level_loaded = false;
    }

    public void nextLevel(){
        SceneManager.LoadScene("LevelScene");
        if (level_num < levels.Length - 1){
            level_num++;
        }
        readTilemap(levels[level_num]);
        level_loaded = false;
    }

    public void selectLevel(int player_level){
        SceneManager.LoadScene("LevelScene");
        if (player_level > -1 && player_level < levels.Length - 1){
            level_num = player_level;
        }
        readTilemap(levels[level_num]);
        level_loaded = false;
    }
}