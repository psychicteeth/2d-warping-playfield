using UnityEngine;
using System.Collections;

public class testcontrol : MonoBehaviour {
    Rigidbody2D r ;
    GameObject avatar = null;
    // Use this for initialization
    void Start () {
        Transform t = transform.FindChild("Avatar");
        if (t != null)
        {
            avatar = t.gameObject;
            r = GetComponent<Rigidbody2D>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        // yeah yeah, what's an axis anyway.
        if (Input.GetKey(KeyCode.UpArrow ))r.AddForce(avatar.transform.up * 500);
        if (Input.GetKey(KeyCode.DownArrow ))r.AddForce(avatar.transform.up * -500);
        if (Input.GetKey(KeyCode.LeftArrow )) avatar.transform.Rotate(new Vector3(0,0,Time.deltaTime * 100));
        if (Input.GetKey(KeyCode.RightArrow ))avatar.transform.Rotate(new Vector3(0,0,Time.deltaTime * -100));
	}
}
