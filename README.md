# Super Wizard Platformer

Adventures in C#/.NET game programming with MonoGame.

# Installation

All dependencies (except for the core MonoGame dependency) are NuGet packages. 
The game is based on MonoGame version 3.5, the SDK for which is available 
[here](http://www.monogame.net/2016/03/17/monogame-3-5/).

# Contribution Guidelines

* This project attempts to conform Microsoft's conventions for C# naming and style,
although most classes do not use the preferred leading underscore for private member
variable names.
* Most lines should be kept under 100 characters wide, if possible. Obvious exceptions
for very long strings or documentation comments.
* Take care to not generate garbage during the inner game loop (calls to IScene.Update
or IScene.Draw). Invocation of the GC during game play should be avoided whenever possible.
