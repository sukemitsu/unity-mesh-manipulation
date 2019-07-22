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
//		int[] triangles = mesh.triangles;
		//convert the vertices to global coordinate
		for(int i = 0; i < vertices.Length; i++ )
			vertices[i] = v.transform.TransformPoint(vertices[i]);

		// get the global information from cutting plane
		Vector3 n = cutPlane.transform.TransformDirection(cutPlane.GetComponent<MeshFilter>().mesh.normals[0]);
		Vector3 p = cutPlane.transform.position;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 vp = vertices[i];	// already converted to global
			float s = Vector3.Dot( vp-p, n );
			if( s > 0 )
			{
				Vector3 q = vp - p - s * n + p;	// still in global coord.
				vertices[i] = q;
				normals[i] = v.transform.InverseTransformDirection( n );
			}
		}

		//convert the vertices to global coordinate
		for(int i = 0; i < vertices.Length; i++ )
			vertices[i] = v.transform.InverseTransformPoint(vertices[i]);

		mesh.vertices = vertices;
		mesh.normals = normals;
	}
}
