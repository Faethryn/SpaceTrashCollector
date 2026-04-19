using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private int _hitPoints = 5;

    [SerializeField]
    private GameObject _trashChunks;

    [SerializeField]
    private int _amountOfSpawnedChunks = 5;

    [SerializeField]
    private float _spawnRange = 5f;

    public void RemoveHealth(int hitPoints)
    {
        _hitPoints -= hitPoints;

        if(_hitPoints <= 0)
        {
            SpawnChunks();

            Destroy(this.gameObject);
        }
    }

    private void SpawnChunks()
    {
        for (int i = 0; i < _amountOfSpawnedChunks; i++)
        {
            float randomX = Random.Range(- _spawnRange, _spawnRange);
            float randomY = Random.Range(- _spawnRange, _spawnRange);
            float randomZ = Random.Range(- _spawnRange, _spawnRange);

            Vector3 spawnPosition = new Vector3( transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

            Instantiate(_trashChunks, spawnPosition, Quaternion.identity );
        }
    }
}
