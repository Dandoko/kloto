# Kloto

## Project Description
Kloto is a single-player FPS portal simulator made on the Unity game engine.
Players can explore two distinct maps through a first-person perspective by
teleporting through existing portals or creating their own portals. Using a
portal gun, players can shoot on surfaces to create portals to teleport
through.

## Video Demo
[![Kloto Image](https://img.youtube.com/vi/i44ZGvUswMY/0.jpg)](https://www.youtube.com/watch?v=i44ZGvUswMY)

## Usage  
This repo contains everything required to run the program. Clone this repo to run the program on Unity.  
  
You can also play it [here](https://simmer.io/@Dandoko/kloto).  
**Note:** For the best quality of gameplay, we recommend you to play it on Unity because WebGL and simmer.io compress files 
which can render the Unity Universal Render Pipeline and Render Texture poorly.

## Assets
- [Footsteps Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/foley/footsteps-essentials-189879)
- [Sci-Fi Gun Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/weapons/sci-fi-gun-sounds-pack-lite-141125)
- [Sci-Fi Orb Bitmaps](https://oxmond.com/glowing-orb-visual-effects-vfx/)
- [Sci-Fi Portal Sound Effect](https://assetstore.unity.com/packages/audio/sound-fx/sci-fi-evolution-gift-pack-43104)
- [Unity Particle Pack](https://assetstore.unity.com/packages/essentials/asset-packs/unity-particle-pack-5-x-73777)
- [YuME Map Prototype](https://assetstore.unity.com/packages/tools/level-design/yume-free-77387)

## Technologies
- Unity 2020.3.15f2
- Blender 2.83.13.0
- Visual Studio 16.4.2
- C# 8.0

## Unity Resources
- Universal Render Pipeline
- Render Textures and Shaders
- Resources Class

## How to Play
- Walk: WASD or Arrow Keys
- Jump: Space Bar
- Shoot: Right and Left Mouse Click

## Features
- First-person shooter
- Two open maps to explore
- Two-sided portals that link the two maps
- A portal gun that creates one-sided portals on surfaces
- Portal rendering using render textures and shaders
- Rendering two-sided portal perspectives on one-sided portals and vice versa
- Preserved player momentum when teleporting
- Interactive sound effects

## Limitations
- Platform
	- The game is only playable on the computer

## Existing Bugs
- The portal screen infrequently flickers when passing through it

## Plans for the Future
- Add recursive portal renders
- Slice the models of objects passing through portals

## Contributors
- Calum Clark: Portal rendering, portal teleportation, player movement
- Daniel Ko: Portal rendering, map design, 3D modeling, shooting and creating portals, sound effects