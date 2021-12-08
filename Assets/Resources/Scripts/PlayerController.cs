using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 1.7f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll ^ RigidbodyConstraints2D.FreezePositionY;
    }
    void Update()
    {
        if(!bJump && Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
            bJump = true;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.position += speed * horizontalInput * Time.deltaTime * Vector3.right;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        bJump = false;
    }

    Rigidbody2D rigidbody;
    bool bJump;
    Vector2 jumpForce = new Vector2(0f, 10f);
    float speed = 4f;
}
