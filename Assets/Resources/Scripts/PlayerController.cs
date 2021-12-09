using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 2.5f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.simulated = false;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {

        }
        if(!bJump && Input.GetKey(KeyCode.X))
        {
            bJump = true;
            rigidbody.simulated = true;
            rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        if(transform.position.y<0f)
        {
            bJump = false;
            rigidbody.simulated = false;
            rigidbody.velocity = Vector2.zero;
            Vector3 position = transform.position;
            position.y = 0f;
            transform.position = position;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.position += speed * horizontalInput * Time.deltaTime * Vector3.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.bounds.size);
    }

    Rigidbody2D rigidbody;
    bool bJump;
    Vector2 jumpForce = new Vector2(0f, 12f);
    float speed = 3f;
}
