using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] RatEnemy _enemyPrefab;
    [SerializeField] float _spawnRate = 5f;

    private GameObject _camera;
    private BoxCollider2D _leftBoxCollider;
    private BoxCollider2D _rightBoxCollider;
    private BoxCollider2D _topBoxCollider;
    private BoxCollider2D _bottomBoxCollider;
    private BoxCollider2D[] _allColliders;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        // Getting box colliders, which are components of children of the main camera
        _leftBoxCollider = _camera.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        _rightBoxCollider = _camera.transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
        _topBoxCollider = _camera.transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>();
        _bottomBoxCollider = _camera.transform.GetChild(3).gameObject.GetComponent<BoxCollider2D>();
        // Putting colliders in array
        _allColliders = new BoxCollider2D[]{_leftBoxCollider, _rightBoxCollider, _topBoxCollider, _bottomBoxCollider};
    }

    void Start()
    {
        // Spawning enemy
        SpawnEnemy();
    }

    IEnumerator SpawnEnemy()
    {
        // Getting box collider to spawn in at random
        int colliderIndex = Random.Range(0, 4);
        BoxCollider2D collider = _allColliders[colliderIndex];
        // Getting range values for spawning in
        Vector2 colliderCenter = collider.bounds.center;
        Vector2 colliderExtents = collider.bounds.extents;
        float randomX = Random.Range(colliderCenter.x - colliderExtents.x, colliderCenter.x + colliderExtents.x);
        float randomY = Random.Range(colliderCenter.y - colliderExtents.y, colliderCenter.y + colliderExtents.y);

        Vector2 randomPos = new Vector2(randomX, randomY);

        Instantiate(_enemyPrefab, randomPos, Quaternion.identity);

        yield return new WaitForSeconds(_spawnRate);
    }
}
