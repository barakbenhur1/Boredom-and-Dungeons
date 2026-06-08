# Merchant Shop and Run Currency System V1

Status: APPROVED FUTURE REQUIREMENT — NOT YET IMPLEMENTED
Owner categories: C06 Collectibles/Rewards/Economy, with C04 Horse, C03 Combat, C10 Hazards, C11 UI, and C12 Art/Audio integration.

## 1. Purpose

Add a recurring in-run merchant who turns exploration, combat, and breakable-object rewards into meaningful purchase decisions without interrupting the current V23R19 regression work. The system is a future implementation item and must not be marked implemented until runtime, scene generation, UI, balance, save/run state, QA, and Play Mode verification exist.

## 2. Merchant presentation

- The merchant is a seated, monk-like figure wearing a hooded cloak that hides the face.
- The cloak must use an original stylized design that belongs to the game's colorful/mysterious fantasy direction.
- The merchant sits behind a rug.
- Exactly three purchasable offers are presented on the rug at a time.
- Offer objects, prices, sold state, interaction feedback, and affordability must be readable from the approved gameplay camera and future mobile landscape view.
- Final model, animation, VFX, voice, and audio treatment are production-art tasks; a temporary procedural placeholder is not final release art.

## 3. Shop placement per run

- Every generated map/run contains between 2 and 4 merchant appearances.
- Between 1 and 2 merchants must be placed in rooms that were authored/generated as empty from the beginning of the run. A room that merely became empty after combat does not satisfy this quota.
- The remaining merchants spawn only after the player clears a combat room.
- Every run must contain at least one initially-empty-room merchant and at least one post-clear merchant.
- Merchant placement must not overlap exits, portals, hazards, required pickups, boss/mini-boss locks, or actor spawn volumes.
- A post-clear merchant may appear only after the room-clear state is final and no active enemy still owns the room.

## 4. Inventory generation

- Each merchant inventory contains exactly three distinct offers selected from the approved weighted item pool.
- Price ranges are inclusive integer ranges. The price is rolled when the stock is generated and remains stable until that stock refreshes.
- The same item must not occupy more than one slot in the same inventory generation.
- Items not explicitly marked unique remain eligible for later shops and later refreshes; final stack caps remain a balancing parameter unless specified below.
- Unique once-per-run items use a lower selection weight than normal items and therefore appear more rarely.
- A unique item may be rolled/displayed at most once across the entire run, across every merchant and refresh. After its first appearance it is removed from the run pool whether purchased or not.

## 5. Inventory refresh and paid reroll contract

### 5.1 Automatic partial refresh

A friendly merchant may automatically refill stock only when both independent gates have passed since that merchant's previous automatic refill:

1. **Time gate:** at least a configurable minimum amount of gameplay time has elapsed.
2. **Room-distance gate:** the player has completed a configurable minimum number of valid progressed-room transitions.

Automatic refresh is slot-preserving:

- Only slots that are empty because the player purchased their item are eligible to receive a new offer.
- An occupied unsold slot never changes during automatic refresh.
- If no slot is empty, no automatic stock change occurs even when both gates are ready.
- If the gates are already ready when the player later buys an item, that empty slot may refill on the next safe shop open/re-entry.
- When one or more empty slots are refilled, that merchant's time and room-distance counters restart.
- Re-entering the same room, oscillating across one doorway, pausing, or waiting beside the merchant must not farm refreshes.
- Count only valid room progress after leaving the shop room; implementation must use stable room identities rather than raw trigger entries.
- Stock must not visibly refresh while the player is actively viewing/interacting with that merchant.
- Exact initial values for minimum elapsed time and minimum rooms progressed remain configurable balance values and require approval/tuning.

### 5.2 Paid full reroll

- A friendly merchant offers a `REROLL` action with one fixed currency cost.
- The exact fixed cost is required but remains a balance value to approve before implementation.
- A successful reroll replaces all three rug slots with three newly rolled distinct offers, regardless of whether the previous slots were occupied or sold.
- The fixed cost is paid atomically: either all currency is removed and all three offers are replaced once, or neither operation occurs.
- Reroll uses the same weighted pool, unique-item run history, rarity rules, and valid price ranges as normal stock generation.
- A reroll cannot restore a unique item that has already appeared anywhere in the run.
- Reroll is unavailable while the merchant is hostile, defeated, transforming, or in combat.

### 5.3 Merchant-state restrictions

