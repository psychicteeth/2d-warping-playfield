using UnityEngine;
using System.Collections;

/// <summary>
/// The Arena defines the playable space. It should be placed at the origin in the world. The size should not be altered at run time.
/// The barriers in the field could be linked to this class to make changing its size easier either in the editor or in real time.
/// </summary>
/// 
[ExecuteInEditMode()]
public class Arena : MonoBehaviour {

    public float width = 456; // hard coded defaults as I wanted a low-resolution screen ;)
    public float height = 256;

    public Camera mainCamera;
    public GameObject[] borders;
    public Camera[] cameras;
	
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
        int i = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x != 0 || y != 0)
                {
                    cameras[i].orthographicSize = mainCamera.orthographicSize;
                    cameras[i].transform.localPosition = new Vector3(((float)x + 0.5f) * width, ((float)y + 0.5f) * height, -10);
                    i++;
                }
                else
                    mainCamera.transform.localPosition = new Vector3(width / 2, height / 2, -10);
            }
        }
        borders[0].transform.localPosition = new Vector3(width * 0.5f, -250, 0);
        borders[1].transform.localPosition = new Vector3(width * 0.5f, 250 + height, 0);
        borders[2].transform.localPosition = new Vector3(-250, height * 0.5f, 0);
        borders[3].transform.localPosition = new Vector3(250 + width, height * 0.5f, 0);
    }
}
