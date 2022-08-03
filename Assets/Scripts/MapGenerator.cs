using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { PerlinNoiseMap, IsingMap, TemperatureMap }
    public enum TemperatureMode {FallOffMap, HighTemperatureRightWall}
    public DrawMode drawMode;

    public const int mapChunkSize = 500;
    public Renderer textureRenderer;
    public bool autoUpdate;

    [Header ("Perlin Noise Settings")]
    public float noiseScale = 20;
    public int octaves = 7;
    [Range(0, 1)]
    public float persistance = .5f;
    public float lacunarity = 2;
    public int seed;
    public Vector2 offset;

    [Header ("Ising Model Settings")]
    public int numIsingSteps;
    public int numOfIsingStep;
    
    [Header ("Temperature Settings")]
    public TemperatureMode temperatureMode;
    public float temperatureMultiplier = .4f;

    float[,] magMap;
    public Dictionary<int, float[,]> evolutionHistory = new Dictionary<int, float[,]>(0);

    private void OnValidate() {
        if (numOfIsingStep >= numIsingSteps){
            numOfIsingStep = numIsingSteps - 1;
        } else if (numOfIsingStep < 0){
            numOfIsingStep = 0;
        }
        Dictionary<int, float[,]> evolutionHistory = new Dictionary<int, float[,]>(numIsingSteps);
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void Start()
    {
        magMap = GenerateRandomMagMapData(); 
    }
    public void Update()
    {
        magMap = MagneticEvolution.IsingStep(magMap, TemperatureMap(), temperatureMultiplier);
        DrawTexture(TextureGenerator.TextureFromHeightMap(magMap));
    }

    public void DrawMapInEditor()
    {
        if (drawMode == DrawMode.PerlinNoiseMap)
        {
            float[,] mapData = GeneratePerlinNoiseMapData(Vector2.zero);
            DrawTexture(TextureGenerator.TextureFromHeightMap(mapData));
        }
        else if (drawMode == DrawMode.IsingMap)
        {
            if (evolutionHistory.Count != 0){
                DrawTexture(TextureGenerator.TextureFromHeightMap(evolutionHistory[numOfIsingStep]));
            } else {
                float[,] mapData = GenerateRandomMagMapData();
                DrawTexture(TextureGenerator.TextureFromHeightMap(mapData));
            }
        } else if (drawMode == DrawMode.TemperatureMap){
            DrawTexture(TextureGenerator.TextureFromHeightMap(TemperatureMap()));
        }
    }

    public float[,] TemperatureMap(){

        float[,] mapData = new float[mapChunkSize, mapChunkSize];

        if (temperatureMode == TemperatureMode.FallOffMap){
            mapData = TemperatureMaps.GenerateFalloffMap(mapChunkSize);
        } else if (temperatureMode == TemperatureMode.HighTemperatureRightWall){
            mapData = TemperatureMaps.HighTemperatureWall(mapChunkSize);
        }
        return mapData;
    }

    public void IsingModelEvolve(){
        magMap = GenerateRandomMagMapData();
        for (int iterations = 0; iterations < numIsingSteps; iterations++){
            float [,] mapCopy = (float[,])magMap.Clone();
            evolutionHistory.Add(iterations,mapCopy);
            magMap = MagneticEvolution.IsingStep(magMap, TemperatureMap(), temperatureMultiplier);
        }
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