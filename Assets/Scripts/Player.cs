using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive;
    private bool _isShieldActive;
    private bool _isSecondaryActive;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    private AudioSource _audioSource;
    [SerializeField]
    private int _shieldCount = 3;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private AudioClip _laserOut;
    [SerializeField]
    private GameObject _playerLeft;
    [SerializeField]
    private GameObject _playerRight;
    [SerializeField]
    private GameObject _secondaryPrefab;
    [SerializeField]
    private GameObject _thrusters1;
    [SerializeField]
    private GameObject _thrusters2;
    [SerializeField]
    private GameObject _thrusters3;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private GameObject _missilePrefab;
    private bool _isMissileActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      
        _audioSource = GetComponent<AudioSource>();
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        _playerLeft.SetActive(false);
        _playerRight.SetActive(false);
        _thrusters2.SetActive(false);
        _thrusters3.SetActive(false);

        if(_spawnManager == null)
        {
            Debug.LogError("The spawnmanager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("the UI Manager is NULL");
        }

        if(_gameManager == null)
        {
            Debug.LogError("the game manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("the audiosource is NULL");
        }

        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        MissileFire();
        Thrusters();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * speed * Time.deltaTime);

        //creating player boundaries
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        

        if(_isTripleShotActive == true)
        {
            Instantiate(_TripleShotPrefab, transform.position , Quaternion.identity);
            _audioSource.Play();
        }
        if(_isSecondaryActive == true)
        {
            Instantiate(_secondaryPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
        }
        if(_isTripleShotActive == false && _ammoCount > 0 && _isSecondaryActive == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _ammoCount--;
            _audioSource.Play();
            _uiManager.UpdateAmmoCount(_ammoCount, 15);
        }
        else
        {
            AudioSource.PlayClipAtPoint(_laserOut, transform.position);
        }

        
        
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _shieldCount--;
            if(_shieldCount == 2)
            {
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.magenta;
            }

            else if(_shieldCount == 1)
            {
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.red;
            }

            else if (_shieldCount == 0)
            {
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                
            }
            return;
        }

        _lives --;
       

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            StartCoroutine(_camera.GetComponent<CameraShake>().ShakeCamera());
        }

        if(_lives == 1)
        {
            _leftEngine.SetActive(true);
            StartCoroutine(_camera.GetComponent<CameraShake>().ShakeCamera());
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOver();
            _gameManager.GamesOver();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        speed = 8.5f;
        _thrusters3.SetActive(true);
        StartCoroutine(SpeedBoostCoolDown());
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        speed = 3.5f;
        _thrusters3.SetActive(false);
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldCount = 3;
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.PlayerScore(_score);
    }

    void Thrusters()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5.0f;
            _thrusters2.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3.5f;
            _thrusters2.SetActive(false);
        }
    }

    public void RefillAmmo()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmoCount(_ammoCount, 15);
    }

    public void Health()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }

        if(_lives == 2)
        {
            _leftEngine.SetActive(false);
        }

        else if(_lives == 3)
        {
            _rightEngine.SetActive(false);
        }
    }

    public void SecondaryFire()
    {
        _isSecondaryActive = true;
        _playerLeft.SetActive(true);
        _playerRight.SetActive(true);
        StartCoroutine(SecondaryCoolDown());
    }

    IEnumerator SecondaryCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSecondaryActive = false;
        _playerLeft.SetActive(false);
        _playerRight.SetActive(false);
    }

    public void NegativePowerup()
    {
        _ammoCount = 0;
        _uiManager.UpdateAmmoCount(_ammoCount, 15);
    }

    void MissileFire()
    {
       // if (_isMissileActive == true)
       // {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            }
       // }
    }

    public void MissilePowerup()
    {
        _isMissileActive = true;
        StartCoroutine(MissileCoolDown());
    }

    IEnumerator MissileCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isMissileActive = false;
    }

}
