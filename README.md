### Flappy Bird 3D prototype

* Created in Unity 2022.3.4f1
* Built and tested on Android
* Plays best in landscape orientation
* Can be controlled with arrows and space, mouse or controller in Unity and touch and screen button on Android
* Used textures from everytexture.com
* Some unit tests are created for couple of services
* Input is implemented using new Unity "Input System".
* Configuration parameters are in "Assets/Configs/Config".
* Zenject is used as DI framework.
* "MenuAssets" and "GameAssets" addressable groups can be configured for remote paths.
* "Entry" is starting scene.
* Project is split into 3 modules/scenes: "Entry", "Menu", "Game". "Entry" scene is persistent for entire game, "Menu", "Game" scenes are loaded as needed additively. Each scene is in it's own assembly.
* Every scene has its own entry point which starts main controller.
* Almost every aspect of game has it's own view which communicates with injected controllers.
* Flow of game is managed by generic asynchronous state machines.
* There are also few services for global tasks like asset management, file operations, random number generation.
