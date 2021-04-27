using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandle : MonoBehaviour
{
    [SerializeField] BaseEnemy enemy = null;
    public void TakeDamage(int _damage)
    {
        enemy.TakeDamage(_damage);
    }
    public void TakeDamage(int _damage,Vector3 _hitPosition)
    {
        enemy.TakeDamage(_damage, _hitPosition);
    }
}
