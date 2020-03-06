# AI

AI does not need to take the full flight model into account.
AI combatants can just follow predefined flight paths.
Each predefined flight path should only be a couple of seconds long and the sequence should be selected depending on the situation — maybe with some weights attached to them.

Velocity and altitude can be varied along the path.
In addition to them, some basic logic for tracking an enemy should be implemented — firing missiles and guns when appropriate.

For implementation, consider *behaviour trees*.

## Predefined Flight Paths

- Flying straight
- Banking horizontally / vertically
- Looping / Kulbit
- Immelmann
- Aileron roll
- Barrel roll
- Cobra
