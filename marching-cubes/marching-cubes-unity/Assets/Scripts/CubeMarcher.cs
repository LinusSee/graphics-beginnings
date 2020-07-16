using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeMarcher : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //StartCoroutine(CreateShape());
        CreateShape();
        // Update here if not IEnumerator
        UpdateMesh();
    }

    private void Update()
    {
        UpdateMesh();
    }

    //IEnumerator CreateShape()
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vertex = 0;
        int triangle = 0;
        for (int z = 0; z < zSize; z++, vertex++)
        {
            for (int x = 0; x < xSize; x++, vertex++, triangle += 6)
            {
                triangles[triangle] = vertex;
                triangles[triangle + 1] = xSize + vertex + 1;
                triangles[triangle + 2] = vertex + 1;

                triangles[triangle + 3] = vertex + 1;
                triangles[triangle + 4] = xSize + vertex + 1;
                triangles[triangle + 5] = xSize + vertex + 2;

                //yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Vector3 position = gameObject.transform.position;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i] + position, .1f);
        }
    }
}
