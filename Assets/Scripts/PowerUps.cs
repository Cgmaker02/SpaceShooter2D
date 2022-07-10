using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;
   
    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            switch (_powerupID)
            {
                case 0:
                    other.transform.GetComponent<Player>().TripleShotActive();
                    Destroy(this.gameObject);
                    break;
                case 1:
                    other.transform.GetComponent<Player>().SpeedBoostActive();
                    Destroy(this.gameObject);
                    break;
                case 2:
                    other.transform.GetComponent<Player>().ShieldActive();
                    Destroy(this.gameObject);
                    break;
                case 3:
                    other.transform.GetComponent<Player>().RefillAmmo();
                    Destroy(this.gameObject);
                    break;
                case 4:
                    other.transform.GetComponent<Player>().Health();
                    Destroy(this.gameObject);
                    break;
                case 5:
                    other.transform.GetComponent<Player>().NegativePowerup();
                    Destroy(this.gameObject);
                    break;
                case 6:
                    other.transform.GetComponent<Player>().SecondaryFire();
                    Destroy(this.gameObject);
                    break;
                default:
                    Debug.Log("default value");
                    break;
            }
        }
    }
}
