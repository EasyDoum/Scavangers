using UnityEngine;
using System.Collections;

public class PlayerMgr : MonoBehaviour
{
    public AudioClip[] chopAudio;
    public AudioClip[] footstepAudio;
    public AudioClip[] damageAudio;
    public AudioClip[] eatfoodAudio;

    private AudioSource audioSource;


    public float speed=2f;

    private float restTime = 1f;
    private float timer = 0;

    private Rigidbody2D mRigidbody;

    private Vector2 position;

    private BoxCollider2D mCollider;

    private Animator mAnimator;

    public void TakeDamage()
    {
        mAnimator.SetTrigger("Damage");
        PlayMusic(damageAudio);
        GameMgr.Instance.hp -= 1;
    }

    private void PlayMusic(AudioClip[] audioClip)
    {
        int index = Random.Range(0,audioClip.Length);
        audioSource.clip = audioClip[index];
        audioSource.Play();
        
    }


	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        mRigidbody = GetComponent<Rigidbody2D>();
        position = transform.position;
        mCollider = GetComponent<BoxCollider2D>();
        mAnimator = GetComponent<Animator>();
	
	}

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Instance.food <= 0 || GameMgr.Instance.hp <= 0)
            return;
        float h = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        float v = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        if (h != 0 || v != 0)
        {
            timer += Time.deltaTime;
            if(timer>restTime)
            {
                timer = 0;
                GameMgr.Instance.food -= 1;
            }
            //GameMgr.Instance.food -= 1;
            mCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(position, position + new Vector2(h, v));
            mCollider.enabled = true;
            if (hit.transform == null)
            {
                position += new Vector2(h, v);
                mRigidbody.MovePosition(position);
                PlayMusic(footstepAudio);
            }
            else
            {
                switch (hit.transform.tag)
                {
                    case "Wall":
                        break;
                    case "Obstacle":
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                hit.collider.SendMessage("TakeDamage");
                                mAnimator.SetTrigger("Attack");
                                PlayMusic(chopAudio);
                                GameMgr.Instance.food -= 10;
                            }
                        }
                        break;
                    case "Food":
                        {
                            position += new Vector2(h, v);
                            mRigidbody.MovePosition(position);
                            PlayMusic(eatfoodAudio);
                            Destroy(hit.transform.gameObject);
                            GameMgr.Instance.food += 10;
                        }
                        break;
                    case "Monster":
                        break;
                    case "Exit":
                        {
                            GameMgr.Instance.isEnd = true;
                        }
                        break;
                       

                }
            }

        }
    }
}
