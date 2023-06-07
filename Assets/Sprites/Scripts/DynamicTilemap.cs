using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicTilemap : MonoBehaviour
{
    public Tilemap tilemap;  // El Tilemap
    public TileBase grassTile; // El Tile de césped
    public TileBase dirtTile; // El Tile de tierra
    public TileBase stoneTile; // El Tile de piedra
    public GameObject prefab1; // El primer prefab a generar
    public GameObject prefab2;
    public Transform player;  // El jugador
    public int renderDistance = 10; // Distancia de renderizado alrededor del jugador
    public float terrainVariation = 5f; // Variación de la altura del terreno
    public float terrainOffset = -22f; // Desplazamiento de la altura del terreno

    private Vector3Int previousPlayerTilePosition;
    private HashSet<Vector3Int> playerPath = new HashSet<Vector3Int>();

    void Start()
    {
        previousPlayerTilePosition = tilemap.WorldToCell(player.position);
        GenerateTiles(previousPlayerTilePosition);
    }

    void Update()
    {
        Vector3Int playerTilePosition = tilemap.WorldToCell(player.position);
        if (playerTilePosition != previousPlayerTilePosition)
        {
            previousPlayerTilePosition = playerTilePosition;
            GenerateTiles(playerTilePosition);
            RemoveTilesOutOfRange();
        }
    }

    void GenerateTiles(Vector3Int playerTilePosition)
    {
        for (int i = -renderDistance; i < renderDistance; i++)
        {
            int terrainHeight = Mathf.FloorToInt(Mathf.PerlinNoise((playerTilePosition.x + i) * 0.05f, 0) * terrainVariation + terrainOffset);

            Vector3Int grassTilePosition = new Vector3Int(playerTilePosition.x + i, terrainHeight, playerTilePosition.z);
            Vector3Int dirtTilePosition = grassTilePosition + Vector3Int.down;
            Vector3Int stoneTilePosition = dirtTilePosition + Vector3Int.down;

            if (!playerPath.Contains(grassTilePosition))
            {
                tilemap.SetTile(grassTilePosition, grassTile);
                playerPath.Add(grassTilePosition);

                // Generar el primer prefab de forma aleatoria con una probabilidad del 10%
                if (Random.Range(0, 100) < 10)
                {
                    Vector3 prefabPosition = tilemap.CellToWorld(grassTilePosition) + new Vector3(0, 2, 0);
                    Instantiate(prefab1, prefabPosition, Quaternion.identity);
                }

                // Generar el segundo prefab de forma aleatoria con una probabilidad del 5%
                if (Random.Range(0, 100) < 5)
                {
                    Vector3 prefabPosition = tilemap.CellToWorld(grassTilePosition) + new Vector3(0, 2, 0);
                    Instantiate(prefab2, prefabPosition, Quaternion.identity);
                }
            }

            if (!playerPath.Contains(dirtTilePosition))
            {
                tilemap.SetTile(dirtTilePosition, dirtTile);
                playerPath.Add(dirtTilePosition);
            }

            if (!playerPath.Contains(stoneTilePosition))
            {
                tilemap.SetTile(stoneTilePosition, stoneTile);
                playerPath.Add(stoneTilePosition);
            }
        }
    }


    void RemoveTilesOutOfRange()
    {
        List<Vector3Int> toRemove = new List<Vector3Int>();
        foreach (Vector3Int pos in playerPath)
        {
            if (Vector3Int.Distance(new Vector3Int(pos.x, 0, 0), new Vector3Int(previousPlayerTilePosition.x, 0, 0)) > 2 * renderDistance)
            {
                toRemove.Add(pos);
            }
        }

        foreach (Vector3Int pos in toRemove)
        {
            tilemap.SetTile(pos, null);
            playerPath.Remove(pos);
        }
    }
}
