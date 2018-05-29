using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour, IPointerDownHandler
{
    public GameObject pnGuide;

    public enum TypeScreen
    {
        left,
        right,
        guide
    }

    public TypeScreen typeScreen;

    private PlayerController player;
	
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(typeScreen)
        {
            case TypeScreen.left:
                player.Jump();
                break;
            case TypeScreen.right:
                player.Attack();
                break;
            case TypeScreen.guide:
                DoneTutorial();
                break;
            default:
                break;
        }
    }

    void DoneTutorial()
    {
        pnGuide.SetActive(false);
        GameController.StartGame();
    }
}
