namespace NeuroticOne
{
    partial class NeuroticOneForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kohonenRadioButton = new System.Windows.Forms.RadioButton();
            this.feedForwardRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.biasCheckBox = new System.Windows.Forms.CheckBox();
            this.biPolarRadioButton = new System.Windows.Forms.RadioButton();
            this.unipolarRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cycleCountTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.momentumTextBox = new System.Windows.Forms.TextBox();
            this.learningRateTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.neuronNumberTextBox = new System.Windows.Forms.TextBox();
            this.neuronNumberLabel = new System.Windows.Forms.Label();
            this.inputSizeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.outputNumberTextBox = new System.Windows.Forms.TextBox();
            this.inputNumberTextBox = new System.Windows.Forms.TextBox();
            this.layersNumberTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.learningFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadProblemButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.iterationsCountTextBox = new System.Windows.Forms.TextBox();
            this.parametersButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.parametersFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.testFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kohonenRadioButton);
            this.groupBox1.Controls.Add(this.feedForwardRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Typ sieci";
            // 
            // kohonenRadioButton
            // 
            this.kohonenRadioButton.AutoSize = true;
            this.kohonenRadioButton.Location = new System.Drawing.Point(11, 52);
            this.kohonenRadioButton.Name = "kohonenRadioButton";
            this.kohonenRadioButton.Size = new System.Drawing.Size(68, 17);
            this.kohonenRadioButton.TabIndex = 1;
            this.kohonenRadioButton.TabStop = true;
            this.kohonenRadioButton.Text = "Kohonen";
            this.kohonenRadioButton.UseVisualStyleBackColor = true;
            this.kohonenRadioButton.CheckedChanged += new System.EventHandler(this.kohonenRadioButton_CheckedChanged);
            // 
            // feedForwardRadioButton
            // 
            this.feedForwardRadioButton.AutoSize = true;
            this.feedForwardRadioButton.Location = new System.Drawing.Point(11, 29);
            this.feedForwardRadioButton.Name = "feedForwardRadioButton";
            this.feedForwardRadioButton.Size = new System.Drawing.Size(87, 17);
            this.feedForwardRadioButton.TabIndex = 0;
            this.feedForwardRadioButton.TabStop = true;
            this.feedForwardRadioButton.Text = "FeedForward";
            this.feedForwardRadioButton.UseVisualStyleBackColor = true;
            this.feedForwardRadioButton.CheckedChanged += new System.EventHandler(this.feedForwardRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.biasCheckBox);
            this.groupBox2.Controls.Add(this.biPolarRadioButton);
            this.groupBox2.Controls.Add(this.unipolarRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(197, 93);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wlasciwosci neuronow";
            // 
            // biasCheckBox
            // 
            this.biasCheckBox.AutoSize = true;
            this.biasCheckBox.Location = new System.Drawing.Point(11, 67);
            this.biasCheckBox.Name = "biasCheckBox";
            this.biasCheckBox.Size = new System.Drawing.Size(50, 17);
            this.biasCheckBox.TabIndex = 2;
            this.biasCheckBox.Text = "BIAS";
            this.biasCheckBox.UseVisualStyleBackColor = true;
            this.biasCheckBox.CheckedChanged += new System.EventHandler(this.biasCheckBox_CheckedChanged);
            // 
            // biPolarRadioButton
            // 
            this.biPolarRadioButton.AutoSize = true;
            this.biPolarRadioButton.Location = new System.Drawing.Point(11, 44);
            this.biPolarRadioButton.Name = "biPolarRadioButton";
            this.biPolarRadioButton.Size = new System.Drawing.Size(125, 17);
            this.biPolarRadioButton.TabIndex = 1;
            this.biPolarRadioButton.TabStop = true;
            this.biPolarRadioButton.Text = "Bipolarna f. aktywacji";
            this.biPolarRadioButton.UseVisualStyleBackColor = true;
            this.biPolarRadioButton.CheckedChanged += new System.EventHandler(this.biPolarRadioButton_CheckedChanged);
            // 
            // unipolarRadioButton
            // 
            this.unipolarRadioButton.AutoSize = true;
            this.unipolarRadioButton.Location = new System.Drawing.Point(11, 20);
            this.unipolarRadioButton.Name = "unipolarRadioButton";
            this.unipolarRadioButton.Size = new System.Drawing.Size(132, 17);
            this.unipolarRadioButton.TabIndex = 0;
            this.unipolarRadioButton.TabStop = true;
            this.unipolarRadioButton.Text = "Unipolarna f. aktywacji";
            this.unipolarRadioButton.UseVisualStyleBackColor = true;
            this.unipolarRadioButton.CheckedChanged += new System.EventHandler(this.unipolarRadioButton_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cycleCountTextBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.momentumTextBox);
            this.groupBox3.Controls.Add(this.learningRateTextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.neuronNumberTextBox);
            this.groupBox3.Controls.Add(this.neuronNumberLabel);
            this.groupBox3.Controls.Add(this.inputSizeTextBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.outputNumberTextBox);
            this.groupBox3.Controls.Add(this.inputNumberTextBox);
            this.groupBox3.Controls.Add(this.layersNumberTextBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(13, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(196, 249);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Wlasciwosci sieci";
            // 
            // cycleCountTextBox
            // 
            this.cycleCountTextBox.Location = new System.Drawing.Point(88, 163);
            this.cycleCountTextBox.Name = "cycleCountTextBox";
            this.cycleCountTextBox.Size = new System.Drawing.Size(100, 20);
            this.cycleCountTextBox.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "l. cykli";
            // 
            // momentumTextBox
            // 
            this.momentumTextBox.Location = new System.Drawing.Point(86, 224);
            this.momentumTextBox.Name = "momentumTextBox";
            this.momentumTextBox.Size = new System.Drawing.Size(102, 20);
            this.momentumTextBox.TabIndex = 12;
            // 
            // learningRateTextBox
            // 
            this.learningRateTextBox.Location = new System.Drawing.Point(86, 194);
            this.learningRateTextBox.Name = "learningRateTextBox";
            this.learningRateTextBox.Size = new System.Drawing.Size(102, 20);
            this.learningRateTextBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 26);
            this.label6.TabIndex = 10;
            this.label6.Text = "wspolczynnik\r\nbezwladnosci";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 26);
            this.label5.TabIndex = 9;
            this.label5.Text = "wspolczynnik\r\nuczenia";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // neuronNumberTextBox
            // 
            this.neuronNumberTextBox.Location = new System.Drawing.Point(88, 125);
            this.neuronNumberTextBox.Name = "neuronNumberTextBox";
            this.neuronNumberTextBox.Size = new System.Drawing.Size(102, 20);
            this.neuronNumberTextBox.TabIndex = 3;
            // 
            // neuronNumberLabel
            // 
            this.neuronNumberLabel.AutoSize = true;
            this.neuronNumberLabel.Location = new System.Drawing.Point(16, 132);
            this.neuronNumberLabel.Name = "neuronNumberLabel";
            this.neuronNumberLabel.Size = new System.Drawing.Size(62, 13);
            this.neuronNumberLabel.TabIndex = 8;
            this.neuronNumberLabel.Text = "l. neuronow";
            this.neuronNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputSizeTextBox
            // 
            this.inputSizeTextBox.Location = new System.Drawing.Point(88, 99);
            this.inputSizeTextBox.Name = "inputSizeTextBox";
            this.inputSizeTextBox.Size = new System.Drawing.Size(102, 20);
            this.inputSizeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 26);
            this.label4.TabIndex = 6;
            this.label4.Text = "wielkosc\r\nwejscia";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // outputNumberTextBox
            // 
            this.outputNumberTextBox.Location = new System.Drawing.Point(88, 64);
            this.outputNumberTextBox.Name = "outputNumberTextBox";
            this.outputNumberTextBox.Size = new System.Drawing.Size(102, 20);
            this.outputNumberTextBox.TabIndex = 5;
            // 
            // inputNumberTextBox
            // 
            this.inputNumberTextBox.Location = new System.Drawing.Point(88, 45);
            this.inputNumberTextBox.Name = "inputNumberTextBox";
            this.inputNumberTextBox.Size = new System.Drawing.Size(102, 20);
            this.inputNumberTextBox.TabIndex = 4;
            // 
            // layersNumberTextBox
            // 
            this.layersNumberTextBox.Enabled = false;
            this.layersNumberTextBox.Location = new System.Drawing.Point(88, 24);
            this.layersNumberTextBox.Name = "layersNumberTextBox";
            this.layersNumberTextBox.Size = new System.Drawing.Size(102, 20);
            this.layersNumberTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "l. wyjsc";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "l. wejsc";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "l. warstw";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(614, 402);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(126, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(614, 431);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(126, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // learningFileDialog
            // 
            this.learningFileDialog.InitialDirectory = "./";
            this.learningFileDialog.Title = "Przyklady uczace";
            // 
            // loadProblemButton
            // 
            this.loadProblemButton.Location = new System.Drawing.Point(614, 336);
            this.loadProblemButton.Name = "loadProblemButton";
            this.loadProblemButton.Size = new System.Drawing.Size(126, 23);
            this.loadProblemButton.TabIndex = 5;
            this.loadProblemButton.Text = "Przyklady uczace";
            this.loadProblemButton.UseVisualStyleBackColor = true;
            this.loadProblemButton.Click += new System.EventHandler(this.loadProblemButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(611, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "liczba iteracji:";
            // 
            // iterationsCountTextBox
            // 
            this.iterationsCountTextBox.Enabled = false;
            this.iterationsCountTextBox.Location = new System.Drawing.Point(614, 274);
            this.iterationsCountTextBox.Name = "iterationsCountTextBox";
            this.iterationsCountTextBox.Size = new System.Drawing.Size(126, 20);
            this.iterationsCountTextBox.TabIndex = 7;
            // 
            // parametersButton
            // 
            this.parametersButton.Location = new System.Drawing.Point(614, 307);
            this.parametersButton.Name = "parametersButton";
            this.parametersButton.Size = new System.Drawing.Size(126, 23);
            this.parametersButton.TabIndex = 8;
            this.parametersButton.Text = "Wczytaj parametry";
            this.parametersButton.UseVisualStyleBackColor = true;
            this.parametersButton.Click += new System.EventHandler(this.parametersButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(614, 365);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Przyklady testowe";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // parametersFileDialog
            // 
            this.parametersFileDialog.InitialDirectory = "./";
            this.parametersFileDialog.Title = "Parametry sieci";
            // 
            // testFileDialog
            // 
            this.testFileDialog.InitialDirectory = "./";
            this.testFileDialog.Title = "Przyklady testowe";
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(215, 16);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(390, 438);
            this.outputTextBox.TabIndex = 10;
            // 
            // NeuroticOneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 466);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.parametersButton);
            this.Controls.Add(this.iterationsCountTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.loadProblemButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "NeuroticOneForm";
            this.Text = "NeURoTiC";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton kohonenRadioButton;
        private System.Windows.Forms.RadioButton feedForwardRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox biasCheckBox;
        private System.Windows.Forms.RadioButton biPolarRadioButton;
        private System.Windows.Forms.RadioButton unipolarRadioButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox outputNumberTextBox;
        private System.Windows.Forms.TextBox inputNumberTextBox;
        private System.Windows.Forms.TextBox layersNumberTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label neuronNumberLabel;
        private System.Windows.Forms.TextBox inputSizeTextBox;
        private System.Windows.Forms.TextBox neuronNumberTextBox;
        private System.Windows.Forms.TextBox momentumTextBox;
        private System.Windows.Forms.TextBox learningRateTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.OpenFileDialog learningFileDialog;
        private System.Windows.Forms.Button loadProblemButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox iterationsCountTextBox;
        private System.Windows.Forms.Button parametersButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog parametersFileDialog;
        private System.Windows.Forms.OpenFileDialog testFileDialog;
        private System.Windows.Forms.TextBox cycleCountTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox outputTextBox;
    }
}

