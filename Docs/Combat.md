# Combat

Mapping:
  - A button: fire guns
  - B button: fire missiles
  - Y button: select target
  - Y button (hold): look at target
  - Right stick: look around

## Targeting

Only one target can be selected at any given time.
Missiles always lock on the currently selected target, given it is in range.

When looking around or looking at the target, the camera orbits the player's plane.

## Guns

Guns are always mounted in the forward position and cannot turn by themselves.
They have unlimited ammunition, but can only be used short range.
Excessive use generates heat, causing accuracy to drop.
Upon exceeding the heat gauge, the player needs to wait for the guns to cool down until they can be used gain.

## Missiles

The player posses a limited, yet unrealistically large number of homing missiles.
Typically two missiles are required to take down one enemy.
Between firing missiles, players need to wait until automatic missile reload has been completed.

Missiles require a short amount of time to *lock-on*.
Missiles do not have splash damage, they require a direct hit in order to deal damage.

## Collision

Planes do not collide with each other.
Planes collide with bigger entities (blimps, trains…).
Planes crash immediately if they collide with terrain or bigger entities.
