using UnityEngine;
using System.Collections;

public class MonsterMgr : MonoBehaviour
{
    public float speed = 0.01f;

    private Rigidbody2D mRigidbody;
    private Animator mAnimator;
    private GameObject player;
    private BoxCollider2D mCollider;

    private float restTime = 1f;
    private float timer = 0;

    // Use this for initialization
    void Start ()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (GameMgr.Instance.food <= 0 || GameMgr.Instance.hp <= 0)
            return;
        if (Vector2.Distance(transform.position, player.transform.position) <= 5)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 1.2)
            {
                timer += Time.deltaTime;
                if (timer > restTime)
                {
                    mAnimator.SetTrigger("Attack");
                    player.SendMessage("TakeDamage");
                    timer = 0;
                }
            }
            else
            {
                Vector3 offset = (player.transform.position - transform.position).normalized * speed;
                Vector2 position = transform.position + offset;
                mCollider.enabled = false;
                RaycastHit2D hit = Physics2D.Linecast(transform.position, position);
                mCollider.enabled = true;
                if (hit.transform == null)
                {
                    mRigidbody.MovePosition(position);
                }
                else
                {
                    switch (hit.transform.tag)
                    {
                        case "Wall":
                            break;
                        case "Obstacle":
                            {
                                mAnimator.SetTrigger("Attack");
                                hit.collider.SendMessage("TakeDamage");
                            }
                            break;
                        case "Food":
                            {
                                mRigidbody.MovePosition(position);
                            }
                            break;
                            case "Player":

                                break;
                    }
                }
            }
        }
	}
}
