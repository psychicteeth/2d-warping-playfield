using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralMesh : MonoBehaviour {

    Mesh mesh;

    List<Vector3> verts = new List<Vector3>();
    List<Color> cols = new List<Color>();
    List<Vector3> normals = new List<Vector3>();
    List<int> tris = new List<int>();

    public void Tri(Vector3 p1, Vector3 p2, Vector3 p3, Color col, Vector3 normal)
    {
        int n = verts.Count;
        verts.Add(p1);
        verts.Add(p2);
        verts.Add(p3);
        tris.Add(n);
        tris.Add(n+1);
        tris.Add(n+2);
        cols.Add(col);
        cols.Add(col);
        cols.Add(col);
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);
    }

    public void Circle(float radius, int sections, Color color, float distortion = 0.0f)
    {
        float angle = 0;
        float section = Mathf.PI * 2.0f / sections;
        float[] heights = new float[sections];
        for (int i = 0; i < sections; i++)
            heights[i] = Random.Range(radius, radius + distortion);
        for (int i = 0; i < sections; i++)
        {
            int j = i + 1; if (j == sections) j = 0;
            Vector3 centre = Vector3.zero;
            Vector3 pos = new Vector3(Mathf.Sin(angle) * heights[i], Mathf.Cos(angle) * heights[i], 0);
            Vector3 pos2 = new Vector3(Mathf.Sin(angle + section) * heights[j], Mathf.Cos(angle + section) * heights[j], 0);
            Vector3 normal = new Vector3(Mathf.Sin(angle + section/ 2), Mathf.Cos(angle + section/2), 0);
            Tri(centre, pos, pos2, color, normal);
            angle += section;
        }
    }

    public void Make()
    {
        mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = cols.ToArray();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
    }

	// Use this for initialization
	void Awake () {
        Circle(20.0f, 7, Color.gray, 4.0f);
        Make();

        MeshFilter mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
