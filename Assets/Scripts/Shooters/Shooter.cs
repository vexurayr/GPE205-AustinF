using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    public abstract void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan);
}