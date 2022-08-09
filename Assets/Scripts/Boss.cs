using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossState {Waves, Start, Waiting, Moving, Vulnerable }
    [SerializeField]
    private BossState _state = BossState.Waves;
    private Vector3 _currentPosition;
    private Vector3 _endPosition = new Vector3(0f, 10f, 0f);
    private Vector3 _moveRight = new Vector3(6f, 10f, 0f);
    private Vector3 _moveLeft = new Vector3(-6f, 10f, 0f);
    private float _speed = 2f;
    private bool _movedRight = false;
    private bool _vulnerable = false;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameObject _BossLaserPrefab;
    private bool _bossShoot = false;
    private int _maxHealth = 5;
    [SerializeField]
    private int _currentHealth;
    [SerializeField]
    private GameObject _explosionPrefab;
    private GameManager _gameManager;
    private UIManager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(VuneralbeCountdown());
       

        _currentHealth = _maxHealth;

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == BossState.Start)
        {
            if (transform.position != _endPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, _endPosition, _speed * Time.deltaTime);
            }
            else
            {
                _state = BossState.Waiting;
            }
        }

        if(_state == BossState.Waiting)
        {
            StartCoroutine(WaitAndMove(3));
        }

        if(_state == BossState.Moving)
        {
            if(_movedRight == false)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }
    }

    IEnumerator WaitAndMove(int WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        _state = BossState.Moving;
    }

    private void MoveRight()
    {
        if(transform.position != _moveRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, _moveRight, _speed * Time.deltaTime);
        }
        else
        {
            _movedRight = true;
            _state = BossState.Moving;
        }
    }

    private void MoveLeft()
    {
        if(transform.position != _moveLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, _moveLeft, _speed * Time.deltaTime);
        }
        else
        {
            _movedRight = false;
            _state = BossState.Waiting;
        }
    }

    private IEnumerator VuneralbeCountdown()
    {
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        _vulnerable = true;
        StartCoroutine(EngineBreakDown());
    }

    private IEnumerator EngineBreakDown()
    {
        StartCoroutine(Recover());
        while(_vulnerable == true)
        {
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1, 0.240566f, 0.240566f);
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1,1,1);
        }
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(7.0f);
        _vulnerable = false;
        StartCoroutine(VuneralbeCountdown());
    }

    private IEnumerator Shoot()
    {
        while (_bossShoot == true)
        {
            Vector3 offset = new Vector3(-5, 0, 0);
            yield return new WaitForSeconds(5.0f);
            Instantiate(_BossLaserPrefab, transform.position + offset, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_vulnerable == true)
        {
            if(other.tag == "Laser")
            {
                Destroy(other.gameObject);
                _currentHealth--;

                if(_currentHealth < 1)
                {
                    Vector3 offset = new Vector3(0, -5, 0);
                    Destroy(this.gameObject);
                    Instantiate(_explosionPrefab, transform.position + offset, Quaternion.identity);
                    _gameManager.GamesOver();
                    _uiManager.YouWin();
                }
            }
        }
    }

    public void StartBoss()
    {
        _state = BossState.Start;
        _bossShoot = true;
        StartCoroutine(Shoot());
    }
}
