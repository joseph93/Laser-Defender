using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] easyEnemies;
    public GameObject[] mediumEnemies;
    public GameObject[] hardEnemies;
    private int roundCount = 0;

    public float width;
    public float height;

    private bool movingRight = true;
    private float speed;

    private float xMax;
    private float xMin;

    private float spawnDelay = 0.5f;
	// Use this for initialization
	void Start ()
	{

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
	    xMax = rightBoundary.x;
	    xMin = leftBoundary.x;

	    float widthOfScreen = rightBoundary.x - leftBoundary.x;
	    speed = widthOfScreen/5;

        SpawnUntilFull();
	}

    public void SpawnUntilFull()
    {
        Transform nextPosition = NextFreePosition();
        if (nextPosition != null)
        {
            if (roundCount <= 1)
            {
                int index = Random.Range(0, easyEnemies.Length);
                GameObject enemy =
                    Instantiate(easyEnemies[index], nextPosition.position, Quaternion.identity) as GameObject;
                if (enemy != null) enemy.transform.SetParent(nextPosition);
            }
            else if (roundCount > 1 && roundCount <= 3)
            {
                int index = Random.Range(0, mediumEnemies.Length);
                GameObject enemy =
                    Instantiate(mediumEnemies[index], nextPosition.position, Quaternion.identity) as GameObject;
                if (enemy != null) enemy.transform.SetParent(nextPosition);
            }
            else if (roundCount > 3)
            {
                int index = Random.Range(0, hardEnemies.Length);
                GameObject enemy =
                    Instantiate(hardEnemies[index], nextPosition.position, Quaternion.identity) as GameObject;
                if (enemy != null) enemy.transform.SetParent(nextPosition);
            }
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    // Update is called once per frame
	void Update ()
	{
	    if (movingRight)
	    {
	        transform.position += Vector3.right*speed*Time.deltaTime;
	    }
	    else
	    {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

	    float rightEdgeOfFormation = transform.position.x + 0.5f*width;
	    float leftEdgeOfFormation = transform.position.x - 0.5f*width;
	    if (leftEdgeOfFormation <= xMin)
	    {
	        movingRight = true;
	    }
        else if (rightEdgeOfFormation >= xMax)
        {
            movingRight = false;
        }

	    if (AllMembersDead())
	    {
	        print("Formation is empty.");
	        roundCount++;
            SpawnUntilFull();
	    }
	}

    public Transform NextFreePosition()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount <= 0)
            {
                return child;
            }
        }
        return null;
    }

    public bool AllMembersDead()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
                return false;
        }
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
}
