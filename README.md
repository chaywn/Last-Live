Last Live
===
Last Live is a singleplayer first-person shooter (FPS) zombie game that challenges players to survive from waves of zombies with only one live.

Game: [Last Live](/Last%20Live_Game)
Video Demo: https://youtu.be/nM_P2qx79_Y

## Technologies
The game engine used in the production is Unity 2021.3.4f1. 

Due to the compatibility of the assets and packages that are being used in this game, running the project in older versions of Unity **may not work properly as intended**.


## Aim & Objectives
The aim of this game is to serve as my final project for CS50’s Introduction to Game Development.

The objectives of the game are:
1. To apply the knowledge that I've learned throughout the lessons in CS50’s Introduction to Game Development
2. To further expand my knowledge and skills in Unity
3. To explore horror/survival game genre


## How To Play
> *You are at an abandoned supermarket. The sun has just set. As darkness sets in, you realize you are not alone...*

The goal of the game is simple: Survive as many waves as possible with only one live. 

Throughout each wave, zombies will spawn around the map; In order to proceed to the next wave, you have to kill all zombies or survive till the timer ends. Equip your gun and get ready to fight!

Controls:
- WASD - Movement
- L-Shift - Sprint
- L-Mouse - Shoot/Fire
- R-Mouse - Aim
- R - Reload
- P - Pause


## Game Scenes/States
Last Live consists of four main scenes, which are:
- a title scene
- a weapon selection scene
- the actual game scene
- a gameover scene

