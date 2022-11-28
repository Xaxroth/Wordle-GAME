using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Logistics")]

    [SerializeField] const int _numberOfLettersPerWord = 5;
    [SerializeField] private int _numberOfRows = 5;
    [SerializeField] private int _currentRow = 1;

    [SerializeField] private int _letterNumber;

    [SerializeField] private int index = 0;
    [SerializeField] private int currentRow = 0;

    [SerializeField] private LetterScript _letterBoxPrefab;
    [SerializeField] private GridLayoutGroup _grid = null;
    [SerializeField] private WordClass _wordClass;

    [SerializeField] private bool _allLettersCorrect;
    [SerializeField] private bool _gameOver;

    [SerializeField] public List<LetterScript> allLetters = null;
    [SerializeField] public List<ButtonScript> allButtons = null;

    public char?[] wordGuess = new char?[_numberOfLettersPerWord];

    [Header("Audio")]

    [SerializeField] private AudioSource _managerAudioSource;

    [SerializeField] private AudioClip _wrongWord;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _insufficientLetters;
    [SerializeField] private AudioClip _lose;

    [Header("Error Messages")]

    [SerializeField] private GameObject ErrorMessageWordTooShort;
    [SerializeField] private GameObject ErrorMessageWordDoesNotExist;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject LoseScreen;

    void Awake()
    {
        InitializeGame();
        ErrorMessageWordTooShort.SetActive(false);
        ErrorMessageWordDoesNotExist.SetActive(false);
        VictoryScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown && !_gameOver) // Checks for player inputs during runtime, and translates whatever input entered.
        {
            TranslateInput(Input.inputString);
        }
    }

    public void TranslateInput(string keyInput)
    {
        foreach(char c in keyInput)
        {
            if (c == '\b') // Backspace deletes the letter in the current slot
            {
                DeleteKey();
            }
            else
            {
                if (c != '\b' && c != '\n' && c != '\r') // As long as the input is not Enter, Return or Newline, the key pressed is added as a part of the list.
                {
                    EnterKey(c);
                }
                else
                {
                    if (index == _numberOfLettersPerWord) // If the index is at the last slot of the word (the word's total length), it will be a valid guess. If not, an error message appears.
                    {
                        Guess();
                    }
                    else
                    {
                        StartCoroutine(WordTooShort());
                        _managerAudioSource.PlayOneShot(_insufficientLetters);
                    }
                }
            }
        }
    }

    public void ManualTranslateInput() // The on-screen keyboard submit function - runs on clicking the "Submit" key on the screen. Works the same as submitting through keyboard.
    {
        if (index == _numberOfLettersPerWord) // If the index is at the last slot of the word (the word's total length), it will be a valid guess. If not, an error message appears.
        {
            Guess();
        }
        else
        {
            StartCoroutine(WordTooShort());
            _managerAudioSource.PlayOneShot(_insufficientLetters);
        }
    }

    private void InitializeGame() // Runs on awake and establishes the grid based on the number of rows and word length. Standard is 5 rows with a word length of 5 characters.
    {
        if (allLetters == null)
        {
            allLetters = new List<LetterScript>(); 
        }

        for (int i = 0; i < _numberOfRows; i++) // Initilalizes rows, and then for each row it makes letter boxes according to the total length of the words.
        {
            for (int j = 0; j < _numberOfLettersPerWord; j++)
            {
                LetterScript letterBox = Instantiate(_letterBoxPrefab);
                letterBox.transform.SetParent(_grid.transform);
                allLetters.Add(letterBox);
            }
        }
    }

    public void EnterKey(char c)
    {
        if (index < _numberOfLettersPerWord && !_gameOver) // As long as there are not 5 characters present, the player can input letters. For each letter entered it moves forward in the current row index.
        {
            c = char.ToUpper(c); // Converts whatever input is given into upper-case letters so that the string comparison matches. All words in the wordlist are uppercase, so the input must match.

            allLetters[(currentRow * _numberOfLettersPerWord) + index].InputLetter(c); // Changes the text value of the current letter box to the char given.
            wordGuess[index] = c;
            index++;
        }
    }

    public void DeleteKey()
    {
        if (index > 0) // As long as the index is greater than 0, we move back in the row.
        {
            index--;
            allLetters[(currentRow * _numberOfLettersPerWord) + index].DeleteLetter();
            wordGuess[index] = null;
        }
    }

    public void Guess()
    {
        StringBuilder word = new StringBuilder();

        for (int i = 0; i < _numberOfLettersPerWord; i++)
        {
            word.Append(wordGuess[i]);
        }

        if (_wordClass._wordList.Contains(word.ToString()))
        {
            // The guess is a real 5 letter word.

            for (int i = 0; i < _numberOfLettersPerWord; i++)
            {
                bool correct = wordGuess[i] == _wordClass.wordToBeGuessed[i];

                if (!correct)
                {
                    bool letterExists = false;

                    for (int j = 0; j < _numberOfLettersPerWord; j++)
                    {
                        letterExists = wordGuess[i] == _wordClass.wordToBeGuessed[j];

                        if (letterExists)
                        {
                            allLetters[i + (currentRow * _numberOfLettersPerWord)].SetLetterState(StateHandler.WrongLocation);
                            break;
                        }
                    }
                }
                else
                {
                    allLetters[i + (currentRow * _numberOfLettersPerWord)].SetLetterState(StateHandler.Correct);
                }
            }

            if (word.ToString().Equals(_wordClass.wordToBeGuessed) && _wordClass.wordToBeGuessed != null)
            {
                _managerAudioSource.PlayOneShot(_victory);
                VictoryScreen.SetActive(true);
                _gameOver = true;
            }
            else
            {
                _managerAudioSource.PlayOneShot(_wrongWord);
            }

            if (currentRow < _numberOfRows)
            {
                currentRow++;
                index = 0;
            }
            else
            {
                _managerAudioSource.PlayOneShot(_victory);
                return;
            }

            if (currentRow == _numberOfRows && !_gameOver)
            {
                _managerAudioSource.PlayOneShot(_lose);
                LoseScreen.SetActive(true);
                _currentRow = 0;
                _gameOver = true;
            }
        }
        else
        {
            StartCoroutine(WordDoesNotExist());
            return;
        }
    }

    private IEnumerator WordTooShort()
    {
        ErrorMessageWordTooShort.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorMessageWordTooShort.SetActive(false);
    }

    private IEnumerator WordDoesNotExist()
    {
        _managerAudioSource.PlayOneShot(_insufficientLetters);
        ErrorMessageWordDoesNotExist.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorMessageWordDoesNotExist.SetActive(false);
    }

    public void ResetGame()
    {
        LoseScreen.SetActive(false);
        VictoryScreen.SetActive(false);

        _wordClass.Retry(); // Calls the retry method of the wordClass script which picks a new word from the word list to be guessed. It can be the same as the previous word.

        for (int i = 0; i < allLetters.Count; i++) // Clears all boxes of information - current index is counted down on DeleteKey() until index reaches 0.
        {
            allLetters[i].ClearAll();
            DeleteKey();
        }

        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].SetColor(0);
        }

        currentRow = 0;

        _gameOver = false;
    }
}
