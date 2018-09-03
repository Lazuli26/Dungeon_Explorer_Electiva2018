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
    public Transform waterTile;
    [Range(0, 100)]
    public float waterChance;
    [Range(0, 100)]
    public float waterExtension;
    public Transform player;
    public int render_view;
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
                    if ((x < mapSize.x / 2 - 5 || x > mapSize.x / 2 + 5) && (y < mapSize.y / 2 + -5 || y > mapSize.x / 2 + -5))
                    {
                        if (Random.Range(0, 100) < propChance)
                        {
                            propMat[x].Add(Random.Range(0, 3));
                        }
                        else
                            propMat[x].Add(
                                ((Random.Range(0, 100) < waterChance ||
                                (((y != 0 && propMat[x][y - 1] == 4) || (x != 0 && propMat[x - 1][y] == 4)) &&
                                Random.Range(0, 100) < waterExtension))
                                ? 4 : -1));
                    }
                    else
                        propMat[x].Add(-1);
                }
                Debug.Log(propMat[x].Count);
            }
        }
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        for (int x = Mathf.Max((int)(mapSize.x / 2 + player.position.x - render_view), 0); x < Mathf.Min((mapSize.x / 2 + player.position.x + render_view), mapSize.x); x++)
        {
            for (int y = Mathf.Max((int)(mapSize.y / 2 + player.position.z - render_view), 0); y < Mathf.Min((mapSize.y / 2 + player.position.z + render_view), mapSize.y); y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + x + (0.5f * ((mapSize.x + 1) % 2)), -0, -mapSize.y / 2 + y + (0.5f * ((mapSize.y + 1) % 2)));
                
                Transform newTile = Instantiate(propMat[x][y]==4? waterTile : tilePrefab, tilePosition,(propMat[x][y] == 4 ? waterTile : tilePrefab).rotation) as Transform;
                newTile.localScale = newTile.localScale * (1 - outlinePercent);
                newTile.parent = mapHolder;
                if (propMat[x][y] != -1 && propMat[x][y]!=4)
                {
                    Transform newProp = Instantiate(propList[propMat[x][y]], mapHolder);
                    newProp.transform.position = tilePosition;
                    newProp.parent = mapHolder;
                }
            }
        }
    }
}
