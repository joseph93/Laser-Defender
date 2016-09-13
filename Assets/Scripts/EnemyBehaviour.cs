using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{

    public float health;

    public GameObject laser;
    public float projectileSpeed;
    public float shotsPerSecond;

    private ScoreKeeper scoreKeeper;
    public int scoreValue;

    public AudioClip destroyed;
    public AudioClip hit;

	// Use this for initialization
	void Start ()
	{
	    scoreKeeper = FindObjectOfType<ScoreKeeper>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float probability = shotsPerSecond * Time.deltaTime;
	    if (Random.value < probability)
	    {
	        shootBeam();
	    }
	    
	}

    public void shootBeam()
    {
        Vector3 startPosition = transform.position - new Vector3(0f, 0.7f, 0f);
        GameObject beam = Instantiate(laser, startPosition, Quaternion.identity) as GameObject;
        if (beam != null) beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -projectileSpeed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile != null)
        {
            health -= missile.getDamage();
            missile.Hit();
            AudioSource.PlayClipAtPoint(hit, transform.position);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(destroyed, transform.position);
                Destroy(gameObject);
                scoreKeeper.UpdateScore(scoreValue);
            }
        }
    }
}
