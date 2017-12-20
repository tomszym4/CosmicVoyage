using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileReady : MonoBehaviour {

    private void OnEnable()
    {
        GameManager.lose += Disabling;
    }
    private void OnDisable()
    {
        GameManager.lose -= Disabling;
    }

    private void Disabling()
    {
        gameObject.SetActive(false);
    }
}
