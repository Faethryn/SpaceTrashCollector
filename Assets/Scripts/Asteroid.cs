using System.Collections;
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

    [SerializeField]
    ShaderValueChange _shaderChange;

    private void Awake()
    {
        _shaderChange = this.GetComponent<ShaderValueChange>();
    }

    public void RemoveHealth(int hitPoints)
    {
        _hitPoints -= hitPoints;
        PlayHitColour();

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

    private void PlayHitColour()
    {
        _shaderChange.TweenChange(1, 0.1f);
        StopAllCoroutines();
        StartCoroutine(CoReturnDelayed());
    }

    private IEnumerator CoReturnDelayed()
    {
        yield return new WaitForSeconds(0.1f);

        _shaderChange.TweenChange(0, 0.1f);

        yield return null;
    }
}
