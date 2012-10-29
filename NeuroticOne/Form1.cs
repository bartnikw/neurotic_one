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

        //private double[,] teachingKData = null;
        //private double[,] points = null;// new int[pointsCount, 2];	// x, y
        private double[][][] trainingKData; //= new double[pointsCount][];


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
            this.learningRateTextBox.Text = learningRate.ToString();
            this.momentumTextBox.Text = momentum.ToString();
            this.inputNumberTextBox.Text = inputValues.ToString();
            this.outputNumberTextBox.Text = outputValues.ToString();
            this.cycleCountTextBox.Text = numberOfCycles.ToString();
            this.startButton.Enabled = parametersLoaded && teachingsLoaded && testsLoaded;
            if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
            {
                this.kohonenRadioButton.Checked = true;
            }
            else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
            {
                this.feedForwardRadioButton.Checked = true;
            }
            if (this.hiddenLayer!= null) this.layersNumberTextBox.Text = hiddenLayer.Length.ToString();
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

        void FeedForward()
        {
            int samples = trainingFFData.GetLength(0);

            //int hiddenlayers = 1;
            //int outputCount = 1;
            //int cycles = 1000;
           
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
            ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(), inputValues, newHidden);
            
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;
            SetIterationsCount(0);
            int iteration = 1;

            //double[,] solution = new double[50, outputValues];
            double[] networkInput = new double[inputValues];

            while (!needToStop)
            {
                double error = teacher.RunEpoch(input, output);

                SetIterationsCount(iteration);
                if (++iteration > numberOfCycles) break;
            }

            WriteLine("Test:");
            for (int j = 0; !needToStop && j < this.testFFData.GetLength(0); ++j)
            {
                Write("[");
                for (int jj = 0; jj < inputValues; ++jj)
                {
                    networkInput[jj] = this.testFFData[j, jj];
                    Write(String.Concat(" ", networkInput[jj].ToString()));
                }
                double[] result = network.Compute(networkInput);
                Write(" ] -> [");
                for (int jj = 0; jj < outputValues; ++jj)
                {
                    Write(string.Concat(" ", Math.Round(result[jj],5).ToString()));
                }
                WriteLine(" ];");
            }
            BlockInterface(false);
            needToStop = false;
        }

        void FeedForwardWithoutBias()
        {
            int samples = trainingFFData.GetLength(0);

            //int hiddenlayers = 1;
            //int outputCount = 1;
            //int cycles = 1000;

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
                    output[i][q - inputValues] = trainingFFData[i, q];
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
            int[] newHidden = new int[hiddenLayer.Length + 2];
            Array.Copy(hiddenLayer, 0, newHidden, 1, hiddenLayer.Length);
            newHidden[newHidden.Length - 1] = outputValues;
            newHidden[0] = inputValues;
            ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(), inputValues, newHidden);

            BackPropagationLearningWithoutBias teacher = new BackPropagationLearningWithoutBias(network);
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;
            SetIterationsCount(0);
            int iteration = 1;

            //double[,] solution = new double[50, outputValues];
            double[] networkInput = new double[inputValues];

            while (!needToStop)
            {
                double error = teacher.RunEpoch(input, output);

                SetIterationsCount(iteration);
                if (++iteration > numberOfCycles) break;
            }

            WriteLine("Test:");
            for (int j = 0; !needToStop && j < this.testFFData.GetLength(0); ++j)
            {
                Write("[");
                for (int jj = 0; jj < inputValues; ++jj)
                {
                    networkInput[jj] = this.testFFData[j, jj];
                    Write(String.Concat(" ", networkInput[jj].ToString()));
                }
                double[] result = network.ComputeWithoutBias(networkInput);
                Write(" ] -> [");
                for (int jj = 0; jj < outputValues; ++jj)
                {
                    Write(string.Concat(" ", result[jj].ToString()));
                }
                WriteLine(" ];");
            }
            BlockInterface(false);
            needToStop = false;
        }

        void Kohonen()
        {
            //Kohonen is a distance network

            //int inputCount = -1;//to get
            //int neuronCountH = -1;//get
            //int neuronCountV = -1;//get

            Neuron.RandRange = new Range(0.0f, 2.0f);//new DoubleRange(0.0, 2.0);
            DistanceNetwork network = new DistanceNetwork(2, liczba_neuronow_pion * liczba_neuronow_poziom);

            SOMLearning teacher = new SOMLearning(network);

            /*foreach (Neuron n in network.Layers[0].Neurons)
            {
                this.WriteLine(String.Format("I am {0}", n ToString()));
                //for (int i = 0; i < n.Weights.Length; ++i)
                //{
                //    this.Write(String.Format("[{0}] = {1}\t", i, n.Weights[i]));
                //}
                //WriteLine("");
            }
             */

            double driftingLearningRate = this.wps_zmiany_wsp;
            double fixedLearningRate = this.pocz_wart_wsp_nauki;
            double learningRadius = this.pocz_rozmiar_sasiedz;
            double driftingLearningRadius = this.wsp_zmian_rozm_sasiedz;
            int iteration = 1;
            while (!needToStop)
            {
                teacher.LearningRate = driftingLearningRate * (numberOfCycles - iteration) / numberOfCycles + fixedLearningRate;
                teacher.LearningRadius = (double)learningRadius * (numberOfCycles - iteration) / numberOfCycles;

                for (int i = 0; i < trainingKData.GetLength(0); ++i)
                {
                    teacher.RunEpoch(trainingKData[i]);
                    //Console.WriteLine("klik {0}", i);
                }


                SetIterationsCount(iteration++);
                if (iteration > numberOfCycles) break;
            }

            //Console.WriteLine(network.Layers.Length);
            double[][] testNetwork = new double[9][];
            for (int i = 0; i < 9; ++i)
            {
                testNetwork[i] = new double[2];
                testNetwork[i][0] = i / 3;
                testNetwork[i][1] = i % 3;
            }

            //double[] output = network.Compute(testNetwork);
            //for (Neuron n in network.Layers[0].Neurons
            network.GetWinner();
            foreach (Neuron n in network.Layers[0].Neurons)
            {
                this.WriteLine(String.Format("I am {0}", n.ToString()));
                for (int i = 0; i < n.Weights.Length; ++i)
                {
                    this.Write(String.Format("[{0}] = {1}\t", i, n.Weights[i]));
                }
                WriteLine("");
            }
            this.Write("Jest ok");
        }

        // Worker thread
        /*void SearchSolution()
        {
            // number of learning samples
            int samples; //= data.GetLength(0);
            // data transformation factor
            double yFactor;// = 1.7 / chart.RangeY.Length;
            double yMin;// = chart.RangeY.Min;
            double xFactor;// = 2.0 / chart.RangeX.Length;
            double xMin;// = chart.RangeX.Min;

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (data[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (data[i, 1] - yMin) * yFactor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                1, neuronsInFirstLayer, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations
            int iteration = 1;

            // solution array
            double[,] solution = new double[50, 2];
            double[] networkInput = new double[1];

            // calculate X values to be used with solution function
            for (int j = 0; j < 50; j++)
            {
                solution[j, 0] = chart.RangeX.Min + (double)j * chart.RangeX.Length / 49;
            }

            // loop
            while (!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (int j = 0; j < 50; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                chart.UpdateDataSeries("solution", solution);
                // calculate error
                double learningError = 0.0;
                for (int j = 0, k = data.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(data[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                // set current iteration's info
                currentIterationBox.Text = iteration.ToString();
                currentErrorBox.Text = learningError.ToString("F3");

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }


            // enable settings controls
            EnableControls(true);
        }*/

        private void startButton_Click(object sender, EventArgs e)
        {
            UpdateValues();
            BlockInterface(true);
            if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeFeedForward)
            {
                needToStop = false;
                if(bias)
                    workerThread = new Thread(new ThreadStart(FeedForward));
                else
                    workerThread = new Thread(new ThreadStart(FeedForwardWithoutBias));
                workerThread.Start();
            }
            else if (this.currentNetworkType == NeuroticProgramType.NeuroticProgramTypeKohonen)
            {
                needToStop = false;
                workerThread = new Thread(new ThreadStart(Kohonen));
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
                        string str = null;
                        string[] strs = null;
                        //int i = 0;
                        str = reader.ReadToEnd();
                        char[] chars = new char[] { ']' };
                        strs = str.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                        int numberOfExamples = strs.Length;
                        trainingKData = new double[numberOfExamples][][];
                        //chars = new char[] { ']', '[' };
                        
                        for (int i = 0; i < numberOfExamples; ++i)
                        {
                            string[] newLine = new string[] {Environment.NewLine};
                            string[] all = new string[] {Environment.NewLine, " ", ";", "["};
                            string[] strs2 = strs[i].Split(newLine, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(strs[i]);
                            int dimensions = strs2.Length;
                            Console.WriteLine(dimensions.ToString());
                            Console.WriteLine(numberOfExamples.ToString());
                            strs2 = strs[i].Split(all, StringSplitOptions.RemoveEmptyEntries);
                            double[][] pom = new double[strs2.Length][];
                            int q = 0;
                            for (int ii = 0; ii < strs2.Length; ++ii)
                            {
                                int value = int.Parse(strs2[ii]);
                                if (value > 0)
                                {
                                    pom[q] = new double[2];
                                    pom[q][0] = ii / dimensions;
                                    pom[q][1] = ii % dimensions;
                                    q++;
                                }
                            }
                            trainingKData[i] = new double[q][];
                            for (int ii = 0; ii < q; ++ii)
                            {

                                trainingKData[i][ii] = new double[2];
                                trainingKData[i][ii][0] = pom[ii][0];
                                trainingKData[i][ii][1] = pom[ii][1];
                            }

                        }
                        teachingsLoaded = true;
                        for(int i=0; i<numberOfExamples; i++)
                        {
                            for (int j = 0; j < trainingKData[0].Length; j++)
                            {
                                for(int k=0; k<trainingKData[0][0].Length; k++)
                                Console.Write(trainingKData[i][j][k].ToString());
                            }
                            Console.WriteLine();
                            }

                        UpdateLabels();
                    }
                    catch
                    {
                    }
                    finally
                    {
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
        }

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
                    //blabla bla
                    testsLoaded = true;
                    UpdateLabels();
                }
            }
        }
    }

}
