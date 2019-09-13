using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private PaddleController _player1;
    [SerializeField] private PaddleController _player2;

    [SerializeField] private GameObject _player1Select;
    [SerializeField] private GameObject _player2Select;

    private bool player1selected;
    private bool player2selected;

    public void PlayerOnePower()
    {
        _player1.SelectPowerPaddle();
        DisablePlayerOneSelect();
    }
    public void PlayerOneSwift()
    {
        _player1.SelectSwiftPaddle();
        DisablePlayerOneSelect();
    }
    public void PlayerOneLong()
    {
        _player1.SelectLightingPaddle();
        DisablePlayerOneSelect();
    }

    public void PlayerTwoPower()
    {
        _player2.SelectPowerPaddle();
        DisablePlayerTwoSelect();
    }
    public void PlayerTwoSwift()
    {
        _player2.SelectSwiftPaddle();
        DisablePlayerTwoSelect();
    }
    public void PlayerTwoLong()
    {
        _player2.SelectLightingPaddle();
        DisablePlayerTwoSelect();
    }

    public void DisablePlayerOneSelect()
    {
        _player1Select.SetActive(false);
        player1selected = true;
        CheckStart();
    }    
    public void DisablePlayerTwoSelect()
    {
        _player2Select.SetActive(false);
        player2selected = true;
        CheckStart();
    }

    private void CheckStart()
    {
        if(player1selected && player2selected)
        {
            FindObjectOfType<Ball>().StartGame();
            gameObject.SetActive(false);
        }
    }
}
