using UnityEngine;
using System.Collections;

public class Arena : MonoBehaviour {

    public float width = 456;
    public float height = 256;
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3(width/2 ,height/2,0), new Vector3(width, height, 1));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
