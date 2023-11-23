using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace Project_1
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(); 
        SpeechSynthesizer sarah = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
        DateTime TimeNow = DateTime.Now;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefalutCommands.txt")))));
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(recognizer_SpeechRecognized);
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefalutCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
        }


        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if (speech == "")
            {
                sarah.SpeakAsyncCancelAll();
            }
            if (speech == "Hello")
            {
                sarah.SpeakAsync("Hello, I am here");
            }
            if (speech == "Hii")
            {
                sarah.SpeakAsync("Hello, Master ");
            }
            if (speech == "How are you")
            {
                sarah.SpeakAsync("I am working normally");
            }
            if (speech == "What time is it")
            {
                sarah.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }
            if (speech == "Stop Talking")
            {
                sarah.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1);
                if (ranNum == 1)
                {
                    sarah.SpeakAsync("Yes Sir");
                }
                
            }
            if (speech == "Stop Listening")
            {
                sarah.SpeakAsync("if you need me just  ask");
                recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }

            if (speech == "Show Commands")
            {
                string[] commands = (File.ReadAllLines(@"DefalutCommands.txt"));
                LstCommands.Items.Clear();
                LstCommands.SelectionMode = SelectionMode.None;
                LstCommands.Visible = true;
                foreach (string command in commands)
                {
                    LstCommands.Items.Add(command);
                }
            }

            if (speech == "Hide Commands")
            {
                LstCommands.Visible =false;
            }
        }
        private void recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;
        }
        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;  

            if(speech == "Wake up")
            {
                startlistening.RecognizeAsyncCancel();
                sarah.SpeakAsync("Yes, I am here ");
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(RecTimeOut== 10)
            {
                recognizer.RecognizeAsyncCancel();
            }
            else if(RecTimeOut== 11)
            {
                TmrSpecking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut= 0;
            }
        }
    }
}
