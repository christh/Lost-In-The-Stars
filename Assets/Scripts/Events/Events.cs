using System;
using UnityEngine;
using UnityEngine.Events;

namespace IR
{
    public enum FadeTypes
    {
        FADE_IN,
        FADE_OUT
    }

    public class Events
    {
        [Serializable] public class EventFadeComplete : UnityEvent<FadeTypes> { }
        [Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
        [Serializable] public class EventGameUIUpdate : UnityEvent { }
        [Serializable] public class EventEnemyKilled : UnityEvent { }
    }
}