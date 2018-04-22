using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skydome : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Grab the mesh component and store all the normals in that mesh in an array
		Mesh mesh = this.GetComponent<MeshFilter> ().mesh;
		Vector3[] normals = mesh.normals;

		// Invert all of the normals on the mesh
		for (int count = 0; count < normals.Length; count++) {
			normals[count] = -1 * normals[count];
		}

		// Store the inverted normals back on the mesh
		mesh.normals = normals;

		// Swap the order of all of the vertices for the triangles on the mesh
		for (int count = 0; count < mesh.subMeshCount; count++) {
			int[] triangles = mesh.GetTriangles(count);

			for(int count2 = 0; count2 < triangles.Length; count2+=3){
				int temp = triangles[count2];
				triangles[count2] = triangles[count2 + 1];
				triangles[count2 + 1] = temp;
			}
			mesh.SetTriangles(triangles, count);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Rotate skydome so the clouds move
		transform.Rotate(Vector3.up * Time.deltaTime * 2);		
	}
}
