using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(14.0f, 2.5f, 1.0f));
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(col.gameObject);
	}
}
