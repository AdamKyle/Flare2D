using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        if (CoinSceneDeletion.coinsToDelete.ContainsKey(gameObject.name)) {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update() {}
}
