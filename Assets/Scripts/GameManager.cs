using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    // Update is called once per frame
    private void Update()
    {
        if(player.HasReachedGoal())
        {
            SceneManager.LoadScene("Win");
        }

        if(player.IsBitten())
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
