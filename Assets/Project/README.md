# Backbone Systems - State Machine / Event System / Game Manager / Game Data

Owner: Divan. This README explains how these four systems fit together and how the rest
of the team should hook into them. You shouldn't need to read GameManager's source to use
it - this doc + the tooltips/comments in the scripts should be enough.

## Folder structure

```
Assets/_Project/Scripts/Core/
   GameManager.cs                     <- the central hub, one per scene
   StateMachine/
      IState.cs
      GameStateMachine.cs
      States/
         MainMenuState.cs
         DayStartState.cs
         QueueState.cs
         DayEndState.cs
         GameOverState.cs
   Events/
      VoidEventChannelSO.cs / VoidEventListener.cs
      IntEventChannelSO.cs  / IntEventListener.cs
      StringEventChannelSO.cs / StringEventListener.cs
   Data/
      GameData.cs
```

The `.asset` files you create from these (the actual event channels) should go in
`Assets/Project/Events/` - keep them separate from scripts so the Project window stays
readable.

## The mental model

There's **one flow** through the game: `MainMenu → DayStart → Queue → DayEnd → (loop back
to DayStart, or → GameOver)`. `GameManager` owns a `GameStateMachine`, which holds the
current `IState`. Each state's `Enter()` raises an event announcing that phase has begun;
your systems listen for the events that matter to you and ignore the rest.

**You should never need a direct reference from your script to someone else's script.**
Document/Rule/Verification/Queue/Dialogue/Economy/UI all talk to each other by raising and
listening to events on `GameManager`, and by reading `GameManager.Instance.Data`. That's
the whole point of the architecture - you can build and test your system before anyone
else's code exists, as long as you know which events to raise/listen for.

## Setting it up in the scene (one-time)

1. Empty GameObject named `GameManager`, add the `GameManager` component.
2. In the Project window: `Create → Events →` (Void / Int / String) `Event Channel` for
   each event listed in the GameManager Inspector (OnDayStarted, OnQueueStarted,
   OnQueueEmpty, OnDayEndStarted, OnDayResolved, OnGameOver, OnMoneyChanged). Name them to
   match, drop them in `Assets/Project/Events/`.
3. Drag each asset into its matching slot on the `GameManager` component.
4. Need a new event that isn't listed above? Create the asset, add a public field for it on
   `GameManager` (or ask Divan to), and document what it means in the tooltip.

## How to listen for an event

**No-code way:** put a `Void/Int/String Event Listener` component on any GameObject, drag
in the channel asset, wire the response like a Button's OnClick.

**Code way:**

```csharp
[SerializeField] private VoidEventChannelSO onDayStarted;

private void OnEnable()  { onDayStarted.OnEventRaised += HandleDayStarted; }
private void OnDisable() { onDayStarted.OnEventRaised -= HandleDayStarted; }

private void HandleDayStarted() { /* generate today's rules, etc. */ }
```

Always unsubscribe in `OnDisable`

## Who raises / listens to what (starting point - extend as needed)

Event | Raised by | Who should listen |

`OnDayStarted` | DayStartState | Rule System (generate today's rules), UI (briefing screen) |
`OnQueueStarted` | QueueState | Queue System (start pulling applicants) |
`OnQueueEmpty` | **Queue System** | GameManager (moves to Day End) |
`OnDayEndStarted` | DayEndState | Economy System (tally salary/expenses/bribes) |
`OnDayResolved` | **Economy System** | GameManager (loops to next day or Game Over) |
`OnGameOver` | GameOverState | UI (show Game Over screen + `Data.GameOverReason`) |
`OnMoneyChanged` | `GameManager.AddMoney()` | UI (update money display) |

The two bolded rows are the two places your systems need to actively raise an event back
to GameManager - everything else is one-directional (a state tells you something started).

## Reading/writing game data

```csharp
GameManager.Instance.Data.CurrentDay
GameManager.Instance.Data.CurrentMoney
GameManager.Instance.AddMoney(50);      // use this instead of writing CurrentMoney directly
GameManager.Instance.TriggerGameOver("Ran out of money for rent.");
```

`GameData` is **runtime-only** - it resets every time Play is pressed. Don't build save/load
on top of it without checking with Divan first, since that changes the "resets every
session" assumption everything else is built on.

## Using Tick() / Update() correctly

Every frame, Unity calls `GameManager.Update()` → `StateMachine.Tick()` → `CurrentState.Tick()`
on whichever `IState` is currently active. Only the active state's `Tick()` runs — e.g.
`DayEndState.Tick()` does nothing at all while you're in `QueueState`.

**If your logic only matters during one specific phase and is small** (a timer, simple
polling), just write it directly inside that state's `Tick()` method. Example —
`QueueState` tracking how long an applicant has been waiting:

```csharp
public void Tick()
{
    waitTimer += Time.deltaTime;
    if (waitTimer > 30f) { /* applicant gets impatient */ }
}
```

**If you're building a whole system** (Verification, Dialogue, Queue spawning, etc.), it
should be its own MonoBehaviour with its own `Update()` - don't stuff your system's logic
into someone else's state class just because that's where `Tick()` lives. To stop your
`Update()` from running when it's not relevant, gate it with one of:

```csharp
// Cheap: check the phase at the top of Update()
private void Update()
{
    if (GameManager.Instance.Data.CurrentPhase != DayPhase.Queue) return;
    // your logic
}
```

```csharp
// Cleaner: toggle `enabled` via events so Update() doesn't run at all when off
private void OnEnable()
{
    gm.OnQueueStarted.OnEventRaised += () => enabled = true;
    gm.OnQueueEmpty.OnEventRaised  += () => enabled = false;
}
```

Rule of thumb: `IState.Tick()` is for logic tightly bound to _being in that phase_ and
small enough to live in the state itself. A whole system with meaningful per-frame work
gets its own script and gates itself off your events instead.

## Adding a new state

1. New class implementing `IState` in `StateMachine/States/`, constructor takes
   `GameManager gm`.
2. It should only be entered/exited by GameManager. If you think you need a new state
   (e.g. a Bribe/Negotiation sub-phase), talk to Divan first - extra states multiply the
   number of transitions everyone else needs to reason about.
