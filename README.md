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

### General
- [Local to World Matrix](https://docs.unity3d.com/ScriptReference/Transform-localToWorldMatrix.html)
- [Access Child GameObject](https://answers.unity.com/questions/464616/access-child-of-a-gameobject.html)
- [Get MeshRenderer of GameObject](https://answers.unity.com/questions/959195/get-meshrenderer-component-of-gameobjects-in-an-ar.html)
- [Change Material Programmatically](https://stackoverflow.com/questions/39930186/create-material-from-code-and-assign-it-to-an-object)

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

## Bugs
- `OnCollisionEnter()` not working
	- [Solution Source](https://forum.unity.com/threads/oncollisionenter-not-working.99149/)
	- One of the colliders needs a rigidBody
- `Destroy(this)` not working
	- [Solution Source](https://answers.unity.com/questions/478876/destroythis-not-working-properly.html)
	- Use `Destroy(this.gameObject)` or `Destroy(gameObject)`