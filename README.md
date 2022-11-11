# StateBehaviours
ChilloutVR mod that adds a couple State Machine Behaviours for avatar creators to tinker with until native solutions are released.

**This is local only.**
Custom state behaviors will not run for remote users with this mod, but the end result should sync over the network.

## How to Use

Download unitypackage under the releases tab on the right. 

Import into your unity project.

Add to an animation state like any other state behaviour!

## Included

<img align="right" src="https://user-images.githubusercontent.com/37721153/201439798-a1303151-7899-48b3-941a-f4e56e1f1fc4.png">

### Parameter Drivers:

* Simple Driver:
  * Set, Add/Subtract, Random, Copy (Float, Int, Bool, Trigger)
  * Select Update Mode - run on StateEnter or StateExit!

* Update Driver:
  * Get current state PlayTime as float
  * Get current state NormalizedTime as float
  * Copy float and output lerped result to new float.


### Body Control:
* IK toggles for different parts of body. (Ignore, Enabled, Disabled)
  * OnEnter or OnExit
  * TrackingAll
  * TrackingHead
  * TrackingLeftArm
  * TrackingRightArm
  * TrackingLeftLeg
  * TrackingRightLeg
  * TrackingLocomotion

## Notes

**This mod is likely to become outdated & depricated once ChilloutVR adds their own native solutions or state machine behaviours.**

## Testing in Unity
* Place an object with the LocalAnimatorManager script into your scene.
* Drag the root of your avatar into the Animator slot.
* Make sure your custom AAS override controller is added to your avatars Animator component.
* Enter playmode.

This process will hopefully be automatic, improved, & support multiple avatars once I find the time.

![image](https://user-images.githubusercontent.com/37721153/201440867-ca3b6a44-f8bd-467c-8039-2437313d3410.png)


### Might tinker with later:
* Layer Weight Control (blend animator layer weights)
* Goto State (tell animator to jump to a state in specified layer)
* Write Input (use parameter input as player input, might be better to use OSC mod..?)

---

Here is the block of text where I tell you this mod is not affiliated or endorsed by ABI. 
https://documentation.abinteractive.net/official/legal/tos/#7-modding-our-games

> I am not affiliated with ABI in any official capacity, these mods are not endorsed or outright permitted by ABI and are subject to scrutiny.

> Neither I nor these mods are in any way affiliated with Alpha Blend Interactive and/or ChilloutVR. Using these modifications might cause issues with the performance, security or stability of the game. Use at your own risk.

> Any modifications that are not approved can get your ABI account terminated and such this modification is following the "modding guidelines" at the best it could be.
> They reserve the right to punish users using my mod.
> If you are scared of using modifications in your game do not install mods.

> I do not affiliate ABI and the mod is not supported by ABI.

> Me and this modification are in no affiliation with ABI and not supported by ABI.

> This mod is not affiliated with Alpha Blend Interactive. The mod comes with no warranty. Use at your own risk, as I am not responsible for any misuse.

> I'm not affiliated with Alpha Blend Interactive and this mod is not officially supported by the game.

> When releasing mods to the public, it is required to state, that the mod authors and modification are in no affiliation with ABI and not supported by ABI. :trollface:

> i ran out of places to steal disclaimers from
