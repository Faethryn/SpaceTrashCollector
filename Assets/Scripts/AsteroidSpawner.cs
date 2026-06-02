
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _asteroidPrefabs = new List<GameObject>();

    [SerializeField]
    private Vector3 _origin = Vector3.zero;

    [SerializeField]
    private Vector3 _size = new Vector3(10000f, 10000f, 10000f);

    [SerializeField]
    private int _spawnCount = 20000;

    [SerializeField]
    private Vector3 _minimumScales = new Vector3(1, 1, 1);

    [SerializeField]
    private Vector3 _maximumScales = new Vector3(10, 10, 10);

    [SerializeField]
    private int _startingseed = 10;

    private void Start()
    {
        SpawnAllAsteroids();
    }

    private void SpawnAllAsteroids()
    {
        Random.InitState(_startingseed);
        for(int i = 0; i < _spawnCount; i++)
        {
            SpawnOneAsteroid();
        }
    }

    private void SpawnOneAsteroid()
    {
        float randomX = Random.Range(_origin.x - (_size.x / 2f), _origin.x + (_size.x / 2f));
        float randomY = Random.Range(_origin.y - (_size.y / 2f), _origin.y + (_size.y / 2f));
        float randomZ = Random.Range(_origin.z - (_size.z / 2f), _origin.z + (_size.z / 2f));

        Vector3 randPos = new Vector3(randomX, randomY, randomZ);

        float randomXScale = Random.Range(_minimumScales.x, _maximumScales.x);
        float randomYScale = Random.Range(_minimumScales.y, _maximumScales.y);
        float randomZScale = Random.Range(_minimumScales.z, _maximumScales.z);

        Vector3 randScale = new Vector3(randomXScale, randomYScale, randomZScale);

        float randomXRotation = Random.Range(0, 360);
        float randomYRotation = Random.Range(0, 360);
        float randomZRotation = Random.Range(0, 360);

        Quaternion randRot = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);

        int randomInstance = Random.Range(0, _asteroidPrefabs.Count);

        GameObject tempObject = Instantiate(_asteroidPrefabs[randomInstance], randPos, randRot);

        tempObject.transform.localScale = randScale;
    }
}
