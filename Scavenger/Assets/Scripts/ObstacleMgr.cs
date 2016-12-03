using UnityEngine;
using System.Collections;

public class ObstacleMgr : MonoBehaviour
{
    private int hp;
    public Sprite damage;

	// Use this for initialization
	void Start ()
    {
        hp = Random.Range(1,5);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void TakeDamage()
    {
        hp -= 1;
        GetComponent<SpriteRenderer>().sprite = damage;
        if(hp<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
