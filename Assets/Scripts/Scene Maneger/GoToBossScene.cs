using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBossScene : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Boss");
    }
}
