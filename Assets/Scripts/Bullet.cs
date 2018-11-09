using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour {

	public ParticleSystem mainParticle;
	public ParticleSystem secondaryParticle;
	public TrailRenderer trail;

	private WeaponConfig config;
	private GameObject parent;
	private Vector3 direction;
	private Collider coll;
	private float lifeTime;
	private bool dead = true;

	public void Init(WeaponConfig config, GameObject parent, Vector3 position, float angle) {
		dead = false;
		coll = GetComponent<Collider>();
		direction = Quaternion.Euler(0, angle, 0) * parent.transform.forward;
		transform.position = position;
		trail.Clear();
		this.config = config;
		this.parent = parent;
		ParticleSystem.MainModule main = mainParticle.main;
		main.startColor = config.mainColor;
		ParticleSystem.ColorOverLifetimeModule col = secondaryParticle.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys(config.colorGradient.colorKeys, col.color.gradient.alphaKeys);
		col.color = grad;
		trail.colorGradient = grad;
		trail.time = config.trailLength;
		trail.widthMultiplier = config.trailWidth;
		lifeTime = config.duration;
	}

	// Update is called once per frame
	void Update() {
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0) {
			if (!dead) {
				Die();
			} else if (lifeTime <= -config.deathTime) {
				BulletManager.Instance.Return(this);
			}
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

	public void Die() {
		dead = true;
		lifeTime = 0f;
		mainParticle.Stop();
		secondaryParticle.Stop();
	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other) { }
}