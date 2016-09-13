using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject laser;
    private LevelManager levelManager;

    public float speed;
    private float minX;
    private float maxX;

    public float padding = 1f;
    public float projectileSpeed;
    public float firingRate;

    public float startHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    //private Animator anim;
    private AudioSource playerAudio;

    private bool isDead;
    private bool isDamaged;
    
	// Use this for initialization
	void Start ()
	{
	    float distance = transform.position.z - Camera.main.transform.position.z;
	    Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
	    Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
	    minX = leftMost.x + padding;
	    maxX = rightMost.x - padding;

	    levelManager = FindObjectOfType<LevelManager>();
	    playerAudio = GetComponent<AudioSource>();
	    currentHealth = startHealth;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    navigatePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("shootLaser", 0.000001f, firingRate);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("shootLaser");
        }

	    if (isDamaged)
	    {
	        damageImage.color = flashColor;
	    }
	    else
	    {
	        damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed*Time.deltaTime);
	    }
	    isDamaged = false;
	}

    public void navigatePlayer()
    {
        if (Input.GetKey(KeyCode.A)) //go left
        {
            transform.position += Vector3.left*speed*Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) //go right
        {
            transform.position += Vector3.right*speed*Time.deltaTime;
        }

        float playerPos = Mathf.Clamp(transform.position.x, minX, maxX);

        transform.position = new Vector3(playerPos, transform.position.y, transform.position.z);

        
    }

    public void shootLaser()
    {
        GameObject beam = Instantiate(laser, transform.position + new Vector3(0f, 0.7f, 0f), Quaternion.identity) as GameObject;
        if (beam != null) beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile != null)
        {
            isDamaged = true;
            currentHealth -= missile.getDamage();
            healthSlider.value = currentHealth;
            missile.Hit();
            playerAudio.Play();
            if (IsDead() && !isDead)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        isDead = true;
        AudioSource.PlayClipAtPoint(deathClip, transform.position, 0.5f);
        Destroy(gameObject);
        levelManager.LoadLevel("Game Over");
    }
}
