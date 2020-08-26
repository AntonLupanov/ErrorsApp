using System;
using static System.Math;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.Media.Devices;
//using Windows.Networking.Vpn;
//using Windows.Security.Cryptography.Certificates;
using Windows.UI;
//using Windows.UI.Core;
//using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ErrorsApp

{
    public sealed partial class MainPage : Page
    {
        List<TextBox> textBoxesList = new List<TextBox>();
        double[] values = new double[0];
        int currentValueNumber, previousValueNumber, NumOfVal;
        public static int maxRows = 5;
        bool[] formatOfValIsCorrect = new bool[0];
        double medium, absoluteError, relativeError, sumMedium, sumAbsError, sumRelativeError;
        double p, marginOfError, roundingInterval;
        enum LanguageEnum
        {
            English,
            Ukrainian
        }
        static public byte language = (byte)LanguageEnum.English;
        static public byte theme = (byte)ElementTheme.Dark;


        public MainPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = (ElementTheme)MainPage.theme;
            language = BlankPage1.language;
            theme = BlankPage1.theme;
            ChangeLanguage((LanguageEnum)language);
            maxRows = BlankPage1.maxRows;

            maxRowsEntering.PlaceholderText = $"currently it is {maxRows}";
        }

        //Code for both error of measure and summary error pages

            //Code for menu except of restart

        private void SummaryErrorOpener_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void LanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            language = (byte)LanguageEnum.English;
            ChangeLanguage((LanguageEnum)language);
        }

        private void LanguageUkr_Click(object sender, RoutedEventArgs e)
        {
            language = (byte)LanguageEnum.Ukrainian;
            ChangeLanguage((LanguageEnum)language);
        }

        private void ChangeLanguage(LanguageEnum language)
        {

        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = (byte)ElementTheme.Dark;
            accidErrGrid.RequestedTheme = ElementTheme.Dark;
            accidErrGrid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = (byte)ElementTheme.Light;
            accidErrGrid.RequestedTheme = ElementTheme.Light;
            accidErrGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

            //Entering maximal num of rows in column

        private void maxRowsEntering_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = int.TryParse(maxRowsEntering.Text, out maxRows);
                if (!formatIsCorrect)
                {
                    maxRowsEntering.Text = "Incorrect format";
                }
                else
                {
                    if (maxRowsEntering == sender)
                    {
                        TableCreating();
                    }
                }
            }
        }

            //Incorrect format output

        private void IncorrectFormatOutput(TextBox sender)
        {
            sender.Width = language == (byte)LanguageEnum.Ukrainian ? 210 : 192;
            sender.Text = "";
            sender.PlaceholderText = language == (byte)LanguageEnum.Ukrainian ? "Невірний формат" :"Incorrect format";
        }

            //Entering values and outputing results

        private void Value_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                formatOfValIsCorrect[currentValueNumber] = double.TryParse(textBoxesList[currentValueNumber].Text, out values[currentValueNumber]);
                if (currentValueNumber < NumOfVal - 1 && formatOfValIsCorrect[currentValueNumber])
                {
                    textBoxesList[currentValueNumber + 1].AllowFocusOnInteraction = true;
                    textBoxesList[currentValueNumber + 1].Focus(FocusState.Keyboard);
                }
                else if (currentValueNumber == NumOfVal - 1 && formatOfValIsCorrect[currentValueNumber])
                {
                    ResultsOutput.Focus(FocusState.Programmatic);                    
                }
            }
        }

        private void Value_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Up && currentValueNumber > 0)
            {
                textBoxesList[currentValueNumber - 1].AllowFocusOnInteraction = true;
                textBoxesList[currentValueNumber - 1].Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Down && currentValueNumber < NumOfVal - 1)
            {
                textBoxesList[currentValueNumber + 1].AllowFocusOnInteraction = true;
                textBoxesList[currentValueNumber + 1].Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Left && currentValueNumber / maxRows > 0)
            {
                textBoxesList[currentValueNumber - maxRows].AllowFocusOnInteraction = true;
                textBoxesList[currentValueNumber - maxRows].Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Right && currentValueNumber < NumOfVal - maxRows)
            {
                textBoxesList[currentValueNumber + maxRows].AllowFocusOnInteraction = true;
                textBoxesList[currentValueNumber + maxRows].Focus(FocusState.Keyboard);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < NumOfVal; i++)
            {
                if (sender.Equals(textBoxesList[i]))
                {
                    currentValueNumber = i;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < NumOfVal; i++)
            {
                if (sender.Equals(textBoxesList[i]))
                {
                    previousValueNumber = i;
                }
            }
            
            if (!formatOfValIsCorrect[previousValueNumber])
            {
                IncorrectFormatOutput(textBoxesList[previousValueNumber]);
            }
            else
            {
                textBoxesList[previousValueNumber].Width = 100;
            }
            if (AllNumsEnteredCheck())
            {
                Output();
            }
        }

        private void Output()
        {
            if (language==(byte)LanguageEnum.English)
            {
                absoluteError = ErrorOfMeasure(values);
                relativeError = absoluteError / medium * 100;
                ResultsOutput.Text = $"Medium value: {medium}\nAbsolute error: {Round(absoluteError, 4)}\nRelative error: {Round(relativeError, 2)}%";
            }
            else if (language == (byte)LanguageEnum.Ukrainian)
            {
                absoluteError = ErrorOfMeasure(values);
                relativeError = absoluteError / medium * 100;
                ResultsOutput.Text = $"Середнє значення: {medium}\nАбсолютна похибка: {Round(absoluteError, 4)}\nВідносна похибка: {Round(relativeError, 2)}%";
            }
        }

        private bool AllNumsEnteredCheck()
        {
            bool AllNumsEnteredCheck = true;
            for (int i = 0; i < NumOfVal; i++)
            {
                if (!formatOfValIsCorrect[i])
                {
                    AllNumsEnteredCheck = false;
                }
            }
            return AllNumsEnteredCheck;
        }


        //Code for accidental error of measure page

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            NumOfValuesEntering.Text = "";
        }

        public void Restart()
        {
            EnterValuesBlock.Visibility = Visibility.Collapsed;
            for (int i = 0; i < textBoxesList.Count; i++)
            {
                accidErrGrid.Children.Remove(textBoxesList[i]);
            }
            for (int i = 0; i < formatOfValIsCorrect.Length; i++)
            {
                formatOfValIsCorrect[i] = false;
            }
            textBoxesList.Clear();
            ResultsOutput.Text = "";
            NumOfValuesEntering.AllowFocusOnInteraction = true;
            NumOfValuesEntering.Focus(FocusState.Keyboard);
        }

        private void TableCreating()
        {
            Restart();
            EnterValuesBlock.Visibility = Visibility.Visible;
            for (int i = 0; i < NumOfVal; i++)
            {
                textBoxesList.Add(new TextBox
                {
                    Height = 50,
                    Width = 100,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Visible,
                    Margin = new Thickness
                    {
                        Left = NumOfValuesEntering.Margin.Left + i / maxRows * 210,
                        Top = EnterValuesBlock.Margin.Top + EnterValuesBlock.Height + i % maxRows * 50,
                    },
                    FontSize = NumOfValuesEntering.FontSize,
                    TextAlignment = NumOfValuesEntering.TextAlignment,

                });

                textBoxesList[i].KeyUp += Value_KeyUp;
                textBoxesList[i].KeyDown += Value_KeyDown;
                textBoxesList[i].GotFocus += TextBox_GotFocus;
                textBoxesList[i].LostFocus += TextBox_LostFocus;

                accidErrGrid.Children.Add(textBoxesList[i]);
            }
            textBoxesList[0].AllowFocusOnInteraction = true;
            textBoxesList[0].Focus(FocusState.Keyboard);
        }

        private void NumOfValEnt_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = int.TryParse(NumOfValuesEntering.Text, out NumOfVal);
                if (!formatIsCorrect)
                {
                    IncorrectFormatOutput(NumOfValuesEntering);
                }
                else
                {
                    Array.Resize(ref values, NumOfVal);
                    Array.Resize(ref formatOfValIsCorrect, NumOfVal);
                    ConfProbEntering.AllowFocusOnInteraction = true;
                    ConfProbEntering.Focus(FocusState.Keyboard);
                }
            }
        }

        private void NumOfValEnt_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Right)
            {
                ConfProbEntering.AllowFocusOnInteraction = true;
                ConfProbEntering.Focus(FocusState.Keyboard);
            }
        }

        private void ConfProb_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ResultsOutput.AllowFocusOnInteraction = true;
                ResultsOutput.Focus(FocusState.Programmatic);
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                NumOfValuesEntering.AllowFocusOnInteraction = true;
                NumOfValuesEntering.Focus(FocusState.Keyboard);
            }
        }

        private void ConfProb_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = double.TryParse(ConfProbEntering.Text, out p);
            ConfProbEntering.Width = 100;
            if (!formatIsCorrect)
            {
                IncorrectFormatOutput(ConfProbEntering);
            }

            if (NumOfValuesEntering.Text != "" && ConfProbEntering.Text != "")
            {
                TableCreating();
            }
        }

        private double ErrorOfMeasure(double[] values)
        {
            medium = 0;
            for (int i = 0; i < NumOfVal; i++)
            {
                medium += values[i];
            }
            medium /= NumOfVal;

            if (values.Length == 1)
            {
                return 0;
            }

            double ErrorOfMeasure = 0;
            for (int i = 0; i < NumOfVal; i++)
            {
                ErrorOfMeasure += (values[i] - medium) * (values[i] - medium);
            }
            ErrorOfMeasure = Tpn(p, NumOfVal) * Sqrt(ErrorOfMeasure / (NumOfVal * NumOfVal - NumOfVal));
                        
            return ErrorOfMeasure;
        }

        private double Tpn(double p, int n)
        {
            var tableOfTpn = new double[,]
            {//  n  |  confidence probability
                {0,   0.5,   0.8,  0.9,   0.95,  0.975,  0.99,  0.999  },
                {1,   0,     0,    0,     0,     0,      0,     0      },
                {2,   1,     3.1,  6.31,  12.71, 31.82,  63.66, 636.62 },
                {3,   0.82,  1.9,  2.92,  4.3,   6.97,   9.93,  31.6   },
                {4,   0.77,  1.6,  2.35,  3.18,  4.54,   5.84,  12.92 },
                {5,   0.74,  1.5,  2.13,  2.78,  3.75,   4.6,   8.61 },
                {6,   0.73,  1.5,  2.02,  2.57,  3.37,   4.03,  6.87 },
                {7,   0.72,  1.4,  1.94,  2.45,  3.14,   3.71,  5.96 },
                {8,   0.71,  1.4,  1.90,  2.37,  3.00,   3.50,  5.41 },
                {9,   0.71,  1.4,  1.86,  2.31,  2.90,   3.36,  5.04 },
                {10,  0.7,   1.4,  1.83,  2.26,  2.82,   3.25,  4.78 },
                {11,  0.7,   1.4,  1.81,  2.23,  2.76,   3.17,  4.59 },
                {12,  0.7,   1.4,  1.80,  2.20,  2.72,   3.11,  4.44 },
                {13,  0.7,   1.4,  1.78,  2.18,  2.68,   3.06,  4.32 },
                {14,  0.69,  1.4,  1.77,  2.16,  2.65,   3.01,  4.22 },
                {15,  0.69,  1.3,  1.76,  2.15,  2.62,   2.98,  4.14 },
                {16,  0.69,  1.3,  1.75,  2.13,  2.60,   2.95,  4.07 },
                {17,  0.69,  1.3,  1.75,  2.12,  2.58,   2.92,  4.02 },
                {18,  0.69,  1.3,  1.74,  2.11,  2.57,   2.90,  3.97 },
                {19,  0.69,  1.3,  1.73,  2.10,  2.55,   2.88,  3.92 },
                {20,  0.69,  1.3,  1.73,  2.09,  2.54,   2.86,  3.88 },
                {25,  0.69,  1.3,  1.71,  2.06,  2.49,   2.80,  3.75 },
                {30,  0.68,  1.3,  1.70,  2.04,  2.46,   2.76,  3.66 },
                {40,  0.68,  1.3,  1.68,  2.02,  2.42,   2.70,  3.55 },
                {60,  0.68,  1.3,  1.67,  2.00,  2.39,   2.66,  3.46 },
                {120, 0.68,  1.3,  1.66,  1.98,  2.36,   2.62,  3.37 },
    {double.MaxValue, 0.67,  1.3,  1.65,  1.96,  2.33,   2.58,  3.29 }
            };
            for (int i = 0; i < tableOfTpn.GetLength(0); i++)
            {
                for (int j = 0; j < tableOfTpn.GetLength(1); j++)
                {
                    if (p <= tableOfTpn[0,j] && n <= tableOfTpn[i,0])
                    {
                        return tableOfTpn[i, j];
                    }
                }
            }
            return 0;
        }


        
    }
}
