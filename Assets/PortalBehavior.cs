using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PortalBehavior : MonoBehaviourPunCallbacks
{
    private Action OnConfirmStartGame;
    private bool startedGame = false;
    public GameObject Counter;
    public TMP_Text CounterNumber;
    private void Start()
    {
        OnConfirmStartGame += StartGame;
        Counter.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !startedGame)
        {
            if (LobbyManager.instance.CanStartGame)
            {
                ModalWindowPanel.Instance.ShowModal("Start the match", null, "Do you want to start the match now?", "Yes", "No",
                    OnConfirmStartGame);
            }
        }
    }

    [PunRPC]
    private void ShowCountdown()
    {
        Counter.SetActive(true);
        StartCoroutine(CountdownMatchStart());
    }

    private void StartGame()
    {
        startedGame = true;
        photonView.RPC("ShowCountdown", RpcTarget.All);
        
    }

    private IEnumerator CountdownMatchStart()
    {
        // yield return new WaitForSeconds(1f);
        CounterNumber.text = "3";
        yield return new WaitForSeconds(1f);
        CounterNumber.text = "2";
        yield return new WaitForSeconds(1f);
        CounterNumber.text = "1";
        yield return new WaitForSeconds(1f);
        CounterNumber.text = "Ready!";
        yield return new WaitForSeconds(1f);
        GameplayManager.instance.photonView.RPC("StartMatch", RpcTarget.All);
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Level1");
    }
}
