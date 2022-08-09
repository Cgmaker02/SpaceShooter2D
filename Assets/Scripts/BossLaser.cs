using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private float _speed = 2f;
    private Vector3 _pos;
    private Vector3 _axis;
    private float _frequency = 2f;
    private float _amplitude = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _pos = transform.position;
        _axis = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        _pos += Vector3.down * Time.deltaTime * _speed;

        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Destroy(this.gameObject);
        }
    }
}
