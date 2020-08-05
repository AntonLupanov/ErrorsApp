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
//using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ErrorsApp

{
    public sealed partial class MainPage : Page
    {
        List<TextBox> textBoxesList = new List<TextBox>();
        double[] values = new double[0];
        int currentValueNumber, previousValueNumber, NumOfVal, maxRows = 5;
        bool[] formatOfValIsCorrect = new bool[0];
        double medium, absoluteError, relativeError, sumMedium, sumAbsError, sumRelativeError;
        bool menuOpened = false;
        double p, marginOfError, roundingInterval;

        ListView menu;
        Button errorOfMeasureOpener;
        Button summaryErrorOpener;
        //TextBox maxRowsEntering;
        Button menuCloser;



        public MainPage()
        {
            this.InitializeComponent();

            maxRowsEntering.PlaceholderText = $"currently it is {maxRows}";
            maxRowsEntering_SummEr.PlaceholderText = $"currently it is {maxRows}";

            menu = new ListView
            {
                Width = 300,
                Height = 250,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness
                {
                    Left = 10,
                    Top = 50
                },
                Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50)),

            };

            errorOfMeasureOpener = new Button
            {
                Width = menu.Width,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness
                {
                    Left = 0,
                    Top = 10
                },
                FontSize = 18,
                Content = "Accidental error of measure",
                Opacity = 1,
            };
            errorOfMeasureOpener.Click += ErrorOfMeasureOpener_Click;
            menu.Items.Add(errorOfMeasureOpener);

            summaryErrorOpener = new Button
            {
                Width = menu.Width,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness
                {
                    Left = 0,
                    Top = 10
                },
                FontSize = 18,
                Content = "Summary Error",
                Opacity = 1,
            };
            summaryErrorOpener.Click += SummaryErrorOpener_Click;
            menu.Items.Add(summaryErrorOpener);

            //maxRowsEntering = new TextBox
            //{
            //    Width = 250,
            //    Height = 100,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Margin = new Thickness
            //    {
            //        Left = 0,
            //        Top = 10
            //    },
            //    FontSize = 18,
            //    Header = "Maximal number of rows in coloumn",
            //    PlaceholderText = $"currently it is {maxRows}"
            //};
            //maxRowsEntering.KeyUp += maxRowsEntering_KeyUp;
            //menu.Items.Add(maxRowsEntering);

            menuCloser = new Button
            {
                Width = menu.Width,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness
                {
                    Left = 0,
                    Top = 10
                },
                FontSize = 18,
                Content = "Close Menu",
                Opacity = 1,
            };
            menuCloser.Click += CloseMenu_Click;
            menu.Items.Add(menuCloser);
        }

        //Code for menu
        
        private void Menu_Click(object sender, RoutedEventArgs e)
        {

            if (menuOpened)
            {
                accidErrGrid.Children.Remove(menu);
                menuOpened = false;
            }
            else
            {
                accidErrGrid.Children.Add(menu);
                menuOpened = true;
            }
        }

        private void ErrorOfMeasureOpener_Click(object sender, RoutedEventArgs e)
        {
            summaryErrorGrid.Visibility = Visibility.Collapsed;
            summaryErrorScroll.Visibility = Visibility.Collapsed;
        }

        private void SummaryErrorOpener_Click(object sender, RoutedEventArgs e)
        {
            summaryErrorScroll.Visibility = Visibility.Visible;
            summaryErrorGrid.Visibility = Visibility.Visible;
        }

        private void CloseMenu_Click(object sender, RoutedEventArgs e)
        {
            accidErrGrid.Children.Remove(menu);
            menuOpened = false;
        }

        //Code for error of measure page

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            NumOfValuesEntering.Text = "";
        }

        private void NumOfValEnt_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = int.TryParse(NumOfValuesEntering.Text, out NumOfVal);
                if (!formatIsCorrect)
                {
                    NumOfValuesEntering.Width = 192;
                    NumOfValuesEntering.Text = "";
                    NumOfValuesEntering.PlaceholderText = "Incorrect format";
                }
                else
                {
                    Array.Resize(ref values, NumOfVal);
                    Array.Resize(ref formatOfValIsCorrect, NumOfVal);
                    TableCreating();
                }
            }
        }

        private void NumOfValEnt_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down)
            {
                textBoxesList[0].AllowFocusOnInteraction = true;
                textBoxesList[0].Focus(FocusState.Keyboard);
            }
        }

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
                        maxRowsEntering_SummEr.Text = "";
                        maxRowsEntering_SummEr.PlaceholderText = $"currently it is {maxRows}";
                        TableCreating();
                    }
                    if (maxRowsEntering_SummEr == sender)
                    {
                        maxRowsEntering.Text = "";
                        maxRowsEntering.PlaceholderText = $"currently it is {maxRows}";
                        TableCreating_SummEr();
                    }
                }
            }
        }

        private void Restart()
        {
            EnterValuesBlock.Visibility = Visibility.Collapsed;
            for (int i = 0; i < textBoxesList.Count; i++)
            {
                accidErrGrid.Children.Remove(textBoxesList[i]);
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
                    Height = NumOfValuesEntering.Height,
                    Width = NumOfValuesEntering.Width,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Visible,
                    Margin = new Thickness
                    {
                        Left = NumOfValuesEntering.Margin.Left + i / maxRows * (10 + NumOfValuesEntering.Width),
                        Top = EnterValuesBlock.Margin.Top + EnterValuesBlock.Height + i % maxRows * NumOfValuesEntering.Height,
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

        private double ErrorOfMeasure(double[] values)
        {
            medium = 0;
            for (int i = 0; i < NumOfVal; i++)
            {
                medium += values[i];
            }
            medium /= NumOfVal;

            double ErrorOfMeasure = 0;
            for (int i = 0; i < NumOfVal; i++)
            {
                ErrorOfMeasure += (values[i] - medium) * (values[i] - medium);
            }
            ErrorOfMeasure = Tpn(p, NumOfVal) * Math.Sqrt(ErrorOfMeasure / (NumOfVal * NumOfVal - NumOfVal));

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


        //Code for summary error page

        private void NumOfValEntSummEr_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = int.TryParse(NumOfValuesEntering_SummEr.Text, out NumOfVal);
                if (!formatIsCorrect)
                {
                    NumOfValuesEntering_SummEr.Width = 192;
                    NumOfValuesEntering_SummEr.Text = "";
                    NumOfValuesEntering_SummEr.PlaceholderText = "Incorrect format";
                }
                else
                {
                    Array.Resize(ref values, NumOfVal);
                    Array.Resize(ref formatOfValIsCorrect, NumOfVal);
                    confProbEntering.AllowFocusOnInteraction = true;
                    confProbEntering.Focus(FocusState.Keyboard);
                }
                if (AllParamsEntered())
                {
                    TableCreating_SummEr();
                }
            }
            if (e.Key == Windows.System.VirtualKey.Right)
            {
                confProbEntering.AllowFocusOnInteraction = true;
                confProbEntering.Focus(FocusState.Keyboard);
            }
        }

        private void NumOfValEntSummEr_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void confProb_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = double.TryParse(confProbEntering.Text, out p);
                if (!formatIsCorrect)
                {
                    NumOfValuesEntering_SummEr.Width = 192;
                    NumOfValuesEntering_SummEr.Text = "";
                    NumOfValuesEntering_SummEr.PlaceholderText = "Incorrect format";
                }
                else
                {                    
                    marginErrorEntering.AllowFocusOnInteraction = true;
                    marginErrorEntering.Focus(FocusState.Keyboard);
                }

                if (AllParamsEntered())
                {
                    TableCreating_SummEr();
                }
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                NumOfValuesEntering_SummEr.AllowFocusOnInteraction = true;
                NumOfValuesEntering_SummEr.Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Right)
            {
                marginErrorEntering.AllowFocusOnInteraction = true;
                marginErrorEntering.Focus(FocusState.Keyboard);
            }
        }

        private void marginError_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = double.TryParse(marginErrorEntering.Text, out marginOfError);
                if (!formatIsCorrect)
                {
                    marginErrorEntering.Width = 192;
                    marginErrorEntering.Text = "";
                    marginErrorEntering.PlaceholderText = "Incorrect format";
                }
                else
                {
                    roundingIntEntering.AllowFocusOnInteraction = true;
                    roundingIntEntering.Focus(FocusState.Keyboard);
                }

                if (AllParamsEntered())
                {
                    TableCreating_SummEr();
                }
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                confProbEntering.AllowFocusOnInteraction = true;
                confProbEntering.Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Right)
            {
                roundingIntEntering.AllowFocusOnInteraction = true;
                roundingIntEntering.Focus(FocusState.Keyboard);
            }
        }

        private void roundingInt_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                bool formatIsCorrect = double.TryParse(roundingIntEntering.Text, out roundingInterval);
                if (!formatIsCorrect)
                {
                    roundingIntEntering.Width = 192;
                    roundingIntEntering.Text = "";
                    roundingIntEntering.PlaceholderText = "Incorrect format";
                }
                if (AllParamsEntered())
                {
                    TableCreating_SummEr();
                }
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                marginErrorEntering.AllowFocusOnInteraction = true;
                marginErrorEntering.Focus(FocusState.Keyboard);
            }
        }

        //private void maxRowsEnteringSummEr_KeyUp(object sender, KeyRoutedEventArgs e)
        //{
        //    bool formatIsCorrect = int.TryParse(maxRowsEntering_SummEr.Text, out maxRows);
        //    if (!formatIsCorrect && e.Key != Windows.System.VirtualKey.Back && e.Key != Windows.System.VirtualKey.Delete)
        //    {
        //        maxRowsEntering_SummEr.Text = "Incorrect format";
        //    }
        //}

        private bool AllParamsEntered()
        {
            if(NumOfValuesEntering_SummEr.Text!="" && confProbEntering.Text!="" && marginErrorEntering.Text!="" && roundingIntEntering.Text != "")
            {
                return true;
            }
                return false;
        }

        private void TableCreating_SummEr()
        {
            Restart_SummEr();
            EnterValuesBlock_SummEr.Visibility = Visibility.Visible;
            for (int i = 0; i < NumOfVal; i++)
            {
                textBoxesList.Add(new TextBox
                {
                    Height = NumOfValuesEntering_SummEr.Height,
                    Width = NumOfValuesEntering_SummEr.Width,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Visible,
                    Margin = new Thickness
                    {
                        Left = NumOfValuesEntering_SummEr.Margin.Left + i / maxRows * (10 + NumOfValuesEntering_SummEr.Width),
                        Top = EnterValuesBlock_SummEr.Margin.Top + EnterValuesBlock_SummEr.Height + i % maxRows * NumOfValuesEntering_SummEr.Height,
                    },
                    FontSize = NumOfValuesEntering.FontSize,
                    TextAlignment = NumOfValuesEntering.TextAlignment,

                });

                textBoxesList[i].KeyUp += Value_KeyUp;
                textBoxesList[i].KeyDown += Value_KeyDown;
                textBoxesList[i].GotFocus += TextBox_GotFocus;
                textBoxesList[i].LostFocus += TextBox_LostFocus;

                summaryErrorGrid.Children.Add(textBoxesList[i]);
            }
            textBoxesList[0].AllowFocusOnInteraction = true;
            textBoxesList[0].Focus(FocusState.Keyboard);
        }

        private void Restart_SummEr()
        {
            EnterValuesBlock_SummEr.Visibility = Visibility.Collapsed;
            for (int i = 0; i < textBoxesList.Count; i++)
            {
                summaryErrorGrid.Children.Remove(textBoxesList[i]);
            }
            textBoxesList.Clear();
            summaryErrorOutput.Text = "";
        }

        private void SummaryErrorCalculating(double[] values)
        {
            sumMedium = 0;
            for (int i = 0; i < NumOfVal; i++)
            {
                sumMedium += values[i];
            }
            sumMedium /= NumOfVal;

            double errorOfInstrument = Tp(p) * marginOfError / 3;
            double errorOfRounding = p * roundingInterval / 2;

            sumAbsError = Sqrt(Pow(ErrorOfMeasure(values), 2) + Pow(errorOfInstrument, 2) + Pow(errorOfRounding, 2));

            sumRelativeError = sumAbsError / sumMedium * 100;
        }

        private double Tp(double p)
        {
            if (p < 0.07966) { return 0.1; }
            if (p < 0.38292) { return 0.5; }
            if (p < 0.51607) { return 0.7; }
            if (p < 0.68269) { return 1; }
            if (p < 0.80640) { return 1.3; }

            if (p < 0.89040) { return 1.6; }
            if (p < 0.95450) { return 2.0; }
            if (p < 0.97219) { return 2.2; }
            if (p < 0.98360) { return 2.4; }
            if (p < 0.99068) { return 2.6; }

            if (p < 0.99489) { return 2.8; }
            if (p < 0.99730) { return 3.0; }
            if (p < 0.99953) { return 3.5; }
            if (p < 0.99994) { return 4.0; }
            if (p < 0.999994) { return 4.5; }
            else { return 5; }
        }

        //Code for entering values and output results

        private void Value_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (currentValueNumber < NumOfVal - 1 && formatOfValIsCorrect[currentValueNumber])
                {
                    textBoxesList[currentValueNumber + 1].AllowFocusOnInteraction = true;
                    textBoxesList[currentValueNumber + 1].Focus(FocusState.Keyboard);
                }
                else if (currentValueNumber == NumOfVal - 1 && formatOfValIsCorrect[currentValueNumber])
                {
                    TextBox_LostFocus(sender, e);
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
            formatOfValIsCorrect[previousValueNumber] = double.TryParse(textBoxesList[previousValueNumber].Text, out values[previousValueNumber]);
            if (!formatOfValIsCorrect[previousValueNumber])
            {
                textBoxesList[previousValueNumber].Width = 192;
                textBoxesList[previousValueNumber].Text = "";
                textBoxesList[previousValueNumber].PlaceholderText = "Incorrect format";
            }
            if (AllNumsEnteredCheck())
            {
                if (summaryErrorGrid.Visibility == Visibility.Collapsed)
                {
                    absoluteError = ErrorOfMeasure(values);
                    relativeError = absoluteError / medium * 100;
                    ResultsOutput.Text = $"Medium value: {medium}\nAbsolute error: {absoluteError}\nRelative error: {Round(relativeError, 2)}%";
                }
                else
                {
                    SummaryErrorCalculating(values);
                    summaryErrorOutput.Text = $"Medium value: {sumMedium}\nAbsolute error: {sumAbsError}\nRelative error: {Round(sumRelativeError, 2)}% ";
                }
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
    }
}
