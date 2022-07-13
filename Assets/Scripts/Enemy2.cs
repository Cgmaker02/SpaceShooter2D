using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private float _speed = 1f;
    [SerializeField]
    private Vector3 _direction;
    [SerializeField]
    private GameObject _misslePrefab;
    private float _canFire;
    private bool _canFireActive = true;
    private float _fireRate;
    private AudioSource _audioSource;
    private Animator _anim;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireMissle();
    }

    void CalculateMovement()
    {
        if (transform.position.x == 0f)
        {
            _direction = new Vector3(-.2f, Random.Range(-2f, 2f), 0);
        }
        if (transform.position.x > 7f)
        {
            _direction = new Vector3(-.2f, 2f, 0);
        }

        if (transform.position.x < -7f)
        {
            _direction = new Vector3(-.2f, -2f, 0);

        }
        transform.Translate(_direction * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            Destroy(this.gameObject);
        }
    }

    void FireMissle()
    {
        if (Time.time > _canFire)
        {
            if (_canFireActive == true)
            {
                _fireRate = Random.Range(3.0f, 7.0f);
                _canFire = Time.time + _fireRate;
                GameObject enemyMissle = Instantiate(_misslePrefab, transform.position, Quaternion.identity);

            }
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

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
           
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.UpdateScore(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            
            _canFireActive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }
    }
}
