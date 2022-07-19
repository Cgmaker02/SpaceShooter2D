using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Missle : MonoBehaviour
{
    private float _speed = 2f;
    private Vector3 _pos;
    private Vector3 _axis;
    private float _frequency = 2f;
    private float _amplitude = 2f;
    private bool _isFireBackwards;

    // Start is called before the first frame update
    void Start()
    { 
        _pos = transform.position;
        _axis = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
       if(_isFireBackwards == false)
        {
            MoveDown();
        }
       else if(_isFireBackwards == true)
        {
            MoveUP();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }

    void MoveUP()
    {
        _pos += Vector3.up * Time.deltaTime * _speed;

        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        _pos += Vector3.down * Time.deltaTime * _speed;

        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    public void FireBackwards()
    {
        _isFireBackwards = true;
    }
}
