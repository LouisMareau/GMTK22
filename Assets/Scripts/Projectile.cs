using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("DATA")]
	public float speed = 10f;
	[SerializeField] private float _timeBeforeDestruction = 3f;

	[SerializeField] private Transform _rootTransform;
	[SerializeField] private Transform _meshTransform;
	private Vector3 _direction;
	private float _damage;
    public float knockback = 0;
    private bool _isSeeker = false;

	private void Awake()
	{
		if (_rootTransform == null) _rootTransform = transform;
		if (_meshTransform == null) _meshTransform = transform.GetChild(0);
	}

	public void Initialize(Vector3 origin, Vector3 direction, float damage)
	{
		_rootTransform.position = origin;
		_direction = direction.normalized;
		_damage = damage;

		Destroy(gameObject, _timeBeforeDestruction);
	}

    public void setAutopilot(bool b)
    {
        _isSeeker = b;
    }

	private void Update()
	{
        //When on Autopilot
        if (_isSeeker) {
            //find closest enemy and update direction
            var target = EnemySpawner.Instance.findClosestEnemy(this.transform.position);
            if (target != null) {
                this._direction = (target.transform.position - this.transform.position).normalized;
            }
            
        }

		_rootTransform.Translate(_direction * speed * Time.deltaTime);

		Ray ray = new Ray(_meshTransform.position, -_direction);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, speed / 10f))
		{
			if (hit.collider.tag == "Enemy")
			{
				Vector3 targetLocation = new Vector3(hit.point.x, 1, hit.point.z);

				var enemy = hit.collider.GetComponent<Enemy>();
                enemy.TakeDamage(_damage);
				// Knockback
				enemy.transform.Translate(-enemy.currentDirection * knockback);

				Destroy(gameObject);
			}
		}
	}

    //HACKY solution because if the projectile is on auto the raycast will be from inside
    //the collider and it can't detect the enemy
    //!!! not being triggered
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            this.setAutopilot(false);
            this._direction.z = 1;
        }

    }
}
