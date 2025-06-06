using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    private BombController instance;
    public BombController Instance => instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    [Header("Bomb")]
    public GameObject bombPrefabs;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    public int bombsRemaining;

    [Header("Explosion")]

    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 2;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }


    private void Update()
    {
        if (bombsRemaining>0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlacedBomb());
        }
    }
    private IEnumerator PlacedBomb()
    {
        Vector2 positionBomb = transform.position;
        positionBomb.x = Mathf.Round(positionBomb.x);
        positionBomb.y = Mathf.Round(positionBomb.y);
        GameObject bomb = Instantiate(bombPrefabs,positionBomb,Quaternion.identity);
        bombsRemaining--;
        yield return new WaitForSeconds(bombFuseTime);
        //
        positionBomb  = bomb.transform.position;    
        positionBomb.x = Mathf.Round(positionBomb.x);
        positionBomb.y = Mathf.Round(positionBomb.y);                           

        Explosion _explosion = Instantiate(explosionPrefab,positionBomb,Quaternion.identity);
        _explosion.SetActiveRenderer(_explosion.start);
        _explosion.DestroyAfter(explosionDuration);

        Explode(positionBomb, Vector2.up, explosionRadius);
        Explode(positionBomb, Vector2.down, explosionRadius);
        Explode(positionBomb, Vector2.left, explosionRadius);
        Explode(positionBomb, Vector2.right, explosionRadius);
        Destroy(bomb);
        bombsRemaining++;
    }
    
    private void Explode(Vector2 position,Vector2 dir,int length)
    {
        if (length <= 0) return;
        position += dir;

        if (Physics2D.OverlapBox(position,Vector2.one/2,0f,explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }
        Explosion _explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        _explosion.SetDirection(dir);
        _explosion.SetActiveRenderer(length>1?_explosion.middle:_explosion.end);
        _explosion.DestroyAfter(explosionDuration);

        Explode(position,dir,length-1);
    }
    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);
        if (tile != null)
        {
            Instantiate(destructiblePrefab,position,Quaternion.identity);
            destructibleTiles.SetTile(cell,null);
        }
    }
    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }
    public void AddRadius()
    {
        explosionRadius++;
    }
}