- **Friendly:** purchasing, partial automatic refresh, and paid full reroll are available.
- **Hostile and alive:** the merchant may continue appearing in later assigned merchant rooms, but displays no rug offers and provides no purchase, refresh, or reroll interaction. When the player and merchant share a room, the merchant attacks immediately.
- **Defeated:** the merchant is removed from every future merchant appearance for the rest of the run. No automatic refresh or reroll can ever occur again.
- When a friendly merchant is defeated at an active shop, every ordinary offer physically remaining on that rug becomes free to collect. Purchased/empty slots do not regenerate.
- Free rug offers are independent from the exclusive boss-weapon reward described below.
- If the merchant was already hostile from an earlier encounter, later appearances contain no rug stock; defeating that hostile appearance cannot invent free shop offers.

## 5A. Merchant aggression and combat encounter

### Aggression threshold

The merchant does not enter combat from one accidental light hit. Combat begins when any one of these thresholds is reached against the same friendly merchant state:

- at least two normal/light melee hits;
- one heavy melee hit;
- at least two player-projectile hits.

How bombs, grappling damage, spin attacks, environmental damage, companions, or mixed partial counters contribute remains a required design decision before implementation.

### Transformation presentation

After the threshold is reached:

1. shop interaction closes and all purchasing/refresh/reroll actions lock;
2. the merchant begins a dedicated stand-up animation;
3. while rising, the merchant removes the cloak and hood;
4. the revealed body is tall, broad, and physically imposing;
5. the transformation casts a large readable shadow forward;
6. the merchant equips a large cannon in the left hand and a light sword in the right hand.

The seated cloak, reveal, body proportions, shadow, cannon, light sword, VFX, animation, and audio require production-quality authored assets. Temporary procedural motion is not final release animation.

### Combat behavior

- The merchant is an aggressive high-pressure encounter and may chain actions frequently.
- The cannon fires many projectiles in organized rows distributed through an approximately 45-degree arc.
- The merchant may move while firing the cannon.
- The light sword attacks rapidly and deals high damage.
- The merchant can perform a dodge comparable in readability and purpose to the player's dodge.
- Cannon fire and light-sword attack may never execute at the same time.
- Dodge, movement, cannon bursts, sword attacks, recovery windows, hit reactions, stagger policy, health, damage, cooldowns, and arena safety require balance values and focused QA.

### Escape and run-global state

- If the player triggers combat and leaves without defeating the merchant, the run-global merchant state becomes `HostileAlive`.
- A hostile living merchant continues to occupy later merchant placements but no longer sells anything and shows no rug offers.
- A hostile living merchant attacks whenever the player enters the same room.
- Merchant hostility persists across room transitions for the remainder of the run.

### Defeat and rewards

- Defeating the merchant sets the run-global state to `Defeated`; the merchant never appears again elsewhere in that run.
- Any ordinary rug offers still physically present at the shop where the fight began become free pickups, with no purchase cost, reroll, or refresh.
- The merchant also drops the light sword and cannon as a separate exclusive reward choice.
- The player may collect exactly one of these two weapons; collecting one immediately removes the other.
- Collecting the light sword permanently doubles the player's base melee/sword damage for the remainder of the run.
- Collecting the cannon permanently doubles the player's base projectile damage for the remainder of the run.
- The player's held weapon model, attack presentation, projectile model, muzzle presentation, and related UI must update to match the selected reward.
- Reward selection is atomic and persists across room transitions for the rest of the run.

## 6. Approved item pool

### 6.1 Six player bombs

- Reward: add 6 bombs to the player's usable inventory.
- Bomb timing, explosion area, visual/audio identity, and physical behavior should match the established enemy-bomb system while using correct player ownership and damage-team rules.
- Price: 16–22.

### 6.2 Player protection

- Reward: protection equal to 25% of the player's current maximum health capacity.
- Protection is presented as a separate shield/armor bar above the health bar in a clearly different color.
- Incoming damage consumes protection before normal health, including correct overflow handling.
- Price: 40–46.

### 6.3 Player maximum health increase

- Reward: increase player maximum health by 10%.
- Price: 110–116.

### 6.4 New stronger weapon — unique

- Reward: replace/upgrade the normal sword combat loadout with a weapon whose damage range is stronger than the normal weapon.
- Current visual direction candidate: two axes instead of the sword. This visual is not final until approved in production art.
- May appear only once per run and uses a reduced rare-item selection weight.
- Price: exactly 400.

### 6.5 Horse protection

- Reward: protection equal to 25% of the horse's maximum health, with a separate shield/armor bar using a distinct color.
- While the horse still has protection and the player is mounted, incoming mounted damage routes to the horse/protection instead of the player.
- The horse does not buck/throw the rider because of that protected hit while protection was active at impact.
- Normal routing resumes after protection is depleted.
- Price: 54–62.

