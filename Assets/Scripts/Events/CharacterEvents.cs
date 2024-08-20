using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events
{
    public class CharacterEvents
    {
        public static UnityAction<GameObject, float> characterDamaged;
        public static UnityAction<GameObject, float> characterHealed;
        public static UnityAction<PlayerController, int> pointsAdded;
    }
}