### Title Scene
![Title Scene](/Screenshots/Title%20Scene_Screenshot.png)
This is the first scene of the game, where players are brought to upon running the game. 
From the title scene, players can nativate to the [Weapon scene](#weapon-selection-scene) and [Game scene](#game-scene) by pressing the "Weapon" and "Play" button. Pressing the "Quit Game" button will quit the game application.


### Weapon (Selection) Scene
![Weapon Scene](/Screenshots/Weapon%20Scene_Screenshot.png)
Entering the weapon selection scene, players can view and select all the available guns by navigating using the arrow buttons on the screen or using the arrow keys on the keyboard. By default, the first gun in the list (which is the AK-47) will be displayed as the first selection upon selecting for the first time. After that, the weapon selection will always display the player's last selection. Pressing the "Confirm" button will redirect the player back to the [Title scene](#title-scene).


### Game Scene
![Game Scene](/Screenshots/Game%20Scene_Screenshot.png)
This is where the game (aka where all the action) takes place. Upon spawning into the game, the player will be equipped with the gun they selected (or the default gun if no selection was made). The timer will start as the first wave begins, and zombies will start spawning around the map. When the target zombie kill count is reached or the timer is up, the next wave will begin.


### GameOver Scene
![GameOver Scene](/Screenshots/GameOver%20Scene_Screenshot.png)
When the player dies, they will be redirected to the gameover scene, where the number of waves they have survived will be displayed on the screen. From here, player can either restart the game by pressing the "Play Again" button or return to the title scene by pressing the "Back to Menu" button.


## Behind the Scenes
During the development of Last Live, I have received a great deal of help from tutorials on YouTube (I'll link them below), suggested solutions from [Unity Forum](https://forum.unity.com/), and also documentation from [Unity Manual](https://docs.unity3d.com/Manual/index.html) to better understand the usage of different methods.

Below are some of the components of the game which I'd like to highlight, the process of their building, the problems I've encountered in the process and the approaches I've taken to solve them. For a better understanding of my code, I've also included comments in my some of my actual scripts to explain the code.

### The Game Manager
To keep track of the entire game progress, I needed a 'central control panel' to manage the connections between each gameobject and script. And that is the game manager. The game manager keeps track of the zombie kill count and the timer, and starts a new wave when the target zombie kill count is reached or when the timer is up. It also takes into account the player's preferences and apply it into the game scene, e.g. the selected gun to be equipped by the player; And it stores certain data using static variables, e.g. the waves survived that will be displayed during the game over scene.

### Gun
In building a functioning gun, I used [Coroutines](https://docs.unity3d.com/Manual/Coroutines.html) to create the 'cooldown' effect for gun fire and reloading. When it came to designing the shooting mechanism, I tried different approaches to achieve a high accuracy aim-to-shoot effect. For starters, I'm using raycasting to detect for any zombies with an 'Enemy' layer. In the initial stage, I created a gun muzzle to cast ray from as that is the most common approach I've seen from several tutorials. However, affer testing its effect using different guns and different aiming positions, I found the accuracy of shooting the gun this way to be inconsistent, and unreliable especially when in aiming mode without the help of a crosshair.

So, I added a crosshair. But since the crosshair has to be placed in the center of the screen, I had to aim the ray to the center of the screen as well. Hence, instead of casting the ray from the muzzle of the gun, I decided to cast the ray from the camera to the center of the screen using Unity's ```Camera.ViewportPointToRay``` method. 

When aiming the gun, I wanted to zoom in the camera for player to see in a distance. To do this, I decrease the camera's field of view whenever the player is holding down the right mouse button and increase back to normal when the player lets go. However, the zooming of the camera will produce a jittering effect whenever the player is moving. It was an extremely frustrating bug due to player movement and camera movement happening at the same time. After attempting numerous ways to fix this, I have yet to able to entirely resolve the bug, but I managed to reduce it to only an occasional delay when zooming the camera. Something to be noted is that the code for the camera zoom is not written in the gun script but in the ```FirstPersonController``` script from the [Standard Assets](#unity-store-assets) as part of the solution to this fix.

Credits to the [Gun Programming Tutorial by Davy](https://youtu.be/om-SS-CBZ8g) and [Plai's Unity Basic Weapon System Tutorial](https://youtu.be/kXbQMhwj5Uc) for the some of the gun code and inspiration. Plai's tutorial video has introduced me to the concept of using [ScriptableObjects](https://docs.unity3d.com/Manual/class-ScriptableObject.html) as data containers for objects of the same type, which is something I've never heard of before stumbling upon his video. After understanding its usage and how it can greatly benefit my game structure, I started implementing it on other components of my game for a cleaner and more organized game structure.

Lastly, for a swaying effect, I am using the code from [Plai's Unity Weapon Sway Tutorial](https://youtu.be/kXbQMhwj5Uc).


### Gun Selection
When it comes to coding a weapon selection system, I went for a simple and straightforward approach. The logic behind is simple (but can be tricky to get your head around at first): Firstly, we get the current gun selection index (0 by default). Based on the current selection index, we get the previous and next selection index. Now everytime a previous or next button is pressed, update the selections and move the new current gun into the camera view and the old current gun (aka the new previous/next gun) out of view. I then assigned each gun their own ```localPositionToCamera``` in their gun data to ensure they fit nicely in front of the camera. 

The reason I'm doing it this way (instead of updating the selections *after* moving the guns) is because I am only retrieving the reference for the **current gun**'s data, which is where I keep the position of gun to the camera. Hence, to get the position to camera for the previous or next gun, I have to set it as the current selection first.

Upon confirming the selection, the player's selection will be saved using ```PlayerPrefs```, allowing them to retain the gun they've chosen in every game launch.

Once the code was completed, all I had to do is to add a UI text to display their names and info. It's nothing fancy, but it gets the work done. I have no doubt that there is definitely better (and even cooler) ways of doing this, but nevertheless, I am proud to able to code this section entirely by myself using my own idea and the knowledge I've gained throughout this developing journey. 

### Zombies
To create a working AI for the zombies, I made use of Unity's navigation system and [NavMesh experimental package](#packages). By adding a [NavMeshAgent](https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html) component on the zombie gameobject, I can easily make them chase the player by setting their destination to the player's position using ```NavMeshAgent.SetDestination``` method. 

For the zombie animation, I was able to find a free [zombie animation pack](#assets) on Unity's Asset Store. However, because the zombie asset and animation asset are of different [animation type](https://docs.unity3d.com/Manual/FBXImporter-Rig.html), I had to retarget the animation from the animation pack to the zombie. After that, I only have to control the animation state in code using booleans and triggers.

As for the zombie sounds, I found a list of [zombie sound effects](#others-audio) online to play loop it.

### *Zombie Lair?*
No, there's no zombie lair. But to make up for that, we have a **zombie spawner** :sunglasses: The zombie spawner spawns zombies at random locations at a given rate; It will stop when the targeted number of zombies to spawn is reached, or reset when a new wave has begun. To do this, I'm using the ```InvokeRepeating()``` method to repeatedly spawn zombies into the game and the ```CancelInvoke()``` method to end the process. Once again I'm using ScriptableObject to contain the necessary data for the spawner, such as the list of zombie prefabs and the locations to spawn them after considering the possibility of adding other spawner types into the game.

### Health Bar
To create a health bar that changes colour with the player's health value, I am using most of the code (and the sprite) from [Brackey's Health Bar Tutorial](https://youtu.be/BLfNP4Sc_iA).


## Resources
Creating Last Live from scratch was no small task. Not only do I have to self-learn through tutorial videos, I had to search for assets for my game, including zombie models, gun models, zombie animations, and the sounds for zombies and guns (not to mention other sound effects as well). All of these I have gathered from [Unity's Asset Store](https://assetstore.unity.com/) and some from other third-party sources (may be subject to copyright). Below are the resources that I've used to implement into my game:

### Unity Store Assets:
- [Standard Assets (for Unity 2018.4)](https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2018-4-32351#reviews)
    - The asset for the first person controller (aka the player)
    - After being introduced to using the first person controller from the Standard Assets during my course, I wanted to continue using it in my project as I found it more convenient and simpler to implement compared to Unity's [Starter Assets](https://assetstore.unity.com/packages/essentials/starter-assets-first-person-character-controller-196525). However, the Standard Assets is incompatible with the newer Unity version I'm using. To fix this, I replaced the outdated scripts in the asset with a fix from [UnityStandardAssetFixes](https://github.com/johnathanhales/UnityStandardAssetFixes) by johnathanhales on GitHub.
    - Some parts of the script have also been modified to better fit the game.
- [POLYGON - Zombie Pack](https://assetstore.unity.com/packages/3d/characters/creatures/polygon-zombie-pack-81953) by 255 pixel studios
    - The asset for the supermarket map and zombie prefabs
- [Zombie Animation Pack Free](https://assetstore.unity.com/packages/3d/animations/zombie-animation-pack-free-150219) by Animus Digital
    - The asset for the zombie animations, including idle animation, walking animation, attack animation and death animation
- [Guns Pack: Low Poly Guns Collection](https://assetstore.unity.com/packages/3d/props/guns/guns-pack-low-poly-guns-collection-192553) by Fun Assets
    - The asset for the guns
- [Post Apocalypse Guns Demo](https://assetstore.unity.com/packages/audio/sound-fx/weapons/post-apocalypse-guns-demo-33515) by Sound Earth Game Audio
    - The asset for the gun audio

### Experimental Package:
- [NavMesh Building Components](https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.0/manual/index.html) (Experimental Package)
    - The package for building NavMeshes and the high level components for the navigation system

### Others:
- [Audiomachine - Bang And Burn (Dark Cinematic Modern Action)](https://youtu.be/EYfN9TAVdEI) from YouTube
    - The theme music of Last Live
- [ZOMBIE sound effects pack, growls, howls and grunts 🎵](https://youtu.be/8CvToeXz1xQ) from YouTube
    - The audio for zombie sounds
- Other sounds effects are from [Freesound](https://freesound.org/), [Pixabay](https://pixabay.com/), and [Zapsplat](https://www.zapsplat.com/)
- All fonts used are obtained on [free-font](https://www.free-fonts.com)

## Contributor
- Chay Wen Ning

