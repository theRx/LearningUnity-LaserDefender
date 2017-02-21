using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 1.0f);
	}
}