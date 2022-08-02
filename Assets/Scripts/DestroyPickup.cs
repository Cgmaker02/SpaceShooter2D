using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPickup : MonoBehaviour
{
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Powerup")
        {
            GameObject enemyLaser = Instantiate(_enemy._laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            lasers[0].AssignEnemyLaser();
            lasers[1].AssignEnemyLaser();
            Debug.Log("Powerup destroy");
        }
    }
}
