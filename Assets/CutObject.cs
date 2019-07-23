﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
		int[] sides = new int[vertices.Length]; // 1 : above, 0 : on the plane, -1 : under

		// get the information from cutting plane
		Vector3 n = cutPlane.transform.TransformVector(cutPlane.GetComponent<MeshFilter>().mesh.normals[0]);
		Vector3 p = cutPlane.transform.position;

		// switch to global coord. and check which side the point is.
		for(int i = 0; i < vertices.Length; i++ )
		{
			float s;
			vertices[i] = v.transform.TransformPoint(vertices[i]);
			s = Vector3.Dot( vertices[i]-p, n);
			sides[i] = Math.Sign(s);
		}

		List<int> newTriangles = new List<int>();
		for(var i = 0; i < triangles.Length; i+=3)
		{
			int vi0 = triangles[i];
			int vi1 = triangles[i+1];
			int vi2 = triangles[i+2];
			// if all the three points are under the plane, keep it
			if( sides[vi0]<=0 && sides[vi1]<=0 && sides[vi2]<=0 )
			{
				newTriangles.Add( vi0 );
				newTriangles.Add( vi1 );
				newTriangles.Add( vi2 );
			}
			// otherwise, delete it
		}

		// https://stackoverflow.com/questions/1367504/converting-listint-to-int
		// update the triangle array
		mesh.triangles = newTriangles.ToArray();
	}
}
