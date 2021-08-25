# kloto

## Remarks About Branching
- Remember to **use and push to your branch**, not the main branch
	- To be on your branch, `git checkout <user>Branch`
	- To push to your branch, `git push -u origin <user>Branch`
	- After pushing to your branch, submit a pull request to the main branch
		- Note: Pull requests can be done remotely through the GitHub website
- Remember to **pull from the main branch before starting on anything**
(programming, modelling, etc.) to minimize merge conflicts and to update your local
directory with changes someone else could have made
	- Change branches locally to the main branch by `git checkout main`
	- Pull from the remote GitHub repository by `git pull`
	- After pulling, change branches to your branch through `git checkout <user>Branch` 
	- In your branch, merge with the main branch by `git merge main`
	- After merging, `git push` immediately to have the branch on the remote repo be
	even with main

## Links
### Assets
- [Footsteps Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/foley/footsteps-essentials-189879)
- [Sci-Fi Gun Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/weapons/sci-fi-gun-sounds-pack-lite-141125)
- [Sci-Fi Orb Bitmaps](https://oxmond.com/glowing-orb-visual-effects-vfx/)
- [Sci-Fi Portal Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/sci-fi-evolution-gift-pack-43104)
- [Particle Pack](https://assetstore.unity.com/packages/essentials/asset-packs/unity-particle-pack-5-x-73777)
- [YuME Map Prototype](https://assetstore.unity.com/packages/tools/level-design/yume-free-77387)

### Tutorial
- [Learn Unity Tutorial](https://youtu.be/pwZpJzpE2lQ)
- [Learn Low Poly Blender Tutorial](https://youtu.be/1jHUY3qoBu8)
- [Retro Sci-Fi Gun Blender Model](https://youtu.be/nBmtTOQCfTo)
- [Unity FPS Gun with Raycasts](https://youtu.be/THnivyG0Mvo)
- [Unity Shooting Bullets](https://youtu.be/6eIVxyxoimc)
- [Unity Muzzle Flash](https://youtu.be/rf7gHVixmmc)
- [Unity Interval Between Shooting](https://forum.unity.com/threads/script-for-bullet-with-a-second-delay.720470/)
- [Continuous Collision Detection](http://wiki.unity3d.com/index.php?title=DontGoThroughThings&_ga=2.49978917.2097179850.1624410069-592288669.1621822495&_gl=1*c1ab9y*_ga*NTkyMjg4NjY5LjE2MjE4MjI0OTU.*_ga_1S78EFL1W5*MTYyNDQ5MDA4Mi4yMi4xLjE2MjQ0OTAyNTguNjA)
- [Unity Asset Manager](https://youtu.be/7GcEW6uwO8E)
- [Unity Sound Manager](https://youtu.be/QL29aTa7J5Q)
- [Unity Glowing Orb](https://youtu.be/pxNzoLfreOo)
- [Unity AudioSource Doppler Effect](https://youtu.be/eQphjWreQ0U)
- [Hull Outline Shader in Unity URP](https://youtu.be/1QPA3s0S3Oo)

### Portal Examples
- [Coding Adventure: Portals](https://youtu.be/cWpFZbjtSQg)
- [Smooth Portals in Unity](https://youtu.be/cuQao3hEKfs)
- [FPS Portals](https://youtu.be/PkGjYig8avo)

### C#
- [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-5.0)
- [Foreach](https://stackoverflow.com/questions/18863187/how-can-i-loop-through-a-listt-and-grab-each-item)
- [Shallow Copy a List](https://stackoverflow.com/questions/222598/how-do-i-clone-a-generic-list-in-c)
- [Pass By Reference](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref)
- [Find Visual Studio Version](https://stackoverflow.com/questions/5089389/how-can-i-check-what-version-edition-of-visual-studio-is-installed-programmatica)
- [C# Versions](https://stackoverflow.com/questions/247621/what-are-the-correct-version-numbers-for-c)

### Unity

#### Editor
- [Update Visual Studio with Package Manager](https://forum.unity.com/threads/update-to-latest-visual-studio-2019-core-editor-package.988289/)

#### General
- [Local to World Matrix](https://docs.unity3d.com/ScriptReference/Transform-localToWorldMatrix.html)
- [Access Child GameObject](https://answers.unity.com/questions/464616/access-child-of-a-gameobject.html)
- [Get MeshRenderer of GameObject](https://answers.unity.com/questions/959195/get-meshrenderer-component-of-gameobjects-in-an-ar.html)
- [Change Material Programmatically](https://stackoverflow.com/questions/39930186/create-material-from-code-and-assign-it-to-an-object)
- [Create Primitive Game Objects Through Code](https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html)
- [Size of GameObject](https://answers.unity.com/questions/24012/find-size-of-gameobject.html)
- [Change Radius of Primitive Sphere](https://answers.unity.com/questions/577187/increase-the-radius-of-unitys-primitive-sphere.html)
- [Change Axis of GameObject](https://answers.unity.com/questions/62675/redefine-axis-of-an-object.html)
- [Add Script to GameObject](https://answers.unity.com/questions/1136397/how-to-add-a-script-to-a-gameobject-during-runtime.html)
- [Get LayerMask by Name](https://docs.unity3d.com/ScriptReference/LayerMask.NameToLayer.html)
- [Change Colour of GameObject](https://docs.unity3d.com/ScriptReference/Material.SetColor.html)
- [Combining Layer Masks](https://answers.unity.com/questions/8715/how-do-i-use-layermasks.html)
- [Excluding Layer Masks](https://answers.unity.com/questions/1343414/ignore-one-layermask-question.html)
- [Difference Between TransformVector and TransformPoint](https://answers.unity.com/questions/1021968/difference-between-transformtransformvector-and-tr.html)
- [Resources.Load](https://docs.unity3d.com/ScriptReference/Resources.Load.html)
- [Object.Destroy](https://docs.unity3d.com/ScriptReference/Object.Destroy.html)
- [Destroying Objects Without Monobehaviour](https://forum.unity.com/threads/destroy-without-monobehaviour.160249/)
- [Destroy Parent of Child GameObject](https://answers.unity.com/questions/275343/destroy-parent-of-child-gameobject.html)
- [Scale Particle System](https://forum.unity.com/threads/how-does-the-transforms-scale-work-with-a-particle-system.101964/?_gl=1*147n9lq*_ga*NTkyMjg4NjY5LjE2MjE4MjI0OTU.*_ga_1S78EFL1W5*MTYyNzE3NDI3NC40My4xLjE2MjcxNzQ5NzYuNjA.&_ga=2.239049219.2050753888.1627087806-592288669.1621822495)
- [Get a Script of a GameObject](https://forum.unity.com/threads/how-to-get-a-script-component-of-a-gameobject-solved.401979/)
- [Remove a Script from a GameObject](https://answers.unity.com/questions/1505999/how-to-remove-material-from-object.html)
- [Missing GameObject](https://answers.unity.com/questions/34926/missing-transform-vs-none-how-to-code-this-or-quer.html)
- [Debug.LogError](https://docs.unity3d.com/ScriptReference/Debug.LogError.html)

#### Input
- [Input.GetButton](https://docs.unity3d.com/ScriptReference/Input.GetButton.html)

#### Math
- [Linear Interpolation Between Two Points](https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html)
- [Get Direction Between Two Points](https://answers.unity.com/questions/697830/how-to-calculate-direction-between-2-objects.html)
- [Mathf.Abs](https://docs.unity3d.com/ScriptReference/Mathf.Abs.html)

#### Camera and Renderer
- [Camera.Render](https://docs.unity3d.com/ScriptReference/Camera.Render.html)
- [RenderTexture](https://docs.unity3d.com/ScriptReference/RenderTexture-ctor.html)
- [RenderTexture.Release](https://docs.unity3d.com/ScriptReference/RenderTexture.Release.html)
- [Checks if Renderer is Visible](https://wiki.unity3d.com/index.php/IsVisibleFrom)
- [Camera.depth](https://docs.unity3d.com/ScriptReference/Camera-depth.html)
- [Shadow Distance](https://docs.unity3d.com/Manual/shadow-distance.html)

#### Collision Detection
- [Physics.Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html)
- [Raycast Normal](https://docs.unity3d.com/ScriptReference/RaycastHit-normal.html)
- [Debug.DrawRay](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html)
- [Physics.CheckSphere](https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html)
- [Physics.OverlapSphere](https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html)
- [Physics.OverlapBox](https://docs.unity3d.com/ScriptReference/Physics.OverlapBox.html)
- [Physics.IgnoreCollision](https://docs.unity3d.com/ScriptReference/Physics.IgnoreCollision.html)

#### Audio
- [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html)
- [AudioClip.length](https://docs.unity3d.com/ScriptReference/AudioClip-length.html)

### Blender

#### UV Editor
- [Show unselected vertices in UV Image Editor](https://blender.stackexchange.com/questions/2781/show-unselected-vertices-in-uv-image-editor)

## Bugs
- `OnCollisionEnter()` not working
	- [Solution Source](https://forum.unity.com/threads/oncollisionenter-not-working.99149/)
	- One of the colliders needs a rigidBody
- `Destroy(this)` not working
	- [Solution Source](https://answers.unity.com/questions/478876/destroythis-not-working-properly.html)
	- Use `Destroy(this.gameObject)` or `Destroy(gameObject)`
- Can't see Debug.DrawRay()
	- [Solution Source](https://answers.unity.com/questions/1441912/debugdrawray-isnt-working-at-all.html)
	- Need to toggle Gizmos in the Game view
- You are trying to create a MonoBehaviour using the 'new' keyword. This is not allowed.
	- [Solution Source](https://answers.unity.com/questions/653904/you-are-trying-to-create-a-monobehaviour-using-the-2.html)
	- You cannot instantiate anything inherting from the `MonoBehaviour` class
- Collection was modified, enumeration operation may not execute
	- Use a copy of the list-to-iterate in the foreach loop
	- If a copy isn’t used, the runtime doesn’t know which list to write to and which one to read from
- Raycasts passing through colliders
	- [Solution Source](https://forum.unity.com/threads/raycast-not-finding-objects-collider.323109/)
	- Attempted repositioning the raycast origin and extending the raycast length, but it didn't solve the issue
- Physics.OverlapSphere Layer Mask not working
	- [Solution Source](https://answers.unity.com/questions/681890/how-to-use-physicsoverlapsphere-with-layer-mask.html)
	- Use `1 << 8` or `1 << layer`
- Physics.Raycast layer mask is not working
	- The [Unity documentation](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) is misleading
		- “A Layer mask that is used to selectively ignore Colliders when casting a ray.”
	- The layermask parameter for Physics.Raycast should be the layers you want the raycast to hit
- Null error with Resources.Load
	- Need to create a `Resources` folder in the game Assets
	- Save your resource in this `Resources` folder
- Wrong camera is rendering on game mode
	- [Solution Source](https://answers.unity.com/questions/203376/wrong-choice-of-main-camera-among-multiple-cameras.html)
	- Set the depth of the camera 
	- Higher depths will be rendered last
- AudioSource.PlayOneShot() is not looping
	- [Solution Source](https://answers.unity.com/questions/1123649/audiosourceplayoneshot-is-not-looping.html)
	- Use `AudioSource.Play()` instead
- The type 'UnityEngine.Vector3' cannot be declared const
	- [Solution Source](https://answers.unity.com/questions/60262/the-type-unityenginevector3-cannot-be-declared-con.html)
	- Only the C# built-in types (excluding System.Object) may be declared as const
- Universal Render Pipeline makes turns materials pink
	- [Solution Source](https://answers.unity.com/questions/1519382/textures-and-materials-turn-pink-after-installing.html)
	- Upgrade materials to URP materials
- After upgrading materials to URP, default materials remain pink
	- Apply a non-default material to the game object
- URP makes shadows look pixelated
	- [SolutionSource](https://forum.unity.com/threads/pixelated-shadows.921257/)
	- Reduce shadow distance
- A game object can only be in one layer. The layer needs to be in the range [0...31]
	- [Solution Source](https://answers.unity.com/questions/1103260/a-game-object-can-only-be-in-one-layer-the-layer-n.html)
	- `mask = LayerMask.NameToLayer("RemotePlayer");`

## Debugging

### Visualizing spheres for sphere colliders
```
GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
tempSphere.transform.position = <position>;
tempSphere.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
tempSphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
tempSphere.GetComponent<Collider>().enabled = false;
```