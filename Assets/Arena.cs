using UnityEngine;
using System.Collections;

/// <summary>
/// The Arena defines the playable space. It should be placed at the origin in the world. The size should not be altered at run time.
/// The barriers in the field could be linked to this class to make changing its size easier either in the editor or in real time.
/// </summary>
public class Arena : MonoBehaviour {

    public float width = 456; // hard coded defaults as I wanted a low-resolution screen ;)
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
