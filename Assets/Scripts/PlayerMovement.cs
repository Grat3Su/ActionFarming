using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    Rigidbody2D rb;
    Animator anim;
    public GameObject DashEffect;
    int attackIdx = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetInteger("isAttack") == 0)
		{
            if (Input.GetKeyDown(KeyCode.Space)){
                PlayerMove(true);
            }
            else
                PlayerMove(false);
		}

		if (Input.GetMouseButton(0))
		{
            if(anim.GetInteger("isAttack") == 0)
			{
                StartCoroutine(PlatAnimation("Particle/SimpleOrangeSlash"));
                // АјАн
                anim.SetInteger("isAttack", attackIdx);
                attackIdx = attackIdx == 2 ? 1 : 2;
            }            
		}
    }

    void PlayerMove(bool dash)
	{
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        anim.SetBool("isMove", true);

        Vector3 movement = new Vector3(moveX, moveY, 0).normalized * moveSpeed;// * Time.deltaTime;		
        
        if (dash)
        {
            Vector3 warpPosition = transform.position + movement.normalized * 1;
            transform.position = warpPosition;


            if (DashEffect != null)
			{
                Vector3 direction = movement- warpPosition;
                Quaternion rotation = Quaternion.LookRotation(direction);
                GameObject dashParticle = Instantiate(DashEffect, warpPosition, rotation);
                dashParticle.transform.rotation = rotation;


                StartCoroutine(AnimationEnd(dashParticle));
            }
            dash = false;
        }

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);
        transform.position = newPosition;

        if (moveX < 0|| moveY<0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moveX > 0|| moveY>0) transform.localScale = new Vector3(1, 1, 1);

        if (movement == Vector3.zero)
        {
            anim.SetBool("isMove", false);
        }
    }

    IEnumerator AnimationEnd(GameObject obj)
	{
        yield return new WaitForSeconds(0.1f);

        Destroy(obj); 
        anim.SetInteger("isAttack", 0);
    }

    IEnumerator PlatAnimation(string path)
	{
        anim.SetBool("isMove", false);
        yield return new WaitForSeconds(0.4f);
        GameObject particle = Resources.Load<GameObject>(path);

        particle = Instantiate(particle, transform.position, particle.transform.rotation);
        particle.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);  

        anim.SetInteger("isAttack", 0);
        yield return new WaitForSeconds(0.3f);

        Destroy(particle); 
    }
}
