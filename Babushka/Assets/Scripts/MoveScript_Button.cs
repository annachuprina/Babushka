using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class MoveScript_Button : MonoBehaviour
{
    float dirX;
    private float moveSpeed = 10f;
    Rigidbody2D rb;
    public static int score = 0;
    public Text scoreText, timeText;
    Animator babushka_animator;
    public AudioSource object_collected;
    public AudioSource object_missed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        babushka_animator = gameObject.GetComponent<Animator>();

        TimeTracker.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        babushka_animator.speed = 1;
        if (dirX == -1)
        {
            babushka_animator.SetBool("isWalking", true);
            babushka_animator.SetBool("isRotated", false);

            Debug.Log("left");
        }
        else if (dirX == 1)
        {
            babushka_animator.SetBool("isWalking", true);
            babushka_animator.SetBool("isRotated", true);
            Debug.Log("right");
        }
        else if (dirX == 0)
        {
            babushka_animator.SetBool("isWalking", false);
            //babushka_animator.SetBool("isRotated", false);
            Debug.Log("none");
        }
        timeText.text = "Time: " + TimeTracker.GetCurrentTime();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }
    void LeftButtonClick()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dish")
        {
            score += 10;
            scoreText.text = "Score: " + score;
            statsTracker.setScore(score);
            Destroy(collision.gameObject);
            object_collected.Play();

            /* if (score % 100 == 0)
             {
                 Spawn_items.increaseSpeed = true;
             }*/
        }
        else if (collision.tag == "PowerUp")
        {
            LifeTracker.life++;
            statsTracker.increasePowerUp();
            Destroy(collision.gameObject);
            object_collected.Play();
        }
        else if (collision.tag == "PowerDown")
        {
            LifeTracker.life--;
            statsTracker.increasePowerDown();
            Destroy(collision.gameObject);
            //object_missed.Play();
        }
    }

}