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
### Tutorial
- [Learn Unity Tutorial](https://youtu.be/pwZpJzpE2lQ)
- [Learn Low Poly Blender Tutorial](https://youtu.be/1jHUY3qoBu8)
- [Retro Sci-Fi Gun Blender Model](https://youtu.be/nBmtTOQCfTo)
- [Unity FPS Gun with Raycasts](https://youtu.be/THnivyG0Mvo)
- [Unity Shooting Bullets](https://youtu.be/6eIVxyxoimc)
- [Unity Muzzle Flash](https://youtu.be/rf7gHVixmmc)
- [Unity Interval Between Shooting](https://forum.unity.com/threads/script-for-bullet-with-a-second-delay.720470/)

### Portal Examples
- [Coding Adventure: Portals](https://youtu.be/cWpFZbjtSQg)
- [Smooth Portals in Unity](https://youtu.be/cuQao3hEKfs)

### C#
- [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-5.0)
- [Foreach](https://stackoverflow.com/questions/18863187/how-can-i-loop-through-a-listt-and-grab-each-item)
- [Shallow Copy a List(https://stackoverflow.com/questions/222598/how-do-i-clone-a-generic-list-in-c)

### General Unity
- [Local to World Matrix](https://docs.unity3d.com/ScriptReference/Transform-localToWorldMatrix.html)
- [Access Child GameObject](https://answers.unity.com/questions/464616/access-child-of-a-gameobject.html)
- [Get MeshRenderer of GameObject](https://answers.unity.com/questions/959195/get-meshrenderer-component-of-gameobjects-in-an-ar.html)
- [Change Material Programmatically](https://stackoverflow.com/questions/39930186/create-material-from-code-and-assign-it-to-an-object)
- [Create Primitive Game Objects Through Code](https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html)
- [Size of GameObject](https://answers.unity.com/questions/24012/find-size-of-gameobject.html)
- [Change Radius of Primitive Sphere](https://answers.unity.com/questions/577187/increase-the-radius-of-unitys-primitive-sphere.html)
- [Debug.DrawRay](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html)
- [Change Axis of GameObject](https://answers.unity.com/questions/62675/redefine-axis-of-an-object.html)
- [Add Script to GameObject](https://answers.unity.com/questions/1136397/how-to-add-a-script-to-a-gameobject-during-runtime.html)

### Math
- [Linear Interpolation Between Two Points](https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html)
- [Get Direction Between Two Points](https://answers.unity.com/questions/697830/how-to-calculate-direction-between-2-objects.html)

### Camera and Renderer
- [Camera.Render](https://docs.unity3d.com/ScriptReference/Camera.Render.html)
- [RenderTexture](https://docs.unity3d.com/ScriptReference/RenderTexture-ctor.html)
- [RenderTexture.Release](https://docs.unity3d.com/ScriptReference/RenderTexture.Release.html)
- [Checks if Renderer is Visible](https://wiki.unity3d.com/index.php/IsVisibleFrom)

#### Raycasts
- [Physics.Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html)
- [Raycast Normal](https://docs.unity3d.com/ScriptReference/RaycastHit-normal.html)

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