using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordClass : MonoBehaviour
{
    [SerializeField] private TextAsset _allWords;

    public string wordToBeGuessed;

    [SerializeField] private Text _wordSpoiler;

    public List<string> _wordList;

    void Start()
    {
        //Removes empty entries in the attached Text File.
        _wordList = new List<string>(_allWords.text.Split(new char[] { ',', ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        Retry();

        wordToBeGuessed = GenerateNewWord();
    }

    public void Update()
    {
        _wordSpoiler.text = wordToBeGuessed;
    }

    public void Retry()
    {
        wordToBeGuessed = GenerateNewWord();
    }

    public string GenerateNewWord()
    {
        return _wordList[Random.Range(0, _wordList.Count)];
    }

    public bool CheckWord(string word)
    {
        return _wordList.Contains(word);
    }

}
