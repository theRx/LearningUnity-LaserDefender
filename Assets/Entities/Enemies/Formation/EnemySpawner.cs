using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	public float width = 10.0f;
	public float height = 5.0f;
	public float padding;
	public float enemySpeed;
	public float spawnDelay;

	private bool moveLeft = true;
	private float xmin;
	private float xmax;

	// Use this for initialization
	void Start()
	{
		SpawnEnemies();

		//Setting the boundaries for movement
		float distance = transform.position.z-Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));

		xmin = leftMost.x+padding;
		xmax = rightMost.x-padding;

	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0.0f));
	}
	
	// Update is called once per frame
	void Update()
	{
		if(moveLeft)
		{
			transform.position += Vector3.left*enemySpeed*Time.deltaTime;
			if(transform.position.x <= xmin)
				moveLeft = false;
		} else if(!moveLeft)
		{
			transform.position += Vector3.right*enemySpeed*Time.deltaTime;
			if(transform.position.x >= xmax)
				moveLeft = true;
		}

		//Restricts the enemies' movement to the screen
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);

		if(AllMembersDead())
		{
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}

	bool AllMembersDead()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount > 0)
				return false;
		}
		return true;
	}

	Transform NextFreePosition()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount <= 0)
				return childPositionGameObject;
		}
		return null;
	}

	void SpawnEnemies()
	{
		//Instantiating an enemy on each spawn point
		foreach(Transform child in transform)
		{
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		if(freePosition)
		{
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}

		if(NextFreePosition())
			Invoke("SpawnUntilFull", spawnDelay);
	}
}
