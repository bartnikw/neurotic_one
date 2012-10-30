using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using AForge.Controls;
using AForge.Math;
using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System.Globalization;

namespace NeuroticOne
{
    enum NeuroticProgramType{
        NeuroticProgramTypeKohonen,
        NeuroticProgramTypeFeedForward
    };

    enum NeuroticActivationFunctionType
    {
        NeuroticActivationFunctionTypeBipolar,
        NeuroticActivationFunctionTypeUnipolar
    };
    
    public partial class NeuroticOneForm : Form
    {

        delegate void SetEnabledCallback(bool value);
        delegate void SetIterationsCountCallback(int count);
        delegate void UpdateLabelsCallback();
        delegate void WriteCallback(string value);

        private const int MaxLearningLines = 50;
        private bool bias = false;

        private NeuroticProgramType currentNetworkType;
        private NeuroticActivationFunctionType currentActivationFunctionType;

        private int[] hiddenLayer;
        private int inputValues;
        private int outputValues;

        private bool parametersLoaded = false;
        private bool teachingsLoaded = false;
        private bool testsLoaded = false;

        private int numberOfCycles;
        private double learningRate = 0.1;
        private double momentum = 0.0;
        
        private Thread workerThread = null;
        private bool needToStop = false;

        private double [,] trainingFFData = null;
        private double[,] testFFData = null;

        bool teachingDone = false;
        private double[][] trainingKData; //= new double[pointsCount][];
        private double[][] testKData;


        private ActivationNetwork activationNetwork;
        private DistanceNetwork kohonenNetwork;


        //kohonen
        private int liczba_wejsc;
        private int liczba_neuronow_poziom;
        private int liczba_neuronow_pion;
        //private int cykle_nauki;
        private int pocz_rozmiar_sasiedz;
        private double wsp_zmian_rozm_sasiedz;
        private double pocz_wart_wsp_nauki;
        private double wps_zmiany_wsp;


        public void UpdateValues()
        {
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateTextBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }

            try
            {
                momentum = Math.Max(0, Math.Min(0.5, double.Parse(momentumTextBox.Text)));
            }
            catch
            {
                momentum = 0;
            }
            try
            {
                inputValues = Math.Max(1, int.Parse(inputNumberTextBox.Text));
            }
            catch
            {
                inputValues = 1;
            }
            try
            {
                outputValues = Math.Max(1, int.Parse(outputNumberTextBox.Text));
            }
            catch
            {
                outputValues = 1;
            }
            try
            {
                numberOfCycles = Math.Max(1, int.Parse(cycleCountTextBox.Text));
            }
            catch
            {
                numberOfCycles = 1000;
            }

            UpdateLabels();
        }

        public void UpdateLabels()
        {

            if (this.startButton.InvokeRequired)
            {
                UpdateLabelsCallback d = new UpdateLabelsCallback(UpdateLabels);
                this.Invoke(d);
            }
            else
            {
                this.learningRateTextBox.Text = learningRate.ToString();
                this.momentumTextBox.Text = momentum.ToString();
                this.inputNumberTextBox.Text = inputValues.ToString();
                this.outputNumberTextBox.Text = outputValues.ToString();
                this.cycleCountTextBox.Text = numberOfCycles.ToString();
                //this.startButton.Enabled = parametersLoaded && teachingsLoaded && testsLoaded;
                if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
                {
                    this.kohonenRadioButton.Checked = true;
                }
                else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
                {
                    this.feedForwardRadioButton.Checked = true;
                }
                if (this.hiddenLayer != null) this.layersNumberTextBox.Text = hiddenLayer.Length.ToString();
                this.startButton.Enabled = this.teachingDone && parametersLoaded && testsLoaded;
                this.teachButton.Enabled = parametersLoaded && teachingsLoaded;
            }
        }

        public NeuroticOneForm()
        {
            InitializeComponent();
            currentNetworkType = NeuroticProgramType.NeuroticProgramTypeFeedForward;
            currentActivationFunctionType = NeuroticActivationFunctionType.NeuroticActivationFunctionTypeUnipolar;
            inputValues = outputValues = 1;
            numberOfCycles = 100;
            BlockInterface(false);

            //this.parametersFileDialog.InitialDirectory = Path.Combine(Application.ExecutablePath, "Files");
            string current = System.IO.Directory.GetCurrentDirectory();

            current=  Directory.GetParent(Directory.GetParent(Directory.GetParent(current).FullName).FullName).FullName;//System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            current = Path.Combine(current, "Files");
            this.parametersFileDialog.InitialDirectory = current;
            this.parametersFileDialog.FileName = "settings.txt";
            this.testFileDialog.InitialDirectory = current;
            this.testFileDialog.FileName = "test.txt";
            this.learningFileDialog.InitialDirectory = current;
            this.learningFileDialog.FileName = "teachunipolar.txt";
            UpdateLabels();
        }
        #region InterfaceUpdates


