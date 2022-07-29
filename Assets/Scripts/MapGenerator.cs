using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { PerlinNoiseMap, RandomMagMap }
    public DrawMode drawMode;

    public const int mapChunkSize = 100;
    public float noiseScale = 20;

    public int octaves = 7;
    [Range(0, 1)]
    public float persistance = .5f;
    public float lacunarity = 2;
    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public Renderer textureRenderer;

    float[,] magMap;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void Start()
    {
        magMap = GenerateRandomMagMapData(); 
    }

    public void DrawMapInEditor()
    {
        if (drawMode == DrawMode.PerlinNoiseMap)
        {
            float[,] mapData = GeneratePerlinNoiseMapData(Vector2.zero);
            DrawTexture(TextureGenerator.TextureFromHeightMap(mapData));
        }
        else if (drawMode == DrawMode.RandomMagMap)
        {
            float[,] mapData = GenerateRandomMagMapData();
            DrawTexture(TextureGenerator.TextureFromHeightMap(mapData));
        }
    }

    public void Update()
    {
        magMap = MagneticEvolution.IsingStep(magMap);
        DrawTexture(TextureGenerator.TextureFromHeightMap(magMap));
    }

    float[,] GeneratePerlinNoiseMapData(Vector2 centre)
    {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset);
        return noiseMap;
    }

    float[,] GenerateRandomMagMapData()
    {
        float[,] noiseMap = Noise.GenerateMagMap(mapChunkSize, mapChunkSize);
        return noiseMap;
    }
}