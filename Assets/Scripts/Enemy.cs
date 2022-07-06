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


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if(_player == null)
        {
            Debug.LogError("the player is NULL");
        }

        if(_anim == null)
        {
            Debug.LogError("the animator is NULL");
        }

        _pos = transform.position;
        _axis = transform.right;
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
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
        _pos += Vector3.down * Time.deltaTime * _cyclespeed;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;


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

            if(_player != null)
            {
                _player.UpdateScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _canfireActive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.0f);
        }
    }
}
