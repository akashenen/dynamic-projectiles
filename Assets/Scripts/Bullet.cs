using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store data and update each individual bullet
/// </summary>
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour {

	// Other Components
	public ParticleSystem mainParticle;
	public ParticleSystem secondaryParticle;
	public ParticleSystem hitFlash;
	public ParticleSystem hitShockwave;
	public ParticleSystem hitSparks;
	public TrailRenderer trail;

	// Private Variables
	private BulletManager manager;
	private WeaponConfig config;
	private WeaponConfig.BulletBehaviour behaviour;
	private GameObject parent;
	private Vector3 direction;
	private Collider coll;
	private float lifeTime;
	private bool dead = true;

	/// <summary>
	///  Initialization method that is called when a bullet is fired, resetting its properties
	/// </summary>
	/// <param name="config">Weapon config object containing the weapon data for the bullet</param>
	/// <param name="parent">Actor that fired the bullet</param>
	/// <param name="position">Starting position of the bullet</param>
	/// <param name="angle">Starting angle of the bullet</param>
	public void Init(WeaponConfig config, GameObject parent, Vector3 position, float angle) {
		// resetting bullet
		mainParticle.Stop();
		secondaryParticle.Stop();
		dead = false;
		coll = GetComponent<Collider>();
		direction = Quaternion.Euler(0, angle, 0) * parent.transform.forward;
		transform.position = position;
		transform.rotation = Quaternion.Euler(direction);
		trail.Clear();
		// setting parameters
		this.config = config;
		this.parent = parent;
		behaviour = config.behaviour;
		trail.time = config.trailLength;
		trail.widthMultiplier = config.trailWidth;
		lifeTime = config.duration;
		// recolor bullet
		ParticleSystem.MainModule main = mainParticle.main;
		main.startColor = config.mainColor;
		ParticleSystem.ColorOverLifetimeModule secondaryColor = secondaryParticle.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys(config.colorGradient.colorKeys, secondaryColor.color.gradient.alphaKeys);
		secondaryColor.color = grad;
		trail.colorGradient = grad;
		// recolor hit
		ParticleSystem.MainModule flashColor = hitFlash.main;
		ParticleSystem.MainModule shockwaveColor = hitShockwave.main;
		ParticleSystem.MainModule sparksColor = hitSparks.main;
		flashColor.startColor = config.mainColor;
		shockwaveColor.startColor = config.mainColor;
		sparksColor.startColor = config.mainColor;
		// start particles
		mainParticle.Play();
		secondaryParticle.Play();

	}

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update() {
		// updates bullet duration
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0) {
			if (!dead) {
				Die();
			} else if (lifeTime <= -config.deathTime) {
				// check if the manager exists so the bullet can be returned
				if (manager != null) {
					manager.Return(this);
				} else {
					Debug.Log("Manager for bullet not set! Destroying bullet instead.");
					Destroy(gameObject);
				}
			}
			return;
		}
		// moves the bullet according to its behaviour
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

	/// <summary>
	/// Stops all the particles and wait for them to finish before disabling the bullet.
	/// This is necessary because we can't destroy the bullet on a timer, as we are only disabling it.
	/// </summary>
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

	/// <summary>
	/// Sets the bullet manager, used so the bullet returns to the correct manager
	/// </summary>
	/// <param name="manager">The manager which this bullet belongs to</param>
	public void SetManager(BulletManager manager) {
		this.manager = manager;
	}
}