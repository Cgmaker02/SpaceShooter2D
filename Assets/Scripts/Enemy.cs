using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private float _frequency = 1.0f;
    private float _amplitude = 1.0f;
    private float _cyclespeed = 1.0f;
    private Vector3 _pos;
    private Vector3 _axis;
    private bool _canfireActive = true;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _enemyShields;
    private bool _isEnemyShieldActive;
    private float _distance;
    private float _ramSpeed = 2.5f;
    private float _attackRange = 3.0f;
    private float _ramMultiplier = 2.0f;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("the player is NULL");
        }

        if (_anim == null)
        {
            Debug.LogError("the animator is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

        _pos = transform.position;
        _axis = transform.right;

        int x = Random.Range(0, 100);
        if (x >= 85)
        {
            _isEnemyShieldActive = true;
        }
        else
        {
            _isEnemyShieldActive = false;
        }
        _enemyShields.SetActive(_isEnemyShieldActive);
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            if (_canfireActive == true)
            {
                _fireRate = Random.Range(3.0f, 7.0f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                lasers[0].AssignEnemyLaser();
                lasers[1].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        if (_player != null)
        {
            _distance = Vector3.Distance(_player.transform.position, this.transform.position);
            Debug.Log(_distance);
        }

        if (_distance <= _attackRange)
        {
            if (_player != null)
            {
                Vector3 direction = this.transform.position - _player.transform.position;
                direction = direction.normalized;
                this.transform.position -= direction * Time.deltaTime * (_ramSpeed * _ramMultiplier);
            }
        }
        else
        {
            _pos += Vector3.down * Time.deltaTime * _cyclespeed;
            transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;
        }


        if (transform.position.y <= -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.0f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_isEnemyShieldActive == true) //&& _enemyShields.activeInHierarchy == true)
            {
                _enemyShields.SetActive(false);
                _isEnemyShieldActive = false;
                return;
            }
            else
            {
                if (_player != null)
                {
                    _player.UpdateScore(10);
                }

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _canfireActive = false;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.0f);
            }


        }
    }


}
