using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trie01
{
    public partial class Form1 : Form
    {

        #region Members

        public ITrie trie = null;// = new Trie();
        private static readonly long AUTO_INCREMENT_INIT_VALUE = 110;
        private AutoIncrementValue AutoIncrement;

        #endregion


        #region ClassInit

        public Form1()
        {
            InitializeComponent();

            InitCustomComponents();
        }

        private void InitCustomComponents()
        {
            trie = new Trie();

            AutoIncrement = new AutoIncrementValue(AUTO_INCREMENT_INIT_VALUE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #endregion


        #region MainFunctionality

        #region Events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string word = txtWordAdd.Text;

            long value = AutoIncrement.GetNext;
            AddWord(word, value);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string word = txtWordRemove.Text;
            if (String.IsNullOrEmpty(word))
            {
                MessageBox.Show("Invalid value");
                return;
            }
            RemoveWord(word);
        }

        private void btnSearchWord_Click(object sender, EventArgs e)
        {
            SearchWord(txtWordRemove.Text);
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = "trie.txt";
            string filePath = Path.Combine(appPath, fileName);

            trie.Save(filePath);
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = "trie.txt";
            string filePath = Path.Combine(appPath, fileName);

            trie.Load(filePath);
        }

        #endregion

        #region Functions

        private void RemoveWord(string word)
        {
            trie.Delete(word);

            string logText = txtLog.Text;
            string logNewText = String.Empty;

            if (!String.IsNullOrEmpty(logText))
            {
                string[] lines = logText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0, len = lines.Length; i < len; i++)
                {
                    string currWord = lines[i];
                    if (currWord != word)
                    {
                        if (i == 0)
                        {
                            logNewText = currWord;
                        }
                        else
                        {
                            logNewText += Environment.NewLine + currWord;
                        }
                    }
                }

                txtLog.Text = logNewText;
            }
        }

        void AddWord(string word, long value)
        {
            if (String.IsNullOrEmpty(word))
            {
                MessageBox.Show("Invalid value");
                return;
            }

            bool writeRes = trie.TryWrite(word, value);

            if (writeRes)
            {
                if (String.IsNullOrEmpty(txtLog.Text))
                {
                    txtLog.Text = word + " - " + value;
                }
                else
                {
                    txtLog.Text += Environment.NewLine + word + " - " + value;
                }
            }
            else
            {
                MessageBox.Show("Failed adding new value. Please try to free some space.");
            }
        }

        private void SearchWord(string word)
        {
            if (!String.IsNullOrEmpty(word))
            {
                long value;
                bool found = trie.TryRead(word, out value);
                txtFindResults.Text += word + " - " + (found ? value.ToString() : "NULL") + Environment.NewLine;
            }
            else
            {
                MessageBox.Show("Invalid value");
            }
        }

        #endregion

        #endregion


        #region ListsFunctions

        #region Events

        private void btnAddList_Click(object sender, EventArgs e)
        {
            AddList(true);
        }


        private void btnRemoveList_Click(object sender, EventArgs e)
        {
            RemoveList(true);
        }


        private void btnSearchList_Click(object sender, EventArgs e)
        {
            SearchAddList();
        }


        private void btnShuffleAddList_Click(object sender, EventArgs e)
        {
            ShuffleText(txtWordAddList);
        }

        private void btnRemoveListBackwards_Click(object sender, EventArgs e)
        {
            RemoveListBackwards();
        }


        private void btnShuffleRemoveList_Click(object sender, EventArgs e)
        {
            ShuffleText(txtWordRemoveList);
        }

        private void btnCopyToRemoveList_Click(object sender, EventArgs e)
        {
            CopyAddListToRemoveList();
        }


        private void btnRepopulateShuffled_Click(object sender, EventArgs e)
        {
            ResetTrieObject();
            ShuffleText(txtWordAddList);
            AddList(true);
            SearchAddList();

            //trie.Save("trie_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff") + "_add.txt");
        }

        private void btnDeleteShuffled_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string dt_formatted = dt.ToString("dd_MM_yyyy_HH_mm_ss_fff");

            ResetTrieObject();
            ShuffleText(txtWordAddList);
            AddList(true);
            SearchAddList();

            //trie.Save("trie_" + dt_formatted + "_add.txt");

            //ShuffleText(txtWordRemoveList);
            CopyAddListToRemoveList();

            RemoveList(true);

            //trie.Save("trie_" + dt_formatted + "_remove.txt");
        }

        #endregion

        #region Functions

        private void AddList(bool checkErrors)
        {
            string[] words = GetWordsArray(txtWordAddList);

            foreach (string word in words)
            {
                long value = AutoIncrement.GetNext;
                AddWord(word, value);
            }

            ((Trie)trie).CheckParentNodeRefferenceInChildrenFromRoot("ADD", checkErrors, Helper.CheckParentChildRefNotForTests);
        }

        private void RemoveList(bool checkErrors)
        {
            string[] words = GetWordsArray(txtWordRemoveList);

            foreach (string word in words)
            {
                RemoveWord(word);
            }

            ((Trie)trie).CheckParentNodeRefferenceInChildrenFromRoot("REMOVE", checkErrors, Helper.CheckParentChildRefNotForTests);
        }

        private void RemoveListBackwards()
        {
            string[] words = GetWordsArray(txtWordRemoveList);

            for (int i = words.Length - 1; i >= 0; i--)
            {
                var word = words[i];
                RemoveWord(word);
            }
        }

        private void SearchAddList()
        {
            string[] words = GetWordsArray(txtWordAddList);

            txtFindResults.Text = "";

            foreach (string word in words)
            {
                SearchWord(word);
            }
        }

        private void ShuffleText(TextBox txt)
        {
            string[] words = GetWordsArray(txt);

            words.Shuffle();

            string newText = GetSeparatedText(words);
            txt.Text = newText;
        }

        private int GetTextBoxWordsCount(TextBox txt)
        {
            string[] words = GetWordsArray(txt);

            return words.Length;
        }

        private void CopyAddListToRemoveList()
        {
            txtWordRemoveList.Text = txtWordAddList.Text;
        }

        #endregion

        #endregion


        #region HelpFunctions

        private string[] GetWordsArray(TextBox txt)
        {
            string lineSeparator = @"\r\n";
            string[] words = Regex.Split(txt.Text.Trim(lineSeparator.ToCharArray()), lineSeparator)
                .Where(s => !String.IsNullOrEmpty(s)).ToArray();

            return words;
        }

        private string GetSeparatedText(string[] lines)
        {
            string res = string.Empty;

            for (int i = 0, len = lines.Length; i < len; i++)
            {
                string currWord = lines[i];
                res += (i == 0) ? currWord : Environment.NewLine + currWord;
            }
            return res;
        }

        private void GetStats(out long rootCurrentSize, out int rootLengthOfKeyBytes)
        {
            rootCurrentSize = ((Trie)trie).root.RootCurrentSize;
            rootLengthOfKeyBytes = ((Trie)trie).root.RootLengthOfKeyBytes;
        }

        private void btnResetTrie_Click(object sender, EventArgs e)
        {
            ResetTrieObject();
        }

        private void ResetTrieObject()
        {
            trie = new Trie();
        }        

        #endregion
        

        #region Tests

        #region Events

        private void btnTestStatsAddList_Click(object sender, EventArgs e)
        {
            Test_Stats_AddList();
        }

        private void btnTestAddList_Click(object sender, EventArgs e)
        {
            Test_AddList();
        }

        private void btnTestStatsRemoveList_Click(object sender, EventArgs e)
        {
            Test_Stats_RemoveList();
        }

        private void btnTestRemoveList_Click(object sender, EventArgs e)
        {
            Test_RemoveList();
        }

        private void btnTestAddDuplicatesValues_Click(object sender, EventArgs e)
        {
            Test_Stats_AddDuplicatesValues();
        }

        private void btnTestAddDuplicates_Click(object sender, EventArgs e)
        {
            Test_Stats_AddDuplicates();
        }

        #endregion

        private void Test_Stats_RemoveList()
        {
            string issues = String.Empty;
            string log = "LOG: ";

            bool hasIssues = false;

            for (int i = 0; i < 50; i++)
            {
                trie = new Trie();
                ShuffleText(txtWordAddList);
                CopyAddListToRemoveList();

                AddList(false);
                //ShuffleText(txtWordRemoveList);

                RemoveList(false);

                long rootCurrentSize;
                int rootLengthOfKeyBytes;

                GetStats(out rootCurrentSize, out rootLengthOfKeyBytes);

                log += "rootCurrentSize: " + rootCurrentSize + Environment.NewLine
                        + "rootLengthOfKeyBytes: " + rootLengthOfKeyBytes + Environment.NewLine;

                log += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                int actual_RootAmountOfSubArrays = 0;
                int actual_RootGlobalAmountOfPopulatedNodes = 0;
                int actual_RootLengthOfKeyBytes = 0;
                long actual_StatsRootSizeForEmpty = 0;

                var trie_current = (Trie)trie;
                var trie_root_current = trie_current.root;

                trie_current.GetNodeActualStatistics(trie_root_current,
                    ref actual_RootAmountOfSubArrays,
                    ref actual_RootGlobalAmountOfPopulatedNodes,
                    ref actual_RootLengthOfKeyBytes,
                    ref actual_StatsRootSizeForEmpty);


                long current_RootCurrentSize = trie_root_current.RootCurrentSize;
                int current_RootAmountOfSubArrays = trie_root_current.RootAmountOfSubArrays;
                int current_RootGlobalAmountOfPopulatedNodes = trie_root_current.RootGlobalAmountOfPopulatedNodes;
                int current_RootLengthOfKeyBytes = trie_root_current.RootLengthOfKeyBytes;


                if (rootCurrentSize != actual_StatsRootSizeForEmpty || rootLengthOfKeyBytes != 0 ||
                    (actual_RootAmountOfSubArrays != current_RootAmountOfSubArrays
                    || actual_RootGlobalAmountOfPopulatedNodes != current_RootGlobalAmountOfPopulatedNodes
                    || actual_RootLengthOfKeyBytes != current_RootLengthOfKeyBytes))
                {
                    hasIssues = true;

                    issues += "rootCurrentSize: " + rootCurrentSize + Environment.NewLine
                                + "rootLengthOfKeyBytes: " + rootLengthOfKeyBytes + Environment.NewLine;
                    issues += Environment.NewLine + "-----------------------------" + Environment.NewLine;
                    issues += txtWordRemoveList.Text + Environment.NewLine
                        // + txtWordAddList.Text + Environment.NewLine
                        + "========================" + Environment.NewLine;
                }
            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("REMOVE ISSUES STATISTICS: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO REMOVE ISSUES STATISTICS: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        private void Test_Stats_AddList()
        {
            string issues = String.Empty;
            string log = "LOG: ";

            bool hasIssues = false;

            for (int i = 0; i < 50; i++)
            {
                trie = new Trie();
                if (i > 0)
                {
                    ShuffleText(txtWordAddList);
                }
                CopyAddListToRemoveList();

                AddList(false);
                //RemoveList(false);

                var trie_current = ((Trie)trie);
                var trie_root_current = trie_current.root;

                long current_RootCurrentSize = trie_root_current.RootCurrentSize;
                int current_RootAmountOfSubArrays = trie_root_current.RootAmountOfSubArrays;
                int current_RootGlobalAmountOfPopulatedNodes = trie_root_current.RootGlobalAmountOfPopulatedNodes;
                int current_RootLengthOfKeyBytes = trie_root_current.RootLengthOfKeyBytes;

                log += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                         + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                          + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;

                int actual_RootAmountOfSubArrays = 0;
                int actual_RootGlobalAmountOfPopulatedNodes = 0;
                int actual_RootLengthOfKeyBytes = 0;
                long actual_StatsRootSizeForEmpty = 0;

                trie_current.GetNodeActualStatistics(trie_root_current,
                    ref actual_RootAmountOfSubArrays,
                    ref actual_RootGlobalAmountOfPopulatedNodes,
                    ref actual_RootLengthOfKeyBytes,
                    ref actual_StatsRootSizeForEmpty);

                log += Environment.NewLine + "-----" + Environment.NewLine;

                log += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                         + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                          + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;


                log += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                if (/*current_RootCurrentSize != right_RootCurrentSize
                    ||*/
                    current_RootAmountOfSubArrays != actual_RootAmountOfSubArrays
                    || current_RootGlobalAmountOfPopulatedNodes != actual_RootGlobalAmountOfPopulatedNodes
                    || current_RootLengthOfKeyBytes != actual_RootLengthOfKeyBytes)
                {
                    hasIssues = true;

                    issues += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                        + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----" + Environment.NewLine;

                    issues += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                        + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                    issues += txtWordAddList.Text + Environment.NewLine
                        + "========================" + Environment.NewLine;

                    //break;
                }
            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("ADD ISSUES STATISTICS: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO ADD ISSUES STATISTICS: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        private void Test_AddList()
        {
            string issues = String.Empty;
            string log = "LOG: ";

            bool hasIssues = false;

            for (int i = 0; i < 50; i++)
            {
                ResetTrieObject();

                ShuffleText(txtWordAddList);
                CopyAddListToRemoveList();

                AddList(false);

                bool hasErrors = ((Trie)trie).CheckParentNodeRefferenceInChildrenFromRoot("Test_AddList", false, true);
                if (hasErrors)
                {
                    hasIssues = true;
                    issues += Environment.NewLine + "Test_AddList ERROR: " + Environment.NewLine
                        + txtWordAddList.Text + Environment.NewLine;

                    break;
                }
            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("ADD ISSUES: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO ADD ISSUES: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        private void Test_RemoveList()
        {
            string issues = String.Empty;
            string log = "LOG: ";

            bool hasIssues = false;

            for (int i = 0; i < 50; i++)
            {
                ResetTrieObject();

                ShuffleText(txtWordAddList);
                CopyAddListToRemoveList();

                var root = ((Trie)trie).root;


                AddList(false);

                RemoveList(false);


                bool hasErrors = false;
                bool hasCheckReferenceErrors = false;

                int numOfActualPopulatedNodes = 0;
                Trie.Node rootNode = ((Trie)trie).root;

                for (int j = 0, length = rootNode.Arr.Length; j < length; j++)
                {
                    Trie.Node node = rootNode.Arr[j];
                    if (node != null)
                    {
                        numOfActualPopulatedNodes++;
                    }
                }

                if (((Trie)trie).root.NumOfPopulatedNodes != 0 || numOfActualPopulatedNodes > 0)
                {
                    hasErrors = true;
                }


                hasCheckReferenceErrors = ((Trie)trie).CheckParentNodeRefferenceInChildrenFromRoot("Test_AddList", false, true);

                if (hasCheckReferenceErrors)
                {
                    hasErrors = true;
                }

                if (hasErrors)
                {
                    hasIssues = true;
                    issues += Environment.NewLine + "Test_RemoveList ERROR: " + Environment.NewLine
                        + "numOfActualPopulatedNodes = " + numOfActualPopulatedNodes + Environment.NewLine
                        + "hasCheckReferenceErrors = " + hasCheckReferenceErrors.ToString().ToLower() + Environment.NewLine
                        + txtWordAddList.Text + Environment.NewLine;

                    break;
                }

            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("REMOVE ISSUES: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO REMOVE ISSUES: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        private void Test_Stats_AddDuplicates()
        {
            Helper.WriteToLog("TEST DISABLED, please unmark all references to root.AutoIncrementDuplicatesTest", true);
            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
            return;

            string issues = String.Empty;
            string log = "LOG: ";

            bool hasIssues = false;

            int words_in_control = GetTextBoxWordsCount(txtWordAddList);

            for (int i = 0; i < 50; i++)
            {
                //trie = new Trie();
                if (i > 0)
                {
                    ShuffleText(txtWordAddList);
                }
                CopyAddListToRemoveList();

                //((Trie)trie).root.AutoIncrementDuplicatesTest.ReInit(0);
                Helper.WriteToLog("-------------------", Helper.WriteDuplicatesInfo);

                AddList(false);
                //RemoveList(false);

                var trie_current = ((Trie)trie);
                var trie_root_current = trie_current.root;

                long current_RootCurrentSize = trie_root_current.RootCurrentSize;
                int current_RootAmountOfSubArrays = trie_root_current.RootAmountOfSubArrays;
                int current_RootGlobalAmountOfPopulatedNodes = trie_root_current.RootGlobalAmountOfPopulatedNodes;
                int current_RootLengthOfKeyBytes = trie_root_current.RootLengthOfKeyBytes;

                log += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                         + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                          + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;

                int actual_RootAmountOfSubArrays = 0;
                int actual_RootGlobalAmountOfPopulatedNodes = 0;
                int actual_RootLengthOfKeyBytes = 0;
                long actual_StatsRootSizeForEmpty = 0;

                trie_current.GetNodeActualStatistics(trie_root_current,
                    ref actual_RootAmountOfSubArrays,
                    ref actual_RootGlobalAmountOfPopulatedNodes,
                    ref actual_RootLengthOfKeyBytes,
                    ref actual_StatsRootSizeForEmpty);

                log += Environment.NewLine + "-----" + Environment.NewLine;

                log += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                         + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                          + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;


                log += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                long test_duplicate_value = 0; // ((Trie)trie).root.AutoIncrementDuplicatesTest.GetLastAdded;

                if (/*current_RootCurrentSize != right_RootCurrentSize
                    ||*/
                    current_RootAmountOfSubArrays != actual_RootAmountOfSubArrays
                    || current_RootGlobalAmountOfPopulatedNodes != actual_RootGlobalAmountOfPopulatedNodes
                    || current_RootLengthOfKeyBytes != actual_RootLengthOfKeyBytes
                    || (i > 0 && test_duplicate_value != words_in_control))
                {
                    hasIssues = true;

                    issues += "test_duplicate_value: " + test_duplicate_value + Environment.NewLine;

                    issues += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                        + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----" + Environment.NewLine;

                    issues += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                        + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                    issues += txtWordAddList.Text + Environment.NewLine
                        + "========================" + Environment.NewLine;

                    //break;
                }
            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("ADD DUPLICATES ISSUES STATISTICS: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO ADD DUPLICATES ISSUES STATISTICS: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        private void Test_Stats_AddDuplicatesValues()
        {
            string issues = String.Empty;
            string log = "LOG: ";

            StringBuilder sbLogAddWords = new StringBuilder();

            bool hasIssues = false;

            for (int i = 0; i < 50; i++)
            {
                //trie = new Trie();
                if (i > 0)
                {
                    ShuffleText(txtWordAddList);
                }
                CopyAddListToRemoveList();

                //Add list - begin 
                string[] words = GetWordsArray(txtWordAddList);

                int words_length = words.Length;

                string[] words_added = new string[words_length];


                for (int j = 0; j < words_length; j++)
                {
                    long value = AutoIncrement.GetNext;
                    string word = words[j];

                    AddWord(word, value);

                    sbLogAddWords.AppendLine("added: " + word.PadRight(15) + " = " + value);

                    words_added[j] = word + "|" + value;
                }

                sbLogAddWords.AppendLine("-----");

                bool has_missing_words = false;
                string missing_words = String.Empty;

                for (int j = 0; j < words_length; j++)
                {
                    string[] word_value_arr = words_added[j].Split('|');

                    string word = word_value_arr[0];
                    long value = long.Parse(word_value_arr[1]);

                    long res_value = 0;
                    bool word_found = false;
                    word_found = trie.TryRead(word, out res_value);

                    sbLogAddWords.AppendLine("found: " + word.PadRight(15) + " = " + res_value.ToString().PadRight(10) + " - " + word_found.ToString().ToLower());

                    if (!word_found || !(res_value == value))
                    {
                        has_missing_words = true;
                        missing_words += (j == 0 ? word : ", " + word);
                    }
                }

                sbLogAddWords.AppendLine("has_missing_words: " + has_missing_words.ToString().ToLower()
                    + " - missing_words: " + missing_words + Environment.NewLine + "--------------------------");

                //Add list - end   

                var trie_current = ((Trie)trie);
                var trie_root_current = trie_current.root;

                long current_RootCurrentSize = trie_root_current.RootCurrentSize;
                int current_RootAmountOfSubArrays = trie_root_current.RootAmountOfSubArrays;
                int current_RootGlobalAmountOfPopulatedNodes = trie_root_current.RootGlobalAmountOfPopulatedNodes;
                int current_RootLengthOfKeyBytes = trie_root_current.RootLengthOfKeyBytes;

                log += "has_missing_words: " + has_missing_words + Environment.NewLine
                        + "missing_words: " + missing_words;

                log += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                        + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;


                int actual_RootAmountOfSubArrays = 0;
                int actual_RootGlobalAmountOfPopulatedNodes = 0;
                int actual_RootLengthOfKeyBytes = 0;
                long actual_StatsRootSizeForEmpty = 0;

                trie_current.GetNodeActualStatistics(trie_root_current,
                    ref actual_RootAmountOfSubArrays,
                    ref actual_RootGlobalAmountOfPopulatedNodes,
                    ref actual_RootLengthOfKeyBytes,
                    ref actual_StatsRootSizeForEmpty);

                log += Environment.NewLine + "-----" + Environment.NewLine;

                log += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                         + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                          + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;


                log += Environment.NewLine + "-----------------------------" + Environment.NewLine;


                if (/*current_RootCurrentSize != right_RootCurrentSize
                    ||*/
                    current_RootAmountOfSubArrays != actual_RootAmountOfSubArrays
                    || current_RootGlobalAmountOfPopulatedNodes != actual_RootGlobalAmountOfPopulatedNodes
                    || current_RootLengthOfKeyBytes != actual_RootLengthOfKeyBytes
                    || has_missing_words)
                {
                    hasIssues = true;

                    issues += "missing_words: " + missing_words + Environment.NewLine;
                    issues += Environment.NewLine + "--------" + Environment.NewLine;

                    issues += "current_RootCurrentSize: " + current_RootCurrentSize + Environment.NewLine
                        + "current_RootAmountOfSubArrays: " + current_RootAmountOfSubArrays + Environment.NewLine
                        + "current_RootGlobalAmountOfPopulatedNodes: " + current_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "current_RootLengthOfKeyBytes: " + current_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----" + Environment.NewLine;

                    issues += "actual_RootAmountOfSubArrays: " + actual_RootAmountOfSubArrays + Environment.NewLine
                        + "actual_RootGlobalAmountOfPopulatedNodes: " + actual_RootGlobalAmountOfPopulatedNodes + Environment.NewLine
                        + "actual_RootLengthOfKeyBytes: " + actual_RootLengthOfKeyBytes + Environment.NewLine;

                    issues += Environment.NewLine + "-----------------------------" + Environment.NewLine;

                    issues += txtWordAddList.Text + Environment.NewLine
                        + "========================" + Environment.NewLine;

                    //break;
                }
            }

            Helper.WriteToLog(log, Helper.WriteTestLogData);

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, Helper.WriteTestLogData);

            Helper.WriteToLog(sbLogAddWords.ToString(), Helper.WriteDuplicatesLogData);

            if (hasIssues)
            {
                Helper.WriteToLog("ADD DUPLICATES ISSUES FOR VALUES STATISTICS: " + issues, true);
            }
            else
            {
                Helper.WriteToLog("NO ADD DUPLICATES ISSUES FOR VALUES STATISTICS: " + issues, true);
            }

            Helper.WriteToLog(Environment.NewLine + "========================" + Environment.NewLine, true);
        }

        #endregion        

    }
}
