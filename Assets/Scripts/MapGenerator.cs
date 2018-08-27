using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GenerateMap();
    }
    public Transform tilePrefab;
    public Vector2 mapSize;
    [Range(0, 1)]
    public float outlinePercent;
    [Range(0,100)]
    public float propChance;
    List<List<int>> propMat;
    public List<Transform> propList;
    public void GenerateMap()
    {
        System.Console.WriteLine(propMat);
        if (propMat == null)
        {
            propMat = new List<List<int>>();
            for (int x = 0; x < mapSize.x; x++)
            {
                propMat.Add(new List<int>());
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (Random.Range(0, 100) < propChance)
                    {
                        propMat[x].Add(Random.Range(0, 3));
                    }
                    else
                        propMat[x].Add(-1);
                }
            }
        }
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + x + (0.5f * ((mapSize.x + 1) % 2)), -0, -mapSize.y / 2 + y + (0.5f * ((mapSize.y + 1) % 2)));
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
                if (propMat[x][y] != -1)
                {
                    Transform newProp = Instantiate(propList[propMat[x][y]],mapHolder);
                    newProp.transform.position = tilePosition;
                    newProp.parent = mapHolder;
                }
            }
        }
    }
}
