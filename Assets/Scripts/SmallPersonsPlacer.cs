using UnityEngine;
using Random = UnityEngine.Random;

public class SmallPersonsPlacer : MonoBehaviour
{
    // [SerializeField] private GameObject npcPrefab;
    [SerializeField] private LittlePerson npc;
    // [SerializeField] private Material matRed, matGreen, matBlue;

    public enum EntityColorCode
    {
        Red,
        Green,
        Blue
    }

    private EntityColorCode _entityColorCode;

    [SerializeField] private float backOffsetRange;
    [SerializeField] private float columnSpread = 4f;
    [SerializeField] private int columnLength = 12;

    private Vector3 _tileSpawnPoint;
    private Vector3 _backShifted;

    //NPCs are placed in various patterns on the surface of the tiles.
    //generate 2d patterns and project them vertically onto the tiles to get the placement points.
    //pattern creator should know about the size(just  length actually) of the tile.

    private void Start()
    {
        TilePlacer.OnTilePlaced += PlaceFolk;
    }

    private void PlaceFolk(Tile tile)
    {
        _tileSpawnPoint = tile.spawnPoint.transform.position;
        _backShifted = new Vector3(_tileSpawnPoint.x, _tileSpawnPoint.y,
            _tileSpawnPoint.z - 10f);

        var rNum0 = RandomExcept();
        var rNum1 = RandomExcept(rNum0);
        var rNum2 = RandomExcept(rNum0,rNum1);

        for (var i = 0; i < columnLength; i++)
        {
            var shifted = new Vector3(_backShifted.x, _backShifted.y, _backShifted.z - columnSpread * i);

            if (Physics.Raycast(shifted + Vector3.up * 8f, Vector3.down, out var hit))
            {
                // each column's spawn should have a unique color code.
                npc.SetColorCode((EntityColorCode)rNum0);
                Instantiate(npc.gameObject, hit.point, Quaternion.identity);
                npc.SetColorCode((EntityColorCode)rNum1);
                Instantiate(npc.gameObject, hit.point + Vector3.left * 1.5f, Quaternion.identity);
                npc.SetColorCode((EntityColorCode)rNum2);
                Instantiate(npc.gameObject, hit.point + Vector3.right * 1.5f, Quaternion.identity);
            }
        }
    }

    private int RandomExcept(int exception0 = 999, int exception1 = 999)
    {
        var colorsLength = System.Enum.GetValues(typeof(EntityColorCode)).Length;
        int num;
        do
        {
            num = Random.Range(0, colorsLength);
        } while (num == exception0 || num == exception1);

        return num;
    }

    private void Update()
    {
        Debug.DrawRay(_tileSpawnPoint, Vector3.up * 100f, UnityEngine.Color.magenta);
        Debug.DrawRay(_backShifted, Vector3.up * 100f, UnityEngine.Color.green);
    }
}