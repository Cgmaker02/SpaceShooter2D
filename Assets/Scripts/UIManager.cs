using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartLevel;
    [SerializeField]
    private Text _ammoCount;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = ("Score: " + 0);
        _ammoCount.text = ("Ammo Count: 15/15");
        _gameOver.gameObject.SetActive(false);
        _restartLevel.gameObject.SetActive(false);
    }

    public void PlayerScore(int playerscore)
    {
        _scoreText.text = ("Score: " + playerscore);
    }

    public void UpdateLives(int currentlives)
    {
        _livesImg.sprite = _liveSprites[currentlives];
    }

    public void GameOver()
    {
        _gameOver.gameObject.SetActive(true);
        _restartLevel.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    public void UpdateAmmoCount(int current, int max)
    {
        _ammoCount.text = ("Ammo Count: " + current +"/" + max);
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameOver.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
