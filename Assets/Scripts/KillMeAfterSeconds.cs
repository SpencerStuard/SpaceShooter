using UnityEngine;
using System.Collections;

public class KillMeAfterSeconds : MonoBehaviour {


    public void SetUpKillMe(float KillTime)
    {
        Invoke("DestroyNow", KillTime);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
