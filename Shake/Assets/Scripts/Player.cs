using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int attackCount, counter, stopCounter;
    private bool accel, jump, attack, form=true, ground, jumping;
    [SerializeField]
    GameObject[] model;
    private float jumpPower, accelPower;
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rb;
    private UdpReceiver receiver;
    [SerializeField]
    Transform mycamera;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * moveSpeed;
        receiver = new UdpReceiver(AccelAction);
        receiver.UdpStart();
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb.velocity;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        if(transform.position.x > 250)
        {
            GameManager.instance.run = false;
            GameManager.instance.clear = true;
            rb.Sleep();
        }
        //Debug.Log("speed: "+rb.velocity);
        var sub = mycamera.position.x - transform.position.x;
        if (transform.position.y < -5 || sub > 10.5)
        {
            Debug.Log("gameover");
            GameManager.instance.run = false;
            rb.Sleep();
        }

        if(sub < -10) transform.position = new Vector3(mycamera.position.x+10, transform.position.y);
        if (transform.position.y > 5.5) transform.position = new Vector3(transform.position.x, 5.5f);

        if (jumping)
        {
            rb.AddForce(Vector2.up * 1000);
            jumping = false;
        }
        if (attack)
        {
            attackCount = 5;
            if (form)
            {
                Debug.Log("change");
                model[0].SetActive(false);
                model[1].SetActive(true);
                form = false;
            }
            attack = false;
        }
        if (attackCount > 0)
        {
            attackCount--;
            if (attackCount == 0)
            {
                model[0].SetActive(true);
                model[1].SetActive(false);
                form = true;
            }
        }

        if (counter > 3)
        {
            //Debug.Log("check");
            if (jump)
            {
                rb.AddForce(Vector2.up * jumpPower / counter/ ((float)counter / 30 + 3) * 5);
                //Debug.Log("jump");
            }
            else if (accel)
            {
                if (accelPower < 500)
                {
                    rb.velocity = Vector2.right * (1 + accelPower / 500) * moveSpeed;
                }
                else rb.velocity = Vector2.right * 2 * moveSpeed;
                //Debug.Log("check");
            }
        }
    }

    private void AccelAction(float x, float y, float z)
    {
        Debug.Log("flag: "+jump+", "+accel);
        if (x > 7.5f || x < -7.5f) attack = true;
        else if (jump)
        {
            counter++;
            jumpPower += (Mathf.Abs(y) < 8) ? Mathf.Abs(y) : 20;
            if (-1.5 > y || y  > 1.5)
            {
                stopCounter = 8;
            }
            else
            {
                stopCounter--;
                if (stopCounter == 0)
                {
                    jump = false;
                    counter = 0;
                    jumpPower = 0;
                }
            }
        }
        else if (accel)
        {
            counter++;
            accelPower += (Mathf.Abs(x) < 8) ? Mathf.Abs(x) : 20;
            if (-1.5 > z || z > 1.5)
            {
                stopCounter = 8;
            }
            else
            {
                stopCounter--;
                if (stopCounter == 0)
                {
                    accel = false;
                    counter = 0;
                    accelPower = 0;
                }
            }            
        }
        else if (ground && (y > 6 || y < -6))
        {
            jump = true;
            //rb.AddForce(Vector2.up * 1000);
            jumping = true;
            stopCounter = 5;
        }
        else if (ground && (z > 6 || z < -6))
        {
            accel = true;
            stopCounter = 5;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Field")
        {
            ground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Field")
        {
            ground = false;
            accel = false;
            rb.velocity = Vector2.right * moveSpeed;
            //Debug.Log("exit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (attackCount > 0)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                GameManager.instance.run = false;
                rb.Sleep();
            }
        }
    }

    private void OnApplicationQuit()
    {
        receiver.UdpFinish();
    }
}
