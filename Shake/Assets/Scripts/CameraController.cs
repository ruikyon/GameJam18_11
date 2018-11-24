using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private GameObject gameover;
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * moveSpeed;
        gameover.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
        //transform.position += new Vector3(0.1f, 0);
        if (!GameManager.instance.run)
        {
            GetComponent<Rigidbody2D>().Sleep();
            if (!GameManager.instance.clear) gameover.SetActive(true);
        }
	}
}