### 6.6 Horse maximum health increase

- Reward: increase horse maximum health by 10%.
- Price: 120–124.

### 6.7 Grappling-hook range increase — unique

- Reward: increase grappling-hook range by 25%.
- May appear only once per run and uses a reduced rare-item selection weight.
- Price: 200–220.

### 6.8 Horse speed increase — unique

- Reward: increase horse movement speed by 8%.
- May appear only once per run and uses a reduced rare-item selection weight.
- Price: 240–260.

### 6.9 Projectile damage improvement

- Reward: increase player projectile damage by 2%.
- Price: 80–94.

### 6.10 Grappling-hook damage improvement

- Reward: increase grappling-hook damage by 4%.
- Price: 60–70.

### 6.11 Hazard-damage reduction

- Reward: reduce hazard damage received by the player by 10%.
- Price: 52–60.

## 7. Run currency sources

### Enemy drops

- Every defeated enemy receives an independent 20% chance to drop money.
- Successful amount: random integer from 3 through 8.
- Elite/Battery guardians remain enemies for damage/combat classification and therefore participate unless a later explicit balance exception is approved.

### Breakable-object rewards

- Appropriate breakable objects such as pots receive a 30% chance for money to occupy the relevant reward result.
- Successful amount: random integer from 2 through 10.
- Where the breakable already selects one contained reward, money replaces that reward result rather than silently duplicating every reward.

### Currency truth

- The current wallet and exact price must be visible during shop interaction.
- Purchase is atomic: currency is removed and the reward is granted exactly once, or neither operation occurs.
- The persistence scope of money beyond the current run has not been approved; implementation must not assume permanent meta-currency.

## 8. Required implementation ownership

Future implementation should extend established owners rather than create duplicates:

- Run/map generation owns merchant count, placement class, and room identity.
- Room-clear state owns post-clear spawn eligibility.
- A dedicated shop/economy runtime owner owns weighted stock, dual-gate refresh state, unique-offer run state, prices, purchases, and wallet mutations.
- Existing player, combat, horse, health, hazard, projectile, grappling, inventory, and HUD systems own the actual purchased effects.
- The shop UI must not duplicate health, shield, horse, or currency truth.

## 9. Future acceptance gate

1. Every run produces 2–4 merchants, with at least one initially-empty-room merchant and at least one post-clear merchant.
2. Every inventory has three distinct valid offers and prices within the approved ranges.
3. Waiting alone or walking through one doorway repeatedly cannot refresh stock.
4. Stock refreshes only after both configured time and room-progress thresholds pass.
5. Unique items are lower-weight and can appear at most once in the entire run.
6. Enemy and breakable money probabilities and inclusive amount ranges are statistically and deterministically testable.
7. Purchases are atomic and cannot duplicate rewards or produce negative currency.
8. Player and horse protection bars, routing, depletion, overflow, and mounted no-buck behavior match the approved contract.
9. All percentage upgrades affect their real existing system exactly once per purchase and survive room transitions/re-entry for the remainder of the run.
10. Merchant art, three rug offers, prices, interaction feedback, animations, audio, and mobile-landscape readability pass production review.
11. TEST EVERYTHING includes generation, economy-state, unique-pool, refresh, purchase, and routing regression checks.
12. Focused Play Mode covers repeated visits, partial empty-slot refresh, occupied-slot preservation, paid full reroll, insufficient funds, death/restart, and new-run reset.
13. Friendly, HostileAlive, and Defeated merchant states persist correctly across all merchant placements in one run.
14. Aggression thresholds, transformation, forward shadow, cannon/sword exclusivity, movement while firing, dodge, escape, and re-encounter behavior match the approved contract.
15. Defeat exposes only existing rug offers for free, permanently disables refresh/reroll, removes future merchant appearances, and enforces the one-of-two weapon reward choice.
16. Light-sword/cannon rewards double only their approved base-damage channel and update the corresponding player weapon/projectile presentation.

## Relationship to the future Caterpillar gambling NPC

The merchant shop and Caterpillar gambling NPC both use run money but are separate systems.

- The merchant exchanges money for shop inventory.
- A Caterpillar offers one assigned gambling game and owns a finite bankroll.
- Caterpillars appear only in rooms selected for them and only while those rooms are clear.
- Their animated visibility, game-session safety, bankroll threshold/refill behavior and anti-farm rules are canonical in `CATERPILLAR_GAMBLING_NPC_V1.md`.
- Do not reuse shop refresh, merchant hostility, inventory slots or merchant death rules unless a later approved integration explicitly requires it.
