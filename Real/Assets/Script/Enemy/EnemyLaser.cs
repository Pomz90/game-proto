using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
	[SerializeField] private float defDistanceRay = 100;
	public Transform laserFirepoint;
	public LineRenderer m_lineRenderer;
	Transform m_transform;
	private void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	private void Update()
	{
		ShootLaser();
	}

	void ShootLaser()
	{
		if (Physics2D.Raycast(m_transform.position, transform.right))
		{
			RaycastHit2D _hit = Physics2D.Raycast(laserFirepoint.position, transform.right);
			Draw2DRay(laserFirepoint.position, _hit.point);
		}
		else
		{
			Draw2DRay(laserFirepoint.position, laserFirepoint.transform.right * defDistanceRay); ;
		}
	}

	void Draw2DRay(Vector2 Startpos, Vector2 endPos)
	{
		m_lineRenderer.SetPosition(0, Startpos);
		m_lineRenderer.SetPosition(1, endPos);
	}

}