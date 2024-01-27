# PirateSoftware GameJam 14
## Scatter
### Theme: It's Spreading

# Prototype released on Itch!
https://squeegeexd.itch.io/scatter

## Minimum Viable Product:
### Outside (RTS):
- [x] Camera Controller

- [ ] Units
    - [ ] Squad
        - [x] Squad Controller
        - [ ] Squad AI
            - [x] Attack Mechanic
                - when right click on an enemy unit, selected units head towards selected enemy unit and attacks
            - [ ] Gather Resources
                - When right click on a resource, selected unit/s(?) head towards resource
                - different collecting times depending on resource
        - [ ] can find some survivors hiding in buildings

    - [ ] Zombies
        - [x] Zombie AI
        - [x] Attack Mechanic
            - View Distance
            - if Squad Unit enters view distance, zombie locks on to first Squad unit it sees
        - [ ] Spawner
            - when do they spawn?
            - how do they spawn?

- [ ] Items
    - [ ] Weapons
        - [x] ScriptableObjects
        - [ ] Guns
            - [x] Types
                - Pistol
                    - Base, Mid fire rate, mid attack range, mid damage
                - Assault Rifle
                    - Fast fire rate, big attack range, good damage
                - Shotgun
                    - Slow fire rate, small attack range, big damage
                - Knife
                    - Mid fire rate, very small attack range, big damage
            - [ ] Bullet/Indicator that they've fired
    - [ ] Survival Items
        - [ ] Scraps
            - needed for weapons
            - only one type
            - [Weapon] needs X Scraps to be crafted, something like that
        - [ ] Food
        - [ ] Med Kit
        - [ ] Map
            - Rerveals all buildings present in the area
            - Finding the map makes fog not reappear during the day
        - [ ] Radio
        - [ ] Flashlight

- [ ] Infect Mechanic
    - [ ] Squad
        - [x] Infected % bar instead of health bar
        - [x] Has a % chance of getting infected when attacked by zombie
        - [ ] can
    - [ ] Zombies
        - [x] Health Bar
        - [x] Attacking Squad units increases their Infected % bar

- [x] Fog Of War
    - [ ] Map Item Integration

- [ ] Day/Night Cycle
    - [x] In-Game Clock and Day Counter
    - [ ] What happens during xx:00 in game
        - [ ] XX:00
            - Spawn Zombies
        - [ ] XX:00
            - Somehow replenish resources
        - [ ] XX:00
            - Something
        - [ ] XX:00
            - Something
        - [ ] XX:00
            - Something
    - [ ] What happens when day x comes?
        - [ ] Day 1
            - Something
        - [ ] Day 2
            - Something
        - [ ] Day 3
            - Something
        - [ ] Day 4
            - Something

- [ ] UI and Menus [JUST MAKE BASIC UI, IT'S JUST A GAME JAM XD]
    - [x] In-Game Clock and Day Counter
    - [ ] xdd

choice mechanic > have consequences
can choose to pick scraps, kits, ingredients for food without going outside for shooter > risk of getting infected

3rd person shooter:
normal zombie-esque shooter
mechanic: need scraps for weapon > if no scraps then no weapons

looting:
radio, med kit etc.,

infect:
% chance of getting infected
% chance of healing
let outside > die

resource:
radio > can communicate to other living humans > trade mechanic
map > have access on parts of the map without travelling to unacessed location
flashlight > if going outside during night time have vision on place else fog of war
some food are poisonous due to infection rate > can die due to food poisoning
some med kits are ineffective/anomalies

day time cycle: night and day per 10 minutes

outside:
when attacked by hoard>can deflect attacks with parry mechanic

