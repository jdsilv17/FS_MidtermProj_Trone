using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMethods : MonoBehaviour
{
    [SerializeField] string BossRoomName = null;
    public void Restart()
    {
        SceneManager.LoadScene(BossRoomName);
    }
}
