using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    private int _moveDirection;
    private bool _canDodge = true;
    [SerializeField]
    private float _speed = 2f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    public GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool _canfireActive;
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        DodgeShot();
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

    void DodgeShot()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 8.0f);
        Debug.DrawRay(transform.position, Vector3.down * 8.0f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Laser") && _canDodge == true)
            {
                _moveDirection = Random.Range(0, 2) == 0 ? -2 : 2;
                transform.position = new Vector3(transform.position.x - _moveDirection,
                    transform.position.y, transform.position.z);
                _canDodge = false;
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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

        if (other.tag == "Laser" && _canDodge == false || other.tag == "Missile")
        {
            Destroy(other.gameObject);
          
            
            
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
