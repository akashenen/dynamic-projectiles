using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ParticleSystem))]
public class Bullet : MonoBehaviour {

	private WeaponConfig config;
	private GameObject parent;
	private Vector3 direction;
	private Collider coll;
	private ParticleSystem ps;
	private TrailRenderer trail;
	private Light glow;
	private float lifeTime;

	public void Init(WeaponConfig config, GameObject parent, Vector3 position, float angle) {
		coll = GetComponent<Collider>();
		ps = GetComponent<ParticleSystem>();
		trail = GetComponentInChildren<TrailRenderer>();
		glow = GetComponentInChildren<Light>();
		direction = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
		transform.position = position;
		trail.Clear();
		this.config = config;
		this.parent = parent;
		ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
		colm.color = config.gradient;
		trail.colorGradient = config.gradient;
		trail.time = config.trailLenght;
		trail.widthMultiplier = config.trailWidth;
		glow.color = config.glowColor;
		lifeTime = config.duration;
	}

	// Update is called once per frame
	void Update() {
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0) {
			BulletManager.Instance.Return(this);
			return;
		}
		switch (config.behaviour) {
			case WeaponConfig.BulletBehaviour.Straight:
				transform.Translate(direction * config.speed * Time.deltaTime);
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other) { }
}