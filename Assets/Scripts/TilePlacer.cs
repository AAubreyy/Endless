using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TilePlacer : MonoBehaviour
{
    public static event Action<Tile> OnTilePlaced;

    [SerializeField] private List<Tile> tiles;
    private Tile _currentTile;
    Vector3 _sPoint = Vector3.zero;
    private float _firstTileDistance;
    private List<GameObject> _sceneTiles = new List<GameObject>();

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float despawnDistance = 60f;
    [SerializeField] private Transform environmentParent;

    private void Start()
    {
        for (var i = 0; i < 4; i++)
        {
            PlaceTile();
        }
    }

    private void PlaceTile()
    {
        var randomTile = tiles[Random.Range(0, tiles.Count)];
        if (_currentTile != null)
            _sPoint = _currentTile.spawnPoint.transform.position;

        var goTile = Instantiate(randomTile, _sPoint, randomTile.transform.rotation, environmentParent);
        _sceneTiles.Add(goTile.gameObject);
        _currentTile = goTile;
    }

    private void Update()
    {
        //if the distance between the player and the first tile exceeds a value destroy the first tile
        //and immediately instantiate a new one in the front.
        var sceneTile = _sceneTiles[0];
        _firstTileDistance = (playerTransform.position - sceneTile.gameObject.transform.position).magnitude;
        if (_firstTileDistance > despawnDistance)
        {
            Destroy(sceneTile);
            _sceneTiles.RemoveAt(0);
            PlaceTile();
            
            OnTilePlaced?.Invoke(_currentTile);
        }
    }
}