using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutObject : MonoBehaviour
{
	public GameObject cutPlane;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if( Input.GetKeyDown("space") )
		{
			Cut(gameObject, cutPlane);
		}
    }

// https://docs.unity3d.com/ScriptReference/Mesh.html
// Cuts the victim GameObject v with the cutting plane cutPlane,
// Keeps the triangle that is under the cutting plane.
	void Cut(GameObject v, GameObject cutPlane)
	{
		// get the mesh data from GameObject v
		Mesh mesh = v.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		int[] triangles = mesh.triangles;
		for(int i = 0; i < vertices.Length; i++ )
			vertices[i] = v.transform.TransformPoint(vertices[i]);

		// get the information from cutting plane
		Vector3 n = cutPlane.transform.TransformVector(cutPlane.GetComponent<MeshFilter>().mesh.normals[0]);
		Vector3 p = cutPlane.transform.position;


		List<int> newTriangles = new List<int>();

		for(var i = 0; i < triangles.Length; i+=3)
		{
			float s0, s1, s2;
			s0 = Vector3.Dot( vertices[ triangles[i]  ]-p, n );
			s1 = Vector3.Dot( vertices[ triangles[i+1]]-p, n );
			s2 = Vector3.Dot( vertices[ triangles[i+2]]-p, n );
			// if all the three points are under the plane, keep it
			if( s0 <= 0 && s1 <= 0&& s2 <= 0 )
			{
				newTriangles.Add( triangles[i] );
				newTriangles.Add( triangles[i+1] );
				newTriangles.Add( triangles[i+2] );
			}
			// otherwise, delete it
		}

		// https://stackoverflow.com/questions/1367504/converting-listint-to-int
		// update the triangle array
		mesh.triangles = newTriangles.ToArray();
	}
}
