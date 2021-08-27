# Kloto

## Project Description
Kloto is a single player FPS portal simulator made on the Unity game engine.
Players can explore two distinct maps through a first-person perspective by
teleporting through existing portals or creating their own portals. Using a
portal gun, players can shoot on surfaces to create portals to teleport
through.

## Demo
Embed YouTube video here  
Add images here

## Usage
Add link here to playable game here if the game is polished  
This repo contains everything required to run the program. Clone this repo to run the program on Unity.

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

## How to Play
- Walk: WASD or Arrow Keys
- Jump: Space Bar
- Shoot: Right and Left Mouse Click

## Features
- First-person shooter
- Two open maps to explore
- Two-sided portals that link the two maps
- A portal gun that creates one-sided portals on surfaces
- Preserved player momentum when teleporting
- Interactive sound effects

## Limitations
- Platform
	- The game is only playable on the computer

## Existing Bugs
- Looking at a two-sided portal through a one-sided portal can render an unnatural camera view and angle
- Portal gun bullets occasionally go through objects
- A "Screen position out of view frustrum" Unity error inconsistently occurs

## Plans for the Future
- Add recursive portal renders
- Slice the models of objects passing through portals

## Contributors
- Calum Clark: Portal rendering, teleportation, player movement
- Daniel Ko: Map design, gun and bullet orb model, shooting and portal creation mechanics, sound effects