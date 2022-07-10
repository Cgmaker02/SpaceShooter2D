using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private int _maxEnemies = 5;
    [SerializeField]
    private int _currentEnemies = 0;
    [SerializeField]
    private Text _waveText;
    private int _wave = 1;
    private float _timeToSpawn = 5.0f;


   public void StartSpawning()
    {
        StartCoroutine(ShowWave());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnSecondaryFire());
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false)
        {
            if (_maxEnemies > _currentEnemies)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-7f, 7f), 7f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _currentEnemies++;
            }
            else if(_maxEnemies <= _currentEnemies && _wave < 3)
            {
                _currentEnemies = 0;
                _maxEnemies = _maxEnemies * 2;
                _timeToSpawn -= .3f;
                _wave++;
                StartCoroutine(ShowWave());

            }
            else
            {
                _stopSpawning = true;
            }
                yield return new WaitForSeconds(_timeToSpawn);
            
        }
       
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-7f, 7f), 7f, 0);
            int randompowerup = Random.Range(0, 6);
            Instantiate(powerups[randompowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }


    IEnumerator SpawnSecondaryFire()
    {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(40f, 80f));
            Vector3 posToSpawn = new Vector3(Random.Range(-7f, 7f), 7f, 0);
            Instantiate(powerups[6], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    IEnumerator ShowWave()
    {
        _waveText.text = "Wave " + _wave;
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _waveText.gameObject.SetActive(false);
    }
}
