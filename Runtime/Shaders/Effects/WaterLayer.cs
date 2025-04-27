using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLayer : MonoBehaviour
{
    [SerializeField] Vector2Int dimensions = new Vector2Int(100, 100);
    [SerializeField] int gridDensity = 1;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] MeshRenderer meshRenderer;

    Mesh mesh;

    private void Awake() => Generate();

    [ContextMenu("Force regeneration")]
    void Generate()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector2Int gridDimensions = new Vector2Int(dimensions.x * gridDensity, dimensions.y * gridDensity);
        Vector3 offset = new Vector3(dimensions.x, 0, dimensions.y) * 0.5f;

        // Calculate vertex positions
        Vector2Int vertexGridDimensions = gridDimensions + Vector2Int.one;
        int[,] vertexGrid = new int[vertexGridDimensions.x, vertexGridDimensions.y];
        for (int x = 0; x < vertexGridDimensions.x; x++)
        {
            for (int y = 0; y < vertexGridDimensions.y; y++)
            {
                // Save vertex array positions to the current count (which will become the position of the desired index once we add it onto the end)
                vertexGrid[x, y] = vertices.Count;
                Vector3 vertexPos = (new Vector3(x, 0, y) / gridDensity);
                vertices.Add(vertexPos - offset);
            }
        }

        // Calculate the correct vertices to read for each triangle and square
        //int[,,] triangleIndices = new int[gridDimensions.x, gridDimensions.y, 6];
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                int bl = vertexGrid[x, y];
                int tl = vertexGrid[x, y + 1];
                int br = vertexGrid[x + 1, y];
                int tr = vertexGrid[x + 1, y + 1];

                // Each triangle needs 3 vertices to reference, and each square needs two triangles
                // 0, 1, 2, 1, 3, 2
                // BL, TL, BR, TL, TR, BR
                /*
                triangleIndices[x, y, 0] = bl;
                triangleIndices[x, y, 1] = tl;
                triangleIndices[x, y, 2] = br;
                triangleIndices[x, y, 3] = tl;
                triangleIndices[x, y, 4] = tr;
                triangleIndices[x, y, 5] = br;
                */
                triangles.Add(bl);
                triangles.Add(tl);
                triangles.Add(br);
                triangles.Add(tl);
                triangles.Add(tr);
                triangles.Add(br);
            }
        }

        if (mesh == null) mesh = new Mesh();
        mesh.name = $"{name} (mesh)";
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
