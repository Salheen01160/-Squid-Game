using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player = GameObject.Find("player");
    }

    void Update()
    {
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, player.transform.position, 500 * Time.deltaTime); ;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().Hit_Player();
            Destroy(gameObject);
        }
    }
}
