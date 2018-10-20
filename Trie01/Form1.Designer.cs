namespace Trie01
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtWordAdd = new System.Windows.Forms.TextBox();
            this.lblWordAdd = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnSaveToFile = new System.Windows.Forms.Button();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.txtWordAddList = new System.Windows.Forms.TextBox();
            this.btnAddList = new System.Windows.Forms.Button();
            this.txtWordRemoveList = new System.Windows.Forms.TextBox();
            this.btnRemoveList = new System.Windows.Forms.Button();
            this.txtFindResults = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearchList = new System.Windows.Forms.Button();
            this.btnSearchWord = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRemoveListBackwards = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWordRemove = new System.Windows.Forms.TextBox();
            this.btnShuffleRemoveList = new System.Windows.Forms.Button();
            this.btnShuffleAddList = new System.Windows.Forms.Button();
            this.btnTestStatsRemoveList = new System.Windows.Forms.Button();
            this.btnTestStatsAddList = new System.Windows.Forms.Button();
            this.btnResetTrie = new System.Windows.Forms.Button();
            this.btnCopyToRemoveList = new System.Windows.Forms.Button();
            this.btnRepopulateShuffled = new System.Windows.Forms.Button();
            this.btnDeleteShuffled = new System.Windows.Forms.Button();
            this.btnTestAddList = new System.Windows.Forms.Button();
            this.btnTestRemoveList = new System.Windows.Forms.Button();
            this.btnTestAddDuplicates = new System.Windows.Forms.Button();
            this.btnTestAddDuplicatesValues = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnAdd.Location = new System.Drawing.Point(166, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtWordAdd
            // 
            this.txtWordAdd.Location = new System.Drawing.Point(60, 11);
            this.txtWordAdd.Name = "txtWordAdd";
            this.txtWordAdd.Size = new System.Drawing.Size(100, 20);
            this.txtWordAdd.TabIndex = 8;
            this.txtWordAdd.Text = "a";
            // 
            // lblWordAdd
            // 
            this.lblWordAdd.AutoSize = true;
            this.lblWordAdd.Location = new System.Drawing.Point(19, 17);
            this.lblWordAdd.Name = "lblWordAdd";
            this.lblWordAdd.Size = new System.Drawing.Size(36, 13);
            this.lblWordAdd.TabIndex = 10;
            this.lblWordAdd.Text = "Word:";
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnRemove.Location = new System.Drawing.Point(166, 100);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(60, 191);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(127, 221);
            this.txtLog.TabIndex = 15;
            // 
            // btnSaveToFile
            // 
            this.btnSaveToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSaveToFile.Location = new System.Drawing.Point(193, 191);
            this.btnSaveToFile.Name = "btnSaveToFile";
            this.btnSaveToFile.Size = new System.Drawing.Size(94, 23);
            this.btnSaveToFile.TabIndex = 16;
            this.btnSaveToFile.Text = "Save to file";
            this.btnSaveToFile.UseVisualStyleBackColor = true;
            this.btnSaveToFile.Click += new System.EventHandler(this.btnSaveToFile_Click);
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnLoadFromFile.Location = new System.Drawing.Point(193, 231);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(94, 23);
            this.btnLoadFromFile.TabIndex = 17;
            this.btnLoadFromFile.Text = "Load from file";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // txtWordAddList
            // 
            this.txtWordAddList.Location = new System.Drawing.Point(381, 24);
            this.txtWordAddList.Multiline = true;
            this.txtWordAddList.Name = "txtWordAddList";
            this.txtWordAddList.Size = new System.Drawing.Size(127, 166);
            this.txtWordAddList.TabIndex = 23;
            this.txtWordAddList.Text = "a\r\nab\r\nb\r\nbcdef\r\nbcdefg\r\nbcdefgh\r\nbcdefghijklm\r\nucdefghijklm\r\nuqaz\r\nucaz\r\nuc";
            // 
            // btnAddList
            // 
            this.btnAddList.Location = new System.Drawing.Point(514, 22);
            this.btnAddList.Name = "btnAddList";
            this.btnAddList.Size = new System.Drawing.Size(80, 23);
            this.btnAddList.TabIndex = 24;
            this.btnAddList.Text = "Add List";
            this.btnAddList.UseVisualStyleBackColor = true;
            this.btnAddList.Click += new System.EventHandler(this.btnAddList_Click);
            // 
            // txtWordRemoveList
            // 
            this.txtWordRemoveList.Location = new System.Drawing.Point(380, 244);
            this.txtWordRemoveList.Multiline = true;
            this.txtWordRemoveList.Name = "txtWordRemoveList";
            this.txtWordRemoveList.Size = new System.Drawing.Size(105, 168);
            this.txtWordRemoveList.TabIndex = 28;
            this.txtWordRemoveList.Text = "bcdef\r\nuqaz\r\nb\r\nbcdefgh\r\nab\r\nucaz\r\na\r\nbcdefg\r\nucdefghijklm\r\nuc\r\nbcdefghijklm";
            // 
            // btnRemoveList
            // 
            this.btnRemoveList.Location = new System.Drawing.Point(491, 242);
            this.btnRemoveList.Name = "btnRemoveList";
            this.btnRemoveList.Size = new System.Drawing.Size(94, 23);
            this.btnRemoveList.TabIndex = 29;
            this.btnRemoveList.Text = "Remove List";
            this.btnRemoveList.UseVisualStyleBackColor = true;
            this.btnRemoveList.Click += new System.EventHandler(this.btnRemoveList_Click);
            // 
            // txtFindResults
            // 
            this.txtFindResults.Location = new System.Drawing.Point(648, 24);
            this.txtFindResults.Multiline = true;
            this.txtFindResults.Name = "txtFindResults";
            this.txtFindResults.Size = new System.Drawing.Size(143, 232);
            this.txtFindResults.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(645, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Find results:";
            // 
            // btnSearchList
            // 
            this.btnSearchList.Location = new System.Drawing.Point(514, 51);
            this.btnSearchList.Name = "btnSearchList";
            this.btnSearchList.Size = new System.Drawing.Size(75, 23);
            this.btnSearchList.TabIndex = 32;
            this.btnSearchList.Text = "Search List";
            this.btnSearchList.UseVisualStyleBackColor = true;
            this.btnSearchList.Click += new System.EventHandler(this.btnSearchList_Click);
            // 
            // btnSearchWord
            // 
            this.btnSearchWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSearchWord.Location = new System.Drawing.Point(60, 125);
            this.btnSearchWord.Name = "btnSearchWord";
            this.btnSearchWord.Size = new System.Drawing.Size(100, 23);
            this.btnSearchWord.TabIndex = 33;
            this.btnSearchWord.Text = "Search Word";
            this.btnSearchWord.UseVisualStyleBackColor = true;
            this.btnSearchWord.Click += new System.EventHandler(this.btnSearchWord_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Add:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Remove:";
            // 
            // btnRemoveListBackwards
            // 
            this.btnRemoveListBackwards.Location = new System.Drawing.Point(491, 285);
            this.btnRemoveListBackwards.Name = "btnRemoveListBackwards";
            this.btnRemoveListBackwards.Size = new System.Drawing.Size(136, 23);
            this.btnRemoveListBackwards.TabIndex = 38;
            this.btnRemoveListBackwards.Text = "Remove List Backwards";
            this.btnRemoveListBackwards.UseVisualStyleBackColor = true;
            this.btnRemoveListBackwards.Click += new System.EventHandler(this.btnRemoveListBackwards_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Add result log:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Word:";
            // 
            // txtWordRemove
            // 
            this.txtWordRemove.Location = new System.Drawing.Point(60, 99);
            this.txtWordRemove.Name = "txtWordRemove";
            this.txtWordRemove.Size = new System.Drawing.Size(100, 20);
            this.txtWordRemove.TabIndex = 40;
            this.txtWordRemove.Text = "a";
            // 
            // btnShuffleRemoveList
            // 
            this.btnShuffleRemoveList.Location = new System.Drawing.Point(492, 332);
            this.btnShuffleRemoveList.Name = "btnShuffleRemoveList";
            this.btnShuffleRemoveList.Size = new System.Drawing.Size(75, 23);
            this.btnShuffleRemoveList.TabIndex = 42;
            this.btnShuffleRemoveList.Text = "Shuffle";
            this.btnShuffleRemoveList.UseVisualStyleBackColor = true;
            this.btnShuffleRemoveList.Click += new System.EventHandler(this.btnShuffleRemoveList_Click);
            // 
            // btnShuffleAddList
            // 
            this.btnShuffleAddList.Location = new System.Drawing.Point(514, 80);
            this.btnShuffleAddList.Name = "btnShuffleAddList";
            this.btnShuffleAddList.Size = new System.Drawing.Size(75, 23);
            this.btnShuffleAddList.TabIndex = 43;
            this.btnShuffleAddList.Text = "Shuffle";
            this.btnShuffleAddList.UseVisualStyleBackColor = true;
            this.btnShuffleAddList.Click += new System.EventHandler(this.btnShuffleAddList_Click);
            // 
            // btnTestStatsRemoveList
            // 
            this.btnTestStatsRemoveList.Location = new System.Drawing.Point(648, 291);
            this.btnTestStatsRemoveList.Name = "btnTestStatsRemoveList";
            this.btnTestStatsRemoveList.Size = new System.Drawing.Size(143, 23);
            this.btnTestStatsRemoveList.TabIndex = 44;
            this.btnTestStatsRemoveList.Text = "Test - Stats - Remove List";
            this.btnTestStatsRemoveList.UseVisualStyleBackColor = true;
            this.btnTestStatsRemoveList.Click += new System.EventHandler(this.btnTestStatsRemoveList_Click);
            // 
            // btnTestStatsAddList
            // 
            this.btnTestStatsAddList.Location = new System.Drawing.Point(648, 262);
            this.btnTestStatsAddList.Name = "btnTestStatsAddList";
            this.btnTestStatsAddList.Size = new System.Drawing.Size(127, 23);
            this.btnTestStatsAddList.TabIndex = 45;
            this.btnTestStatsAddList.Text = "Test - Stats - Add List";
            this.btnTestStatsAddList.UseVisualStyleBackColor = true;
            this.btnTestStatsAddList.Click += new System.EventHandler(this.btnTestStatsAddList_Click);
            // 
            // btnResetTrie
            // 
            this.btnResetTrie.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetTrie.Location = new System.Drawing.Point(193, 271);
            this.btnResetTrie.Name = "btnResetTrie";
            this.btnResetTrie.Size = new System.Drawing.Size(115, 23);
            this.btnResetTrie.TabIndex = 46;
            this.btnResetTrie.Text = "Reset Trie Object";
            this.btnResetTrie.UseVisualStyleBackColor = true;
            this.btnResetTrie.Click += new System.EventHandler(this.btnResetTrie_Click);
            // 
            // btnCopyToRemoveList
            // 
            this.btnCopyToRemoveList.Location = new System.Drawing.Point(514, 165);
            this.btnCopyToRemoveList.Name = "btnCopyToRemoveList";
            this.btnCopyToRemoveList.Size = new System.Drawing.Size(113, 23);
            this.btnCopyToRemoveList.TabIndex = 47;
            this.btnCopyToRemoveList.Text = "Copy to Remove list";
            this.btnCopyToRemoveList.UseVisualStyleBackColor = true;
            this.btnCopyToRemoveList.Click += new System.EventHandler(this.btnCopyToRemoveList_Click);
            // 
            // btnRepopulateShuffled
            // 
            this.btnRepopulateShuffled.Location = new System.Drawing.Point(514, 109);
            this.btnRepopulateShuffled.Name = "btnRepopulateShuffled";
            this.btnRepopulateShuffled.Size = new System.Drawing.Size(99, 23);
            this.btnRepopulateShuffled.TabIndex = 48;
            this.btnRepopulateShuffled.Text = "Add List Shuffled";
            this.btnRepopulateShuffled.UseVisualStyleBackColor = true;
            this.btnRepopulateShuffled.Click += new System.EventHandler(this.btnRepopulateShuffled_Click);
            // 
            // btnDeleteShuffled
            // 
            this.btnDeleteShuffled.Location = new System.Drawing.Point(492, 389);
            this.btnDeleteShuffled.Name = "btnDeleteShuffled";
            this.btnDeleteShuffled.Size = new System.Drawing.Size(135, 23);
            this.btnDeleteShuffled.TabIndex = 49;
            this.btnDeleteShuffled.Text = "Remove List Shuffled";
            this.btnDeleteShuffled.UseVisualStyleBackColor = true;
            this.btnDeleteShuffled.Click += new System.EventHandler(this.btnDeleteShuffled_Click);
            // 
            // btnTestAddList
            // 
            this.btnTestAddList.Location = new System.Drawing.Point(648, 320);
            this.btnTestAddList.Name = "btnTestAddList";
            this.btnTestAddList.Size = new System.Drawing.Size(95, 23);
            this.btnTestAddList.TabIndex = 50;
            this.btnTestAddList.Text = "Test - Add List";
            this.btnTestAddList.UseVisualStyleBackColor = true;
            this.btnTestAddList.Click += new System.EventHandler(this.btnTestAddList_Click);
            // 
            // btnTestRemoveList
            // 
            this.btnTestRemoveList.Location = new System.Drawing.Point(648, 350);
            this.btnTestRemoveList.Name = "btnTestRemoveList";
            this.btnTestRemoveList.Size = new System.Drawing.Size(95, 23);
            this.btnTestRemoveList.TabIndex = 51;
            this.btnTestRemoveList.Text = "Test - Remove List";
            this.btnTestRemoveList.UseVisualStyleBackColor = true;
            this.btnTestRemoveList.Click += new System.EventHandler(this.btnTestRemoveList_Click);
            // 
            // btnTestAddDuplicates
            // 
            this.btnTestAddDuplicates.Location = new System.Drawing.Point(648, 379);
            this.btnTestAddDuplicates.Name = "btnTestAddDuplicates";
            this.btnTestAddDuplicates.Size = new System.Drawing.Size(127, 23);
            this.btnTestAddDuplicates.TabIndex = 52;
            this.btnTestAddDuplicates.Text = "Test - Add Duplicates";
            this.btnTestAddDuplicates.UseVisualStyleBackColor = true;
            this.btnTestAddDuplicates.Click += new System.EventHandler(this.btnTestAddDuplicates_Click);
            // 
            // btnTestAddDuplicatesValues
            // 
            this.btnTestAddDuplicatesValues.Location = new System.Drawing.Point(648, 408);
            this.btnTestAddDuplicatesValues.Name = "btnTestAddDuplicatesValues";
            this.btnTestAddDuplicatesValues.Size = new System.Drawing.Size(161, 23);
            this.btnTestAddDuplicatesValues.TabIndex = 53;
            this.btnTestAddDuplicatesValues.Text = "Test - Add Duplicates - Values";
            this.btnTestAddDuplicatesValues.UseVisualStyleBackColor = true;
            this.btnTestAddDuplicatesValues.Click += new System.EventHandler(this.btnTestAddDuplicatesValues_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 442);
            this.Controls.Add(this.btnTestAddDuplicatesValues);
            this.Controls.Add(this.btnTestAddDuplicates);
            this.Controls.Add(this.btnTestRemoveList);
            this.Controls.Add(this.btnTestAddList);
            this.Controls.Add(this.btnDeleteShuffled);
            this.Controls.Add(this.btnRepopulateShuffled);
            this.Controls.Add(this.btnCopyToRemoveList);
            this.Controls.Add(this.btnResetTrie);
            this.Controls.Add(this.btnTestStatsAddList);
            this.Controls.Add(this.btnTestStatsRemoveList);
            this.Controls.Add(this.btnShuffleAddList);
            this.Controls.Add(this.btnShuffleRemoveList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWordRemove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemoveListBackwards);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSearchWord);
            this.Controls.Add(this.btnSearchList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFindResults);
            this.Controls.Add(this.btnRemoveList);
            this.Controls.Add(this.txtWordRemoveList);
            this.Controls.Add(this.btnAddList);
            this.Controls.Add(this.txtWordAddList);
            this.Controls.Add(this.btnLoadFromFile);
            this.Controls.Add(this.btnSaveToFile);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lblWordAdd);
            this.Controls.Add(this.txtWordAdd);
            this.Controls.Add(this.btnAdd);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtWordAdd;
        private System.Windows.Forms.Label lblWordAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnSaveToFile;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.TextBox txtWordAddList;
        private System.Windows.Forms.Button btnAddList;
        private System.Windows.Forms.TextBox txtWordRemoveList;
        private System.Windows.Forms.Button btnRemoveList;
        private System.Windows.Forms.TextBox txtFindResults;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearchList;
        private System.Windows.Forms.Button btnSearchWord;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemoveListBackwards;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWordRemove;
        private System.Windows.Forms.Button btnShuffleRemoveList;
        private System.Windows.Forms.Button btnShuffleAddList;
        private System.Windows.Forms.Button btnTestStatsRemoveList;
        private System.Windows.Forms.Button btnTestStatsAddList;
        private System.Windows.Forms.Button btnResetTrie;
        private System.Windows.Forms.Button btnCopyToRemoveList;
        private System.Windows.Forms.Button btnRepopulateShuffled;
        private System.Windows.Forms.Button btnDeleteShuffled;
        private System.Windows.Forms.Button btnTestAddList;
        private System.Windows.Forms.Button btnTestRemoveList;
        private System.Windows.Forms.Button btnTestAddDuplicates;
        private System.Windows.Forms.Button btnTestAddDuplicatesValues;
    }
}

