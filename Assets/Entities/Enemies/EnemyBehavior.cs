using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
	public GameObject enemyLaser;
	public AudioClip laserSound;
	public AudioClip explosion;
	public float health;
	public float projectileSpeed;
	public float fireRate; //shots per second
	public int scoreValue;

	private ScoreKeeper scoreKeeper;

	void Start()
	{
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void Update()
	{
		float fireProb = Time.deltaTime * fireRate; //probability of enemy firing this Update()
		if(Random.value < fireProb)
			FireLaser();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Projectile missile = col.gameObject.GetComponent<Projectile>();
		if(missile)
		{
			Debug.Log("Kablooie");
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0)
			{
				scoreKeeper.Score(scoreValue);
				AudioSource.PlayClipAtPoint(explosion, transform.position, 1.0f);
				Destroy(gameObject);
			}
		}
	}

	void FireLaser()
	{
		AudioSource.PlayClipAtPoint(laserSound, transform.position, 0.5f);
		//Debug.Log("Zap zap");
		GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
	}
}
