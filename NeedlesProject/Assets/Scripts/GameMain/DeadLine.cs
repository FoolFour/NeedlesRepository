using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{

    private SpawnManager m_SpawnManager;

    // Use this for initialization
    void Start()
    {
        m_SpawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<GameCamera.Camera>().CameraReset(new Vector3(m_SpawnManager.GetCurrentSpawnPoint().x,
                                                         m_SpawnManager.GetCurrentSpawnPoint().y,
                                                            Camera.main.transform.position.z));
            other.gameObject.GetComponent<Player>().Dead();
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.transform.position = m_SpawnManager.GetCurrentSpawnPoint();
        }
    }
}
