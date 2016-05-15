using UnityEngine;
using System.Collections;

public class testcontrol : MonoBehaviour {
    Rigidbody2D r ;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow ))r.AddForce(Vector2.up * 500);
        if (Input.GetKey(KeyCode.DownArrow ))r.AddForce(Vector2.down * 500);
        if (Input.GetKey(KeyCode.LeftArrow ))r.AddForce(Vector2.left * 500);
        if (Input.GetKey(KeyCode.RightArrow ))r.AddForce(Vector2.right * 500);
	}
}
