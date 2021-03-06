﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store all information about a specific weapon's visuals and behaviour, 
/// and can be changed during gameplay to change or adapt any properties of that weapon
/// </summary>
[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon", order = 2)]
public class WeaponConfig : ScriptableObject {

	[Header("Visual")]
	[Tooltip("Color used for the main particle.")]
	public Color mainColor;
	[Tooltip("Gradient used for the secondary particle and the trail")]
	public Gradient colorGradient;
	[Tooltip("Length of the projectile's trail")]
	public float trailLength;
	[Tooltip("Width of the projectile's trail")]
	public float trailWidth;

	[Header("Functional")]
	[Tooltip("How many projectiles should be fired at each burst.")]
	public float projectileCount;
	[Tooltip("How much time between each projectile should be proportionally spread across the interval of each burst. 0 = every projectile at the same time; 1 = projectiles are spread across all the interval, making the bursts seem seamless.")]
	[Range(0f, 1f)]
	public float burstSpread;
	[Tooltip("Starting position distance of each projectile in relation to the center one. Ignored in case of nova type burst.")]
	public Vector2 distance;
	[Tooltip("Angle (in degrees) between each projectile and the next.")]
	public float angle;
	[Tooltip("If checked, will ignore distance and angle properties and distribute the starting points of each projectile around the parent object.")]
	public bool nova;
	[Tooltip("If checked, will fire each projectile on a random order. Most useful when combined with burst spread.")]
	public bool randomOrder;
	[Tooltip("Traveling speed of each projectile fired")]
	public float speed;
	[Tooltip("Maximum lifetime in seconds for each projectile.")]
	public float duration;
	[Tooltip("How long each projectile takes for all particles to finish their animations before it gets disabled, can take longer depending on the desired effects. Default = 0.5 seconds")]
	public float deathTime = 0.5f;
	[Tooltip("Multiplier for the parent's damage for each projectile.")]
	public float damageMulti;
	[Tooltip("Multiplier for the parent's firing rate.")]
	public float rateMulti;
	[Tooltip("Behaviour of each projectile shot. Each behaviour is programmed individually, so you have to change the projectile script to add more behaviours.")]
	public ProjectileBehaviour behaviour;
	[Tooltip("WIP. Will have no effect.")]
	public List<ProjectileEffect> effects;

	public enum ProjectileBehaviour {
		Straight,
		Orbit
	}

	public enum ProjectileEffectType {
		Explosion,
		Fork,
		Chain,
	}

	public struct ProjectileEffect {
		public ProjectileEffectType type;
		public float value;
	}
}