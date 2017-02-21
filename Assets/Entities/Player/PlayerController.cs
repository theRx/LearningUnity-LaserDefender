using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject playerLaser;
	public AudioClip laserSound;
	public float shipSpeed;
	public float padding;
	public float projectileSpeed;
	public float firingRate;
	public float health;

	private float xmin;
	private float xmax;

	// Use this for initialization
	void Start()
	{
		float distance = transform.position.z-Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));

		xmin = leftMost.x+padding;
		xmax = rightMost.x-padding;
	}
	
	// Update is called once per frame
	void Update()
	{
		//Handles movement onf the player's ship
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			//ShipMovement(-shipSpeed);
			transform.position += Vector3.left*shipSpeed*Time.deltaTime;
		} else if(Input.GetKey(KeyCode.RightArrow))
		{
			//ShipMovement(shipSpeed);
			transform.position += Vector3.right*shipSpeed*Time.deltaTime;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			InvokeRepeating("FireLaser", 0.000001f, firingRate);
		} else if(Input.GetKeyUp(KeyCode.Space))
		{
			CancelInvoke("FireLaser");
		}

		//Restricts the player's movemen to the screen
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Projectile missile = col.gameObject.GetComponent<Projectile>();
		if(missile)
		{
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0)
				Die();
		}
	}

	void Die()
	{
		LevelManager manage = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		manage.LoadLevel("End Screen");
		Destroy(gameObject);
	}

	void ShipMovement(float move)
	{
		transform.position += new Vector3(move*Time.deltaTime, 0.0f, 0.0f);
	}

	void FireLaser()
	{
		AudioSource.PlayClipAtPoint(laserSound, transform.position, 1.0f);
		GameObject laser = Instantiate(playerLaser, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
	}
}
