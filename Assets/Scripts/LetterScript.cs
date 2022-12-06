using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterScript : MonoBehaviour
{
    // This is attached to every square in the game space, set up into a grid by the GameManager. Handles the info regarding what letter the box has storage wise, as well as the visuals.

    [SerializeField] private char? Letter { get; set; } = null;
    public StateHandler stateHandler { get; set; } = StateHandler.Default;

    public GameObject _default;

    public Text LetterText = null;

    public Image letterBackground;

    public Animator letterAnimator;

    [SerializeField] private GameManager _gameManager;

    public void Awake()
    {
        LetterText = GetComponentInChildren<Text>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void InputLetter(char c)
    {
        Letter = c;
        LetterText.text = c.ToString().ToUpper();
        StartCoroutine(ExpandCoroutine());
    }

    public void DeleteLetter()
    {
        Letter = null;
        LetterText.text = null;
    }

    public void SetLetterState(StateHandler state) // Sets the state for the letterbox, as well as the visual indication for the player.
    {
        stateHandler = state;

        StartCoroutine(FlipLetter());
    }

    public void ClearAll() // Sets the state of the current letterbox to default and removes any stored string/text data.
    {
        stateHandler = StateHandler.Default;
        Letter = null;
        LetterText.text = null;
    }

    private IEnumerator FlipLetter()
    {
        letterAnimator.SetBool("Flip", true);
        yield return new WaitForSeconds(0.4f);

        switch (stateHandler)
        {
            case StateHandler.Default:

                letterBackground.color = new Color32(35, 35, 35, 255); // Changes the background color of the letters to their appropriate color.

                break;
            case StateHandler.WrongLocation:

                letterBackground.color = new Color32(255, 220, 90, 255);

                for (int i = 0; i < _gameManager.allButtons.Count; i++)
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text)
                    {
                        _gameManager.allButtons[i].SetColor(1);
                    }
                }

                break;

            case StateHandler.Correct:

                letterBackground.color = new Color32(80, 255, 70, 255);

                for (int i = 0; i < _gameManager.allButtons.Count; i++)
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text)
                    {
                        _gameManager.allButtons[i].SetColor(2);
                    }
                }

                break;

            case StateHandler.Reset:

                letterBackground.color = new Color32(75, 75, 75, 255);

                for (int i = 0; i < _gameManager.allButtons.Count; i++) // Changes the appropriate letters on the on-screen keyboard to their respective colors.
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text) // Loops through all the buttons on the on-screen keyboard, once it finds the matching key to the correct letter, it turns that key's background the appropriate color.
                    {
                        _gameManager.allButtons[i].SetColor(0);
                    }
                }

                break;
        }

        letterAnimator.SetBool("Flip", false);
    }

    private IEnumerator ExpandCoroutine()
    {
        letterAnimator.SetTrigger("Impact");
        yield return new WaitForSeconds(0.1f);
    }
}
