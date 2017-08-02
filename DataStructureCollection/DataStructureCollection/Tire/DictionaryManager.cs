using System;
using System.Collections.Generic;


namespace DataStructureCollection.Trie
{
    class DictionaryManager
    {
        FourLettersDictionary dictionary;
        private static DictionaryManager instance;
        public static DictionaryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DictionaryManager();
                }
                return instance;
            }
        }

        public DictionaryManager()
        {
            dictionary = new FourLettersDictionary();
        }

        public void SaveDictionary()
        {
            dictionary.Save();
        }

        public bool FindWordByDictionary(string word, out Word wordOut, ICustomDictionary customDictionary = null)
        {
            if (customDictionary != null)
            {
                wordOut = customDictionary.Find(word);
            }
            wordOut = dictionary.Find(word);

            if (wordOut == null)
                return false;
            else
                return true;
        }
        public bool AddWordByDictionary(Word word, ICustomDictionary customDictionary = null)
        {
            if (customDictionary != null)
            {
                return customDictionary.AddWord(word);
            }
            return dictionary.AddWord(word);
        }
        public bool RemoveWordByDictionary(string word, ICustomDictionary customDictionary = null)
        {
            if (customDictionary != null)
            {
                return customDictionary.RemoveWord(word);
            }
            return dictionary.RemoveWord(word);
        }
    }

    //[Serializable]
    class FourLettersDictionary : ICustomDictionary
    {
        public FourLettersDictionary()
        {
            Load();
        }
        WordNode root;
        private bool IsLetterVaild(char letter)
        {
            if ((int)letter >= (int)'a' && (int)letter <= (int)'z')
            {
                return true;
            }
            else
            {
                throw new Exception("the word is invalid");
                //return false;
            }

        }

        public bool Load()
        {
            root = new WordNode();
            return true;
        }


        public bool AddWord(Word word)
        {
            try
            {
                WordNode currentNode = root;
                for (int i = 0; i < word.Name.Length; i++)
                {
                    WordNode node;
                    if (IsLetterVaild(word.Name[i]))
                    {
                        if (currentNode.WordNodeDictionary.TryGetValue(word.Name[i], out node))
                        {
                            currentNode = node;
                        }
                        else
                        {
                            node = new WordNode() { Letter = word.Name[i] };
                            currentNode.WordNodeDictionary.Add(word.Name[i], node);
                            currentNode = node;
                        }
                    }
                }
                if (currentNode.Word == null)
                {
                    currentNode.Word = word;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool RemoveWord(string word)
        {
            return true;
        }


        //if word is not in the dictionary,the return value will be null.
        public Word Find(string word)
        {
            var currentNode = root;
            for (int i = 0; i < word.Length; i++)
            {
                if (currentNode != null && currentNode.WordNodeDictionary.ContainsKey(word[i]))
                {
                    currentNode = currentNode.WordNodeDictionary[word[i]];
                }
                else
                {
                    return null;
                }
            }
            if (currentNode != null)
            {
                return currentNode.Word;
            }
            return null;
        }

        public void Save()
        { }

        class WordNode
        {
            char letter;
            Dictionary<char, WordNode> dictionary = new Dictionary<char, WordNode>();
            Word word;

            public Dictionary<char, WordNode> WordNodeDictionary { get { return dictionary; } }

            public bool IsEnd { get { return word != null; } }

            public Word Word
            {
                get
                {
                    return word;
                }
                set
                {
                    word = value;
                }
            }

            public char Letter
            {
                get
                {
                    return letter;
                }
                set
                {
                    if (((int)value >= (int)'a' && (int)value <= (int)'z'))
                    {
                        letter = value;
                    }
                }
            }
        }
    }

    public class Word
    {
        string name;
        string description;

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public interface ICustomDictionary
    {
        Word Find(string word);
        //if add successfully it will return true, otherwise false
        bool AddWord(Word word);
        //if remove successfully it will return true, otherwise false
        bool RemoveWord(string word);
    }
}