        private void biasCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bias = biasCheckBox.Checked;
        }

        private void unipolarRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.currentActivationFunctionType = NeuroticActivationFunctionType.NeuroticActivationFunctionTypeUnipolar;
        }

        private void biPolarRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.currentActivationFunctionType = NeuroticActivationFunctionType.NeuroticActivationFunctionTypeBipolar;
        }
        private void BlockInterface(bool value)
        {
            
            if (this.feedForwardRadioButton.InvokeRequired)
			{	
				SetEnabledCallback d = new SetEnabledCallback(BlockInterface);
				this.Invoke(d, new object[] { value });
			}
			else
			{
                bool enabled = !value;
                this.feedForwardRadioButton.Enabled = enabled;
                this.kohonenRadioButton.Enabled = enabled;
                this.unipolarRadioButton.Enabled = enabled;
                this.biasCheckBox.Enabled = enabled;
                this.biPolarRadioButton.Enabled = enabled;
                this.inputNumberTextBox.Enabled = enabled;
                this.outputNumberTextBox.Enabled = enabled;
                this.momentumTextBox.Enabled = enabled;
                this.learningRateTextBox.Enabled = enabled;
                //this.layersNumberTextBox.Enabled = enabled;

                this.neuronNumberTextBox.Enabled = enabled;
                this.inputSizeTextBox.Enabled = enabled;
                this.loadProblemButton.Enabled = enabled;


                this.startButton.Enabled = enabled;
                this.stopButton.Enabled = !enabled;
			}
            
        }

        private void SetIterationsCount(int value)
        {
            if (this.iterationsCountTextBox.InvokeRequired)
            {
                SetIterationsCountCallback d = new SetIterationsCountCallback(SetIterationsCount);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.iterationsCountTextBox.Text = value.ToString();
            }
        }

        private void SwitchInterface(NeuroticProgramType type)
        {
            bool value = type == NeuroticProgramType.NeuroticProgramTypeFeedForward;
            //this.layersNumberTextBox.Enabled = value;
            this.inputNumberTextBox.Enabled = value;
            this.outputNumberTextBox.Enabled = value;
            this.neuronNumberTextBox.Enabled = !value;
            this.inputSizeTextBox.Enabled = !value;

            if (value)
            {
                this.neuronNumberTextBox.Text = "";
                this.inputSizeTextBox.Text = "";
            }
            else
            {
                this.layersNumberTextBox.Text = "";
                this.inputNumberTextBox.Text = "";
                this.outputNumberTextBox.Text = "";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SwitchInterface(NeuroticProgramType.NeuroticProgramTypeFeedForward);
        }

        private void feedForwardRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == this.feedForwardRadioButton && this.feedForwardRadioButton.Checked)
            {
                this.SwitchInterface(NeuroticProgramType.NeuroticProgramTypeFeedForward);
            }
        }

        private void kohonenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == this.kohonenRadioButton && this.kohonenRadioButton.Checked)
            {
                this.SwitchInterface(NeuroticProgramType.NeuroticProgramTypeKohonen);
            }
        }
        #endregion
        #region Writing_Texbox
        private void Write(string text)
        {
            if (this.outputTextBox.InvokeRequired)
			{	
				WriteCallback d = new WriteCallback(Write);
				this.Invoke(d, new object[] { text });
			}
            else
            {
                string currentText = this.outputTextBox.Text;
                string newText = String.Concat(currentText,text);
                this.outputTextBox.Text = newText;
            }
        }

        private void WriteLine(string text)
        {
            this.Write(String.Concat(text, Environment.NewLine));
        }
        #endregion

        void FeedForwardTeach()
        {
            int samples = trainingFFData.GetLength(0);

            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            WriteLine("Teaching:");
            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[inputValues];
                output[i] = new double[outputValues];

                // set input
                for (int q = 0; q < inputValues; ++q)
                {
                    input[i][q] = trainingFFData[i, q];
                }

                // set output
                for (int q = inputValues; q < inputValues + outputValues; ++q)
                {
                    output[i][q-inputValues] = trainingFFData[i, q];
                }
                
                Write("[");
                for (int q = 0; q < inputValues; ++q)
                {
                    Write(String.Concat(" ", input[i][q].ToString()));
                }
                Write(" ] -> [");
                for (int q = 0; q < outputValues; ++q)
                { 
                    Write(string.Concat(" ", output[i][q].ToString()));
                }
                WriteLine(" ];");
                   
            }
            //IActivationFunction
            int[] newHidden = new int[hiddenLayer.Length+1];
            Array.Copy(hiddenLayer, 0, newHidden, 0, hiddenLayer.Length);
            newHidden[newHidden.Length-1] = outputValues;
        //    newHidden[0] = inputValues;
            activationNetwork = new ActivationNetwork(new BipolarSigmoidFunction(), inputValues, newHidden);
            ISupervisedLearning teacher;
            if (bias)
            {
                teacher = new BackPropagationLearning(activationNetwork);
                BackPropagationLearning teacherbp = teacher as BackPropagationLearning;
                teacherbp.LearningRate = learningRate;
                teacherbp.Momentum = momentum;
            }
            else
            {
                teacher = new BackPropagationLearningWithoutBias(activationNetwork);
                BackPropagationLearningWithoutBias teacherbp = teacher as BackPropagationLearningWithoutBias;
                teacherbp.LearningRate = learningRate;
                teacherbp.Momentum = momentum;
            }
            SetIterationsCount(0);
            int iteration = 1;

            

            while (!needToStop)
            {
                double error = teacher.RunEpoch(input, output);

                SetIterationsCount(iteration);
                if (++iteration > numberOfCycles) break;
            }
            BlockInterface(false);
            teachingDone = true;
            needToStop = false;
            UpdateLabels();
        }

        void FeedForwardTest()
        {
            WriteLine("Test:");
            double[] networkInput = new double[inputValues];
            for (int j = 0; !needToStop && j < this.testFFData.GetLength(0); ++j)
            {
                Write("[");
                for (int jj = 0; jj < inputValues; ++jj)
                {
                    networkInput[jj] = this.testFFData[j, jj];
                    Write(String.Concat(" ", networkInput[jj].ToString()));
                }
                double[] result = bias ? activationNetwork.Compute(networkInput) : activationNetwork.ComputeWithoutBias(networkInput);
                Write(" ] -> [");
                for (int jj = 0; jj < outputValues; ++jj)
                {
                    Write(string.Concat(" ", Math.Round(result[jj], 5).ToString()));
                }
                WriteLine(" ];");
            }
            BlockInterface(false);
            needToStop = false;
        }

        void KohonenTeach()
        {

            Neuron.RandRange = new Range(0.0f, 1.0f);
            kohonenNetwork = new DistanceNetwork(liczba_wejsc, liczba_neuronow_pion * liczba_neuronow_poziom);
            kohonenNetwork.Randomize();
            SOMLearning teacher = new SOMLearning(kohonenNetwork);


            double driftingLearningRate = this.wps_zmiany_wsp;
            double fixedLearningRate = this.pocz_wart_wsp_nauki;
            double learningRadius = this.pocz_rozmiar_sasiedz;
            double driftingLearningRadius = this.wsp_zmian_rozm_sasiedz;
            int iteration = 1;
            while (!needToStop)
            {
                teacher.LearningRate = driftingLearningRate * (numberOfCycles - iteration) / numberOfCycles + fixedLearningRate;
                teacher.LearningRadius = (double)learningRadius * (numberOfCycles - iteration) / numberOfCycles;

                teacher.RunEpoch(trainingKData);

                SetIterationsCount(iteration++);
                if (iteration > numberOfCycles) break;
            }
            teachingDone = true;
            needToStop = false;
            UpdateLabels();

        }


        void KohonenTest()
        {
            WriteLine("Testowanie Kohonena");
            int oneVectorLength = this.testKData[0].Length;
            double[] networkInput = new double[oneVectorLength];
            for (int j = 0; !needToStop && j < this.testKData.Length; ++j)
            {
                Write("[");
                for (int jj = 0; jj < oneVectorLength; ++jj)
                {
                    networkInput[jj] = this.testKData[j][jj];
                    Write(String.Concat(" ", networkInput[jj].ToString()));
                }
                Write(" ] -> ");
                kohonenNetwork.Compute(networkInput);
                int num = kohonenNetwork.GetWinner();
                WriteLine(String.Format("{0} ;", num));
                //double[] result = activationNetwork.Compute(networkInput);
                //Write(" ] -> [");
                //for (int jj = 0; jj < outputValues; ++jj)
                //{
                //    Write(string.Concat(" ", Math.Round(result[jj], 5).ToString()));
                //}
                //WriteLine(" ];");
            }
            BlockInterface(false);
            needToStop = false;
        }

       

        private void startButton_Click(object sender, EventArgs e)
        {
            UpdateValues();
            BlockInterface(true);
            if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
            {
                needToStop = false;
                workerThread = new Thread(new ThreadStart(FeedForwardTest));
                workerThread.Start();
            }
            else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
            {
                needToStop = false;
                workerThread = new Thread(new ThreadStart(KohonenTest));
                workerThread.Start();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            needToStop = true;
            BlockInterface(false);
            //BlockInterface(false);
        }

        private void loadProblemButton_Click(object sender, EventArgs e)
        {
			// show file selection dialog
            if (learningFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
                {
                    StreamReader reader = null;
                    int length = inputValues + outputValues;
                    double[,] tmp = new double[MaxLearningLines, length];
                    try
                    {
                        reader = File.OpenText(learningFileDialog.FileName);
                        string str = null;
                        int i = 0;

                        while ((i < MaxLearningLines) && ((str = reader.ReadLine()) != null))
                        {
                            char[] chars = { '[', ']', ';', ' ' };
                            str = str.Replace('.', ',');
                            string[] strs = str.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                            for (int ii = 0; ii < length; ++ii)
                            {
                                tmp[i, ii] = double.Parse(strs[ii]);
                            }

                            ++i;
                        }

                        trainingFFData = new double[i, length];
                        Array.Copy(tmp, 0, trainingFFData, 0, i * length);

                        teachingsLoaded = true;
                        UpdateLabels();

                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                        MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        // close file
                        if (reader != null)
                            reader.Close();
                    }
                }
                else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
                {
                    StreamReader reader = null;

                    try
                    {
                        reader = File.OpenText(learningFileDialog.FileName);
                        string str = "";
                        List<Double> list = new List<Double>();
                        List<List<Double>> list2 = new List<List<double>>();
                        list2 = new List<List<double>>();
                        string[] all = new string[] { Environment.NewLine, " ", ";", "[","]", "\n" };
                        while ((str = reader.ReadLine()) != null)
                        {
                            string[] str2 = str.Split(all,StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < str2.Length; ++i)
                            {
                                list.Add(Double.Parse(str2[i], NumberStyles.Any));
                            }
                            if (str.Contains(']'))
                            {
                                list2.Add(list);
                                list = new List<double>();
                            }
                        }
                        trainingKData = new double[list2.Count][];
                        for (int i = 0; i < trainingKData.Length; ++i)
                        {
                            trainingKData[i] = new double[list2[i].Count];
                            for (int ii = 0; ii < list2[i].Count; ++ii)
                            {
                                trainingKData[i][ii] = list2[i][ii];
                            }
                        }
                        teachingsLoaded = true;
                        UpdateLabels();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (reader != null) reader.Close();
                    }

                }
            }
        }

        

        private void parametersButton_Click(object sender, EventArgs e)
        {
            if (parametersFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                    try
                    {
                        reader = File.OpenText(parametersFileDialog.FileName);
                        string str = null;
                        int i = 0;

                        str = reader.ReadLine();
                        str = (str.Split(' '))[0];
                        if (String.Compare(str, "feedforward", true) == 0)
                        {
                            this.currentNetworkType = NeuroticProgramType.NeuroticProgramTypeFeedForward;
                            this.feedForwardRadioButton.Select();

                            str = reader.ReadLine();
                            string[] strs = str.Split(' ');
                            if (String.Compare(strs[0], "bias", true) == 0)
                            {
                                bias = true;
                            }
                            else if (String.Compare(strs[0], "nobias", true) == 0)
                            {
                                bias = false;
                            }

                            if (String.Compare(strs[1], "unipolar", true) == 0)
                            {
                                this.unipolarRadioButton.Select();
                            }
                            else if (String.Compare(strs[1], "bipolar", true) == 0)
                            {
                                this.biPolarRadioButton.Select();
                            }

                            str = reader.ReadLine();
                            str = str.Replace('.',',');
                            int index = str.IndexOf("/*");
                            if (index != -1) str = str.Substring(0, index);
                            char[] chars = { ' ' };
                            strs = str.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                            int hiddenCount = strs.Length-2;
                            hiddenLayer = new int[hiddenCount];
                            WriteLine(hiddenCount.ToString());
                            this.inputValues = int.Parse(strs[0]);
                            this.outputValues = int.Parse(strs[strs.Length - 1]);
                            for (int q = 1; q < strs.Length - 1; ++q)
                            {
                                hiddenLayer[q - 1] = int.Parse(strs[q]);
                            }

                            str = reader.ReadLine();
                            str = str.Replace('.', ',');
                            index = str.IndexOf("/*");
                            if (index != -1) str = str.Substring(0, index);
                            strs = str.Split(chars, StringSplitOptions.RemoveEmptyEntries);

                            this.numberOfCycles = int.Parse(strs[0]);
                            this.learningRate = double.Parse(strs[1]);
                            this.momentum = double.Parse(strs[2]);

                            parametersLoaded = true;

                            UpdateLabels();

                        }
                        else if (String.Compare(str, "kohonen", true) == 0)
                        {
                            this.currentNetworkType = NeuroticProgramType.NeuroticProgramTypeKohonen;
                            this.kohonenRadioButton.Select();

                            String[] line2 = reader.ReadLine().Split(' ');
                            String[] line3 = reader.ReadLine().Split(' ');

                            liczba_wejsc = Int32.Parse(line2[0]);
                            liczba_neuronow_poziom = Int32.Parse(line2[1]);
                            liczba_neuronow_pion = Int32.Parse(line2[2]);

                            numberOfCycles = Int32.Parse(line3[0]);
                            pocz_rozmiar_sasiedz = Int32.Parse(line3[1]);
                            wsp_zmian_rozm_sasiedz = Double.Parse(line3[2], CultureInfo.InvariantCulture);
                            pocz_wart_wsp_nauki = Double.Parse(line3[3], CultureInfo.InvariantCulture);
                            wps_zmiany_wsp = Double.Parse(line3[4], CultureInfo.InvariantCulture);

                            UpdateLabels();
                            parametersLoaded = true;
                        }

                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                        MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        // close file
                        if (reader != null)
                            reader.Close();
                    }
            }
            teachingDone = false;
            UpdateLabels();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (testFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
                {
                    StreamReader reader = null;

                    double[,] tmp = new double[MaxLearningLines, inputValues];
                    try
                    {
                        reader = File.OpenText(testFileDialog.FileName);
                        string str = null;
                        int i = 0;

                        while ((i < MaxLearningLines) && ((str = reader.ReadLine()) != null))
                        {
                            char[] chars = { '[', ']', ';', ' ' };
                            str = str.Replace('.', ',');
                            string[] strs = str.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                            for (int ii = 0; ii < inputValues; ++ii)
                            {
                                tmp[i, ii] = double.Parse(strs[ii]);
                            }

                            ++i;
                        }

                        testFFData = new double[i, inputValues];
                        Array.Copy(tmp, 0, testFFData, 0, i * 2);
                        testsLoaded = true;
                        UpdateLabels();

                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                        MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        // close file
                        if (reader != null)
                            reader.Close();
                    }
                }
                else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
                {
                    StreamReader reader = null;
                    try
                    {
                        reader = File.OpenText(testFileDialog.FileName);
                        string str = "";
                        List<Double> list = new List<Double>();
                        List<List<Double>> list2 = new List<List<double>>();
                        list2 = new List<List<double>>();
                        string[] all = new string[] { Environment.NewLine, " ", ";", "[", "]", "\n" };
                        while ((str = reader.ReadLine()) != null)
                        {
                            string[] str2 = str.Split(all, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < str2.Length; ++i)
                            {
                                string aString = str2[i].Replace('.', ',');
                                list.Add(Double.Parse(aString, NumberStyles.Any));
                            }
                            if (str.Contains(']'))
                            {
                                list2.Add(list);
                                list = new List<double>();
                            }
                        }
                        testKData = new double[list2.Count][];
                        for (int i = 0; i < testKData.Length; ++i)
                        {
                            testKData[i] = new double[list2[i].Count];
                            for (int ii = 0; ii < list2[i].Count; ++ii)
                            {
                                testKData[i][ii] = list2[i][ii];
                            }
                        }

                        testsLoaded = true;
                        UpdateLabels();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (reader != null) reader.Close();
                    }
                }
            }
        }

        private void teachButton_Click(object sender, EventArgs e)
        {
            UpdateValues();
            BlockInterface(true);
            if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
            {
                needToStop = false;
                workerThread = new Thread(new ThreadStart(FeedForwardTeach));
                workerThread.Start();
            }
            else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
            {
                needToStop = false;
                workerThread = new Thread(new ThreadStart(KohonenTeach));
                workerThread.Start();
            }
        }
    }

}
