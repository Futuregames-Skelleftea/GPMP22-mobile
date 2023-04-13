using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerhealth : MonoBehaviour
{
    //variables are set in the inspector
    [SerializeField] private GameOverHandler gameOverHandler;
    public void Crash()
    {
        //will call on gameoverhandler endgame method
        gameOverHandler.EndGame();

        //will set gameobject to false
        gameObject.SetActive(false);
    }
}
