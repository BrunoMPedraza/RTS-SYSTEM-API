# RTS SYSTEM API

Based upon a premise of orders and queue actions.

The current target of our projects is to model a coop based game, top down in
## Script glossary

### CameraController

Completely related to the movement of the camera, it returns nothing else than its own behavior.

Any relevant changes can be made in the inspector since the variables are public.

**Its independent of anything else**

#### Future features
* Player input manager (*Take input information*)
* Center camera on input (F1 by default)
* Lock camera on input (Y by default) 
* Check last relevant event on input (Space by default)
* Roll warcraft 3 style on zoom
* Smooth zoom
* Rotate camera


### PlayerMotor

Manages orders based upon the behavior of the character. Its independent of the INPUT.  
Therefore an AI *might* use it 

### Order
Modularizer that contains most of information to orders
It also distributes with a switch the order types.

Also holds as a constructor.
#### IsCompleted()
Checks if an order is complete by using a bool parameter


### PlayerMovement
input master

***Needs to be looked at, its awful***
  
### Item
WIP - Just checks if it still exists.


### Scriptable Objects

#### ItemType
It looks upon the posibilities of an item, being a valuable the only one who can be picked up and hold in the inventory.

#### InteractionType
Checks upon target type to see what variables apply to the order. For example if the player has the Type "ENEMY" smart order will likely mean attack.
  
