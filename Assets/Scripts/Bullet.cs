using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour {

	public ParticleSystem mainParticle;
	public ParticleSystem secondaryParticle;
	public ParticleSystem hitFlash;
	public ParticleSystem hitShockwave;
	public ParticleSystem hitSparks;
	public TrailRenderer trail;

	private WeaponConfig config;
	private WeaponConfig.BulletBehaviour behaviour;
	private GameObject parent;
	private Vector3 direction;
	private Collider coll;
	private float lifeTime;
	private bool dead = true;

	public void Init(WeaponConfig config, GameObject parent, Vector3 position, float angle) {
		mainParticle.Stop();
		secondaryParticle.Stop();
		dead = false;
		coll = GetComponent<Collider>();
		direction = Quaternion.Euler(0, angle, 0) * parent.transform.forward;
		transform.position = position;
		transform.rotation = Quaternion.Euler(direction);
		trail.Clear();
		this.config = config;
		this.parent = parent;
		behaviour = config.behaviour;
		// recolor bullet
		ParticleSystem.MainModule main = mainParticle.main;
		main.startColor = config.mainColor;
		ParticleSystem.ColorOverLifetimeModule secondaryColor = secondaryParticle.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys(config.colorGradient.colorKeys, secondaryColor.color.gradient.alphaKeys);
		secondaryColor.color = grad;
		trail.colorGradient = grad;
		trail.time = config.trailLength;
		trail.widthMultiplier = config.trailWidth;
		lifeTime = config.duration;
		// recolor hit
		ParticleSystem.MainModule flashColor = hitFlash.main;
		ParticleSystem.MainModule shockwaveColor = hitShockwave.main;
		ParticleSystem.MainModule sparksColor = hitSparks.main;
		flashColor.startColor = config.mainColor;
		shockwaveColor.startColor = config.mainColor;
		sparksColor.startColor = config.mainColor;
		mainParticle.Play();
		secondaryParticle.Play();

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
		switch (behaviour) {
			case WeaponConfig.BulletBehaviour.Straight:
				transform.Translate(direction * config.speed * Time.deltaTime);
				break;
			case WeaponConfig.BulletBehaviour.Orbit:
				Vector3 axis = new Vector3(0, 1, 0);
				transform.RotateAround(parent.transform.position, axis, config.speed * 100f * Time.deltaTime);
				Vector3 desiredPosition = (transform.position - parent.transform.position).normalized * 200f +
					parent.transform.position;
				transform.position = Vector3.MoveTowards(transform.position, desiredPosition,
					Time.deltaTime * config.distance.x);
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