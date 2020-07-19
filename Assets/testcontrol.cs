using UnityEngine;
using System.Collections;

public class testcontrol : MonoBehaviour {
    Rigidbody2D r ;
    GameObject avatar = null;
    // Use this for initialization
    void Start () {
        Transform t = transform.Find("Avatar");
        if (t != null)
        {
            avatar = t.gameObject;
            r = GetComponent<Rigidbody2D>();
        }
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 thrust = avatar.transform.up * 500;
        bool thrusting = false;
        if (Input.GetKey(KeyCode.UpArrow )) thrusting = true;
        if (Input.GetMouseButton(1)) thrusting = true;

        if (thrusting) r.AddForce(thrust);
        float angle = Time.deltaTime * 200;
        if (Input.GetKey(KeyCode.LeftArrow )) avatar.transform.Rotate(new Vector3(0,0,angle));
        if (Input.GetKey(KeyCode.RightArrow ))avatar.transform.Rotate(new Vector3(0,0,angle));
        avatar.transform.Rotate(new Vector3(0, 0, angle * Input.GetAxis("Mouse X")));

        //if (Input.GetKeyDown(KeyCode.Escape)) Cursor.l

	}
}
