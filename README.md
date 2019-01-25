# Dynamic Projectiles

With this project, I focused on achieving two main objectives:

* Exploring the mechanics of projectiles in environments that work with two directions, like top-down or sidescroller shooters, including their movement behaviour and visual aspects, while being able to change, mix and match those in real time.
  
* Learning and improving my skills in working with graphics, including VFX, shaders, post processing and rendering pipelines.

All of the assets included in this project were made by myself, including 2D textures for meshes and particles, and shaders made using Unity's new Shader Graph feature.

To demo this concept I made a scene with two static actors, one that fires projectiles and other that can be hit by them (but will not destroy the bullets). I also made a configuration UI in which you can change and play with each setting during runtime and included some presets.
  
![](https://github.com/akashenen/dynamic-projectiles/blob/master/Gifs/Preset_A.gif) ![](https://github.com/akashenen/dynamic-projectiles/blob/master/Gifs/Preset_B.gif)
![](https://github.com/akashenen/dynamic-projectiles/blob/master/Gifs/Preset_C.gif) ![](https://github.com/akashenen/dynamic-projectiles/blob/master/Gifs/Preset_D.gif)

## Getting Started

To use this project you need Unity 2018.3 or higher. 

The configuration for how the projectiles will function uses the [WeaponConfig](https://github.com/akashenen/dynamic-projectiles/blob/master/Assets/Scripts/WeaponConfig.cs) class, which is a [ScriptableObject](https://docs.unity3d.com/ScriptReference/ScriptableObject.html), meaning you can either create predefined weapon config assets through the editor or create your own objects in runtime. Each WeaponConfig object will contain information for both the behaviour and visuals of a projectile.

#### Visual Properties
* **mainColor:** Color used for the main particle
* **colorGradient:** Gradient used for the secondary particle and the trail
* **trailLength:** Length of the projectile's trail
* **trailWidth:** Width of the projectile's trail

#### Functional Properties
* **projectileCount:** How many projectiles should be fired at each burst
* **burstSpread:** How much time between each projectile should be proportionally spread across the interval of each burst. 0 = every projectile at the same time; 1 = projectiles are spread across all the interval, making the bursts seem seamless.
* **distance:** Starting position distance of each projectile in relation to the center one. Ignored in case of nova type burst.
* **angle:** Angle (in degrees) between each projectile and the next.
* **nova:** If checked, will ignore distance and angle properties and distribute the starting points of each projectile around the parent object.
* **randomOrder:** If checked, will fire each projectile on a random order. Most useful when combined with burst spread.
* **speed:** Traveling speed of each projectile fired.
* **duration:** Maximum lifetime in seconds for each projectile.
* **deathTime:** How long each projectile takes for all particles to finish their animations before it gets disabled, can take longer depending on the desired effects. Default = 0.5 seconds.
* **damageMulti:** Multiplier for the parent's damage for each projectile.
* **rateMulti:** Multiplier for the parent's firing rate.
* **behaviour:** Behaviour of each projectile shot. Each behaviour is programmed individually, so you have to change the projectile script to add more behaviours. More details below.

#### Behaviours

The projectile behaviours define what movement they will perform and are located in the Update method of the [Projectile](https://github.com/akashenen/dynamic-projectiles/blob/master/Assets/Scripts/Projectile.cs) class. For this demo I created 2 behaviours: straight and orbit. Other types of behaviour may include curves, waves, homing, etc.

* **Straight:** the projectile will never change angles and travel in a straight line until it expires.
* **Orbit:** the projectile will orbit around its parent object. The *distance* configuration works a bit differently in this behaviour: the **X** value defines how much the projectile's orbit will distance itself from the center and the **Y** value defines the starting distance from the center. It's important the distance never becomes 0, otherwise the projectile won't know the correct direction to distance itself to.

## Authors

* [Akashenen](https://github.com/akashenen/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

