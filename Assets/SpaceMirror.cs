using UnityEngine;
using System.Collections;

/// SpaceMirror creates virtual copies of a gameobject so that they can show up when the object goes off one edge of the play field.
/// It deals with circle colliders and meshes and lights.
public class SpaceMirror : MonoBehaviour {
	
	Arena arena; // reference to the arena so we can refer to its size.

    GameObject[] gang; // the physics objects.
    GameObject[] gangAvatars; // the meshes / etc.
    Light[] lights; // references to the lights so they can be updated in real time.

    public float radius = 20.0f; // define the size of the object in this class. Later it could be elsewhere and just referred to from this class.

    // various other references we use.
    CircleCollider2D c2d = null;
    Light myLight = null;
    GameObject avatar = null;

    // use meshes to reproduce visuals across arena edges? If not, we use cameras.
    public bool UseMeshDuplication = false;

    public void OnDrawGizmos()
    {
        // only draw the gang if it exists
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
        // if there's no collision object then we don't need to make collision doppelgangers
        c2d = GetComponent<CircleCollider2D>();

        arena = GameObject.FindObjectOfType<Arena>();

        // try to find the avatar (mesh + light etc) - unlikely, but some things might not have one e.g. solitary lights
        Transform t = transform.FindChild("Avatar");
        if (t != null)
        {
            avatar = t.gameObject;
            myLight = avatar.GetComponent<Light>();
        }

        if (transform.parent == null)
        {
            // optimisation here: instead of having 8 doppelgangers surrounding us, reduce this
            // to 3 doppelgangers and move them around depending on where the object is on the arena.
            // so if the object is in the lower left of the arena then the duplicated need to be in the
            // right and top regions.

            if (c2d != null)
            {
                gang = new GameObject[3];
                // assign our given radius to the collider
                c2d.radius = radius;
            }

            if (avatar != null)
            {
                gangAvatars = new GameObject[3]; 
                // the avatar can rotate so it can't be attached to the collider! 
                // If the original collider rotates, the children rotate along with it and the whole
                // frame of duplicates stops matching the orientation of the arena.
            }

            if (myLight != null)
            {
                lights = new Light[3];
            }
            
            for (int i = 0; i < 3; i++)
            {
                // only bother creating objects to hold the collision components if there is a collider on the original object.
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
                    // set the parent so that it moves exactly as the original object does
                    gang[i].transform.SetParent(transform);
                }

                if (avatar != null)
                {
                    gangAvatars[i] = new GameObject();
                    // the light should be on the avatar, not the collider, so that it can rotate if it's a spot light
                    if (myLight != null)
                    {
                        Light light = gangAvatars[i].AddComponent<Light>();
                        light.color = myLight.color;
                        light.range = myLight.range;
                        light.intensity = myLight.intensity;
                        // may need to update these for real time light changes
                        lights[i] = light;
                    }
                    if (UseMeshDuplication)
                    {
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
        // warp to the other side of the arena if we've gone outside.
        if (transform.position.x < 0) transform.position += new Vector3(arena.width, 0, 0);
        if (transform.position.x > arena.width) transform.position += new Vector3(-arena.width, 0, 0);
        if (transform.position.y < 0) transform.position += new Vector3(0, arena.height, 0);
        if (transform.position.y > arena.height) transform.position += new Vector3(0, -arena.height, 0);

        // update the light duplicates in case the light changed its range or colour etc
        if (myLight != null)
        {
            for (int i = 0; i < 3; i++)
            {
                lights[i].color = myLight.color;
                lights[i].range = myLight.range;
                lights[i].intensity = myLight.intensity;
            }
        }

        // we turn off the duplicate entities if they're not required. They're only needed if we're near the edge of the screen.
        float maxRange = radius;
        if (myLight != null) maxRange = Mathf.Max(myLight.range, radius);
        // respot the three doppelgangers so they appear on the correct side of the arena relative to the object
        bool xGutter = true;
        if (transform.position.x > maxRange && transform.position.x < arena.width - maxRange)
        {
            // not in the gutter so we can diable the x duplicates
            xGutter = false;
            if (gang != null) gang[0].SetActive(false);
            if (gangAvatars != null) gangAvatars[0].SetActive(false);
        }
        else
        {
            // enable the x duplicates so collisions with the other side of the arena can happen.
            if (gang != null) gang[0].SetActive(true);
            if (gangAvatars != null) gangAvatars[0].SetActive(true);
            // ensure the dupes are on the right side of the object
            if (transform.position.x > arena.width/2)
            {
                // we're in the right half of the arena so make the objects appear on the left
                if (gang != null) gang[0].transform.localPosition = new Vector3(-arena.width,0,0);
                if (gangAvatars != null) gangAvatars[0].transform.localPosition = new Vector3(-arena.width,0,0);
            }
            else 
            {
                // put the dupes on the right hand side.
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

        // the corner duplicate should be on the same x and y side as the other two, so just add their positions together.
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
            // not needed as we're fully inside the arena.
            if (gang != null) gang[2].SetActive(false);
            if (gangAvatars != null) gangAvatars[2].SetActive(false);
        }

	}
}
