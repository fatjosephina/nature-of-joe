using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    List<Vector3> terrainArray = new List<Vector3>();
    public GameObject terrainCube;
    public int cols, rows;
    public Color color1, color2, color3, color4, color5, color6;

    private void Start()
    {
        GameObject terrain = new GameObject();
        terrain.name = "terrain";

        float xOffset = 0f;
        for (int i = 0; i < cols; i++)
        {
            float yOffset = 0;
            for (int j = 0; j < rows; j++)
            {
                float theta = ExtensionMethods.map(Mathf.PerlinNoise(xOffset, yOffset), 0f, 1f, 0f, 10f);

                float rotationTheta = ExtensionMethods.map(Mathf.PerlinNoise(xOffset, yOffset), 0f, 1f, 0f, 6.5f);

                Quaternion perlinRotation = new Quaternion();
                Vector3 perlinRotationVector3 = new Vector3(Mathf.Cos(rotationTheta), Mathf.Sin(rotationTheta), 0f);
                perlinRotation.eulerAngles = perlinRotationVector3 * 100f;

                terrainCube = Instantiate(terrainCube, new Vector3(i, theta, j), perlinRotation);
                terrainCube.transform.SetParent(terrain.transform);
                Renderer terrainRenderer = terrainCube.GetComponent<Renderer>();
                terrainRenderer.material.SetColor("_Color", colorTerrain(terrainCube.transform.position));

                yOffset += 0.06f;
            }
            xOffset += 0.06f;
        }
    }

    private Color colorTerrain(Vector3 terrainCubePosition)
    {
        Color terrainColor = new Vector4(1f, 1f, 1f);

        if (terrainCubePosition.y >= 0f && terrainCubePosition.y <= 3.5f)
        {
            terrainColor = color1;
        }
        else if (terrainCubePosition.y >= 3.5f && terrainCubePosition.y <= 4.5f)
        {
            terrainColor = color2;
        }
        else if (terrainCubePosition.y >= 4.5f && terrainCubePosition.y <= 5f)
        {
            terrainColor = color3;
        }
        else if (terrainCubePosition.y >= 5f && terrainCubePosition.y <= 5.5f)
        {
            terrainColor = color4;
        }
        else if (terrainCubePosition.y >= 5.5f && terrainCubePosition.y <= 6f)
        {
            terrainColor = color5;
        }
        else
        {
            terrainColor = color6;
        }

        return terrainColor;
    }
}
