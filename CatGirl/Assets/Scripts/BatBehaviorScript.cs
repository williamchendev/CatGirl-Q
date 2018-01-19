using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviorScript : MonoBehaviour {

	private float max;
    private float min;
    private float time;
    private float spd;

	void Start () {

		time = Random.Range(0f, 1f);
        max = transform.position.y + 0.8f;
        min = transform.position.y - 0.8f;
        spd = 0.005f;
	}
	
	void Update () {
        time += spd;
        if (time >= 1){
            time = 0;
        }

        float t = (Mathf.Sin(time * 2 * Mathf.PI) + 1) / 2;
		transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(min, max, t), transform.position.z);
	}
}
