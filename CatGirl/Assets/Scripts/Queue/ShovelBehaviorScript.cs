using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelBehaviorScript : MonoBehaviour {

    private float angle;
    private float spd;
    private Rigidbody2D rb2d;

    void Awake() {
        angle = 0;
        spd = 5f;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        transform.Rotate(new Vector3(transform.rotation.x + 15f, 0, 0));

        float hspd = spd * Mathf.Cos(angle);
        float vspd = spd * Mathf.Sin(angle);
        rb2d.velocity = new Vector2(hspd, vspd);
    }

    public void setAngle(int angle){
        this.angle = ((angle % 360f) / 360f) * 2f * Mathf.PI;
        if (angle == 0){
            this.angle = 2 * Mathf.PI;
        }
        transform.Rotate(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Rocks"){
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Tile"){
            Destroy(this.gameObject);
        }
    }
}
