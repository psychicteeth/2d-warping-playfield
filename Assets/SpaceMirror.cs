using UnityEngine;
using System.Collections;

/// This class created mirrored gameobjects as children to make them show up physically in the wrapped game arena
public class SpaceMirror : MonoBehaviour {
	
	Arena arena;

    GameObject[] gang; // the physics objects
    GameObject[] gangAvatars; // the meshes
    Light[] lights;

    public float radius = 20.0f;

    CircleCollider2D c2d = null;
    Light myLight = null;
    GameObject avatar = null;

    public void OnDrawGizmos()
    {
        if (gang != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.green;
            for (int i = 0; i < gang.Length; i++)
            {
                if (gang[i].activeSelf)
                    Gizmos.DrawWireSphere(gang[i].transform.position, radius);
            }
        }
    }

	// Use this for initialization
	void Start () {
        c2d = GetComponent<CircleCollider2D>();
        arena = GameObject.FindObjectOfType<Arena>();
        Transform t = transform.FindChild("Avatar");
        if (t != null)
        {
            avatar = t.gameObject;
            myLight = avatar.GetComponent<Light>();
        }

        if (transform.parent == null)
        {
            // optimisation here: can reduce this to 3 doppelgangers and move them around depending on where the object is on the arena.

            if (c2d != null)
            {
                gang = new GameObject[3];
                // assign our given radius to the collider
                c2d.radius = radius;
            }

            if (avatar != null)
            {
                gangAvatars = new GameObject[3]; // the avatar can rotate so it can't be attached to the collider
            }

            if (myLight != null)
            {
                lights = new Light[3];
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (c2d != null)
                {
                    gang[i] = new GameObject();
                    if (c2d != null)
                    {
                        CircleCollider2D c2dd = gang[i].AddComponent<CircleCollider2D>();
                        c2dd.radius = radius;
                    }
                    gang[i].name = name + " doppelganger " + i;
                    gang[i].transform.position = transform.position;
                    gang[i].transform.SetParent(transform);
                }

                if (avatar != null)
                {
                    gangAvatars[i] = new GameObject();
                    // the light should be on the avatar
                    if (myLight != null)
                    {
                        Light light = gangAvatars[i].AddComponent<Light>();
                        light.color = myLight.color;
                        light.range = myLight.range;
                        light.intensity = myLight.intensity;
                        // may need to update these for real time light changes
                        lights[i] = light;
                    }
                    // mesh
                    MeshRenderer myMr = avatar.GetComponent<MeshRenderer>();
                    MeshFilter myMf = avatar.GetComponent<MeshFilter>();
                    if (myMr != null)
                    {
                        if (avatar.GetComponent<MeshRenderer>() != null)
                        {
                            MeshFilter mf = gangAvatars[i].AddComponent<MeshFilter>();
                            mf.mesh = myMf.sharedMesh;
                            MeshRenderer mr = gangAvatars[i].AddComponent<MeshRenderer>();
                            mr.sharedMaterial = myMr.sharedMaterial;
                        }
                    }
                    gangAvatars[i].name = name + " avatar doppelganger " + i;
                    gangAvatars[i].transform.position = transform.position;
                    gangAvatars[i].transform.SetParent(transform);
                }
            }

        }
	}
	
	// Update is called once per frame
    void Update () {
        if (transform.position.x < 0) transform.position += new Vector3(arena.width, 0, 0);
        if (transform.position.x > arena.width) transform.position += new Vector3(-arena.width, 0, 0);
        if (transform.position.y < 0) transform.position += new Vector3(0, arena.height, 0);
        if (transform.position.y > arena.height) transform.position += new Vector3(0, -arena.height, 0);

        float maxRange = radius;
        if (myLight != null) maxRange = Mathf.Max(myLight.range, radius);
        // respot the three doppelgangers so they appear on the right side of the arena relative to the object
        bool xGutter = true;
        if (transform.position.x > maxRange && transform.position.x < arena.width - maxRange)
        {
            xGutter = false;
            if (gang != null) gang[0].SetActive(false);
            if (gangAvatars != null) gangAvatars[0].SetActive(false);
        }
        else
        {
            if (gang != null) gang[0].SetActive(true);
            if (gangAvatars != null) gangAvatars[0].SetActive(true);
            if (transform.position.x > arena.width/2)
            {
                if (gang != null) gang[0].transform.localPosition = new Vector3(-arena.width,0,0);
                if (gangAvatars != null) gangAvatars[0].transform.localPosition = new Vector3(-arena.width,0,0);
            }
            else 
            {
                if (gang != null) gang[0].transform.localPosition = new Vector3(arena.width,0,0);
                if (gangAvatars != null) gangAvatars[0].transform.localPosition = new Vector3(arena.width,0,0);
            }
        }

        bool yGutter = true;
        if (transform.position.y > maxRange && transform.position.y < arena.height - maxRange)
        {
            yGutter = false;
            if (gang != null) gang[1].SetActive(false);
            if (gangAvatars != null) gangAvatars[1].SetActive(false);
        }
        else
        {
            if (gang != null) gang[1].SetActive(true);
            if (gangAvatars != null) gangAvatars[1].SetActive(true);
            if (transform.position.y > arena.height/2) 
            {
                if (gang != null) gang[1].transform.localPosition = new Vector3(0,-arena.height,0);
                if (gangAvatars != null) gangAvatars[1].transform.localPosition = new Vector3(0,-arena.height,0);
            }
            else 
            {
                if (gang != null) gang[1].transform.localPosition = new Vector3(0,arena.height,0);
                if (gangAvatars != null) gangAvatars[1].transform.localPosition = new Vector3(0,arena.height,0);
            }
        }

        if (yGutter || xGutter)
        {
            if (gang != null) 
            {
                gang[2].SetActive(true);
                gang[2].transform.localPosition = gang[0].transform.localPosition + gang[1].transform.localPosition;
            }
            if (gangAvatars != null) 
            {
                gangAvatars[2].SetActive(true);
                gangAvatars[2].transform.localPosition = gangAvatars[0].transform.localPosition + gangAvatars[1].transform.localPosition;
            }
        }
        else
        {
            if (gang != null) gang[2].SetActive(false);
            if (gangAvatars != null) gangAvatars[2].SetActive(false);
        }

        // the light should be on the avatar
        if (myLight != null)
        {
            for (int i = 0; i < 3; i++)
            {
                lights[i].color = myLight.color;
                lights[i].range = myLight.range;
                lights[i].intensity = myLight.intensity;
            }
        }
	}
}
