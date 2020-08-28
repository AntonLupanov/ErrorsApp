using System;
using static System.Math;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ErrorsApp
{
    public sealed partial class BlankPage1 : Page
    {
        List<TextBox> textBoxesList = new List<TextBox>();
        double[] values = new double[0];
        int currentValueNumber, previousValueNumber, NumOfVal;
        public static int maxRows = 5;
        bool[] formatOfValIsCorrect = new bool[0];
        double medium, sumMedium, sumAbsError, sumRelativeError;
        double p, marginOfError, roundingInterval;
        enum LanguageEnum
        {
            English,
            Ukrainian
        }
        public static byte language;
        public static byte theme;


        public BlankPage1()
        {
            this.InitializeComponent();
            this.RequestedTheme = (ElementTheme)MainPage.theme;
            language = MainPage.language;
            theme = MainPage.theme;
            ChangeLanguage((LanguageEnum)language);
            maxRows = MainPage.maxRows;

            maxRowsEntering_SummEr.PlaceholderText = $"currently it is {maxRows}";
        }
                
        //Code for menu

        private void ErrorOfMeasureOpener_Click(object sender, RoutedEventArgs e)
        {
            Restart_SummEr();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void RestartSummEr_Click(object sender, RoutedEventArgs e)
        {
            Restart_SummEr();
            NumOfValuesEntering_SummEr.Text = "";
            confProbEnteringSE.Text = "";
            marginErrorEntering.Text = "";
            roundingIntEntering.Text = "";
        }

        private void Restart_SummEr()
        {
            EnterValuesBlock_SummEr.Visibility = Visibility.Collapsed;
            for (int i = 0; i < textBoxesList.Count; i++)
            {
                summaryErrorGrid.Children.Remove(textBoxesList[i]);
                formatOfValIsCorrect[i] = false;
            }
            textBoxesList.Clear();
            summaryErrorOutput.Text = "";
        }

        private void LanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            language = (byte)LanguageEnum.English;
            ChangeLanguage(LanguageEnum.English);
        }

        private void LanguageUkr_Click(object sender, RoutedEventArgs e)
        {
            language = (byte)LanguageEnum.Ukrainian;
            ChangeLanguage(LanguageEnum.Ukrainian);
        }

        private void ChangeLanguage(LanguageEnum language)
        {
            if (language == LanguageEnum.Ukrainian)
            {
                pageChanger_SummEr.Content = "Сторінка";
                ErrorOfMeasureOpener.Text = "Похибка вимірювання";
                RestartButton_SummEr.Content = "Заново";
                languageChanger_SummEr.Content = "Мова";
                languageEng1.Text = "Англійська";
                languageUkr1.Text = "Українська";
                themeChanger_SummEr.Content = "Тема";
                themeDark1.Text = "Темна";
                themeLight1.Text = "Світла";
                EnterNumberBlock_SummEr.Text = "Введіть кількість вимірювань:";
                confProbBlockSE.Text = "Довірча ймовірність:";
                marginErrorBlock.Text = "Гранична похибка:";
                roundingIntBlock.Text = "Інтервал заокруглення:";
                maxRowsEntering_SummEr.Header = "Максимальна кількість рядків в колонці";
                maxRowsEntering_SummEr.PlaceholderText = $"зараз це {maxRows}";
                EnterValuesBlock_SummEr.Text = "Введіть вимірювання";
            }
            if (language == LanguageEnum.English)
            {
                pageChanger_SummEr.Content = "Page";
                ErrorOfMeasureOpener.Text = "Error of measure";
                RestartButton_SummEr.Content = "Restart";
                languageChanger_SummEr.Content = "Language";
                languageEng1.Text = "English";
                languageUkr1.Text = "Ukrainian";
                themeChanger_SummEr.Content = "Theme";
                themeDark1.Text = "Dark";
                themeLight1.Text = "Light";
                EnterNumberBlock_SummEr.Text = "Enter number of values:";
                confProbBlockSE.Text = "Confidence probability:";
                marginErrorBlock.Text = "Margin error:";
                roundingIntBlock.Text = "Rounding interval:";
                maxRowsEntering_SummEr.Header = "Maximal number of rows in column";
                maxRowsEntering_SummEr.PlaceholderText = $"currently it is {maxRows}";
                EnterValuesBlock_SummEr.Text = "Enter values";
            }
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = (byte)ElementTheme.Dark;
            SummaryErrorPage.RequestedTheme = ElementTheme.Dark;
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = (byte)ElementTheme.Light;
            SummaryErrorPage.RequestedTheme = ElementTheme.Light;
        }

        //Incorrect format output

        private void IncorrectFormatOutput(TextBox sender)
        {
            sender.Width = language == (byte)LanguageEnum.Ukrainian ? 210 : 192;
            sender.Text = "";
            sender.PlaceholderText = language == (byte)LanguageEnum.Ukrainian ? "Невірний формат" : "Incorrect format";
        }

        //Entering parameters

        private void NumOfValEntSummEr_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Right)
            {
                confProbEnteringSE.AllowFocusOnInteraction = true;
                confProbEnteringSE.Focus(FocusState.Keyboard);
            }
        }

        private void NumOfValEntSummEr_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void NumOfValEntSummEr_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = int.TryParse(NumOfValuesEntering_SummEr.Text, out NumOfVal);
            NumOfValuesEntering_SummEr.Width = 100;
            if (!formatIsCorrect)
            {
                IncorrectFormatOutput(NumOfValuesEntering_SummEr);
            }
            else
            {
                Array.Resize(ref values, NumOfVal);
                Array.Resize(ref formatOfValIsCorrect, NumOfVal);
            }
            if (AllParamsEntered())
            {
                TableCreating_SummEr();
            }
        }

        private void ConfProbSE_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Right)
            {
                marginErrorEntering.AllowFocusOnInteraction = true;
                marginErrorEntering.Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                NumOfValuesEntering_SummEr.AllowFocusOnInteraction = true;
                NumOfValuesEntering_SummEr.Focus(FocusState.Keyboard);
            }
        }

        private void ConfProbSE_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = double.TryParse(confProbEnteringSE.Text, out p);
            confProbEnteringSE.Width = 100;
            if (!formatIsCorrect)
            {
                IncorrectFormatOutput(confProbEnteringSE);
            }

            if (AllParamsEntered())
            {
                TableCreating_SummEr();
            }
        }

        private void marginError_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Right)
            {
                roundingIntEntering.AllowFocusOnInteraction = true;
                roundingIntEntering.Focus(FocusState.Keyboard);
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                confProbEnteringSE.AllowFocusOnInteraction = true;
                confProbEnteringSE.Focus(FocusState.Keyboard);
            }
        }

        private void MarginError_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = double.TryParse(marginErrorEntering.Text, out marginOfError);
            marginErrorEntering.Width = 100;
            if (!formatIsCorrect)
            {
                IncorrectFormatOutput(marginErrorEntering);
            }
            if (AllParamsEntered())
            {
                TableCreating_SummEr();
            }
        }

        private void roundingInt_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (NumOfValuesEntering_SummEr.Text == "")
                {
                    NumOfValuesEntering_SummEr.AllowFocusOnInteraction = true;
                    NumOfValuesEntering_SummEr.Focus(FocusState.Keyboard);
                }
                else if (confProbEnteringSE.Text == "")
                {
                    confProbEnteringSE.AllowFocusOnInteraction = true;
                    confProbEnteringSE.Focus(FocusState.Keyboard);
                }
                else if (marginErrorEntering.Text == "")
                {
                    marginErrorEntering.AllowFocusOnInteraction = true;
                    marginErrorEntering.Focus(FocusState.Keyboard);
                }
                else if (roundingIntEntering.Text == "")
                {
                    roundingIntEntering.AllowFocusOnInteraction = true;
                    roundingIntEntering.Focus(FocusState.Keyboard);
                }
                else
                {
                    summaryErrorOutput.Focus(FocusState.Programmatic);
                }
            }
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                marginErrorEntering.AllowFocusOnInteraction = true;
                marginErrorEntering.Focus(FocusState.Keyboard);
            }
        }

        private void RoundingIntEnt_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = double.TryParse(roundingIntEntering.Text, out roundingInterval);
            roundingIntEntering.Width = 100;
            if (!formatIsCorrect)
            {
                IncorrectFormatOutput(roundingIntEntering);
            }
            if (AllParamsEntered())
            {
                TableCreating_SummEr();
            }
        }

        private void maxRowsEntering_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                roundingIntEntering.AllowFocusOnInteraction = true;
                roundingIntEntering.Focus(FocusState.Keyboard);
            }
        }

        private void MaxRowsEnt_LostFocus(object sender, RoutedEventArgs e)
        {
            bool formatIsCorrect = int.TryParse(maxRowsEntering_SummEr.Text, out maxRows);
            if (!formatIsCorrect)
            {
                maxRowsEntering_SummEr.Text = "Incorrect format";
            }
            else
            {
                if (maxRowsEntering_SummEr == sender)
                {
                    TableCreating_SummEr();
                }
            }
        }

        private bool AllParamsEntered()
        {
            if (NumOfValuesEntering_SummEr.Text != "" && confProbEnteringSE.Text != "" && marginErrorEntering.Text != "" && roundingIntEntering.Text != "")
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
                    Height = 50,
                    Width = 100,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Visible,
                    Margin = new Thickness
                    {
                        Left = NumOfValuesEntering_SummEr.Margin.Left + i / maxRows * 210,
                        Top = EnterValuesBlock_SummEr.Margin.Top + EnterValuesBlock_SummEr.Height + i % maxRows * NumOfValuesEntering_SummEr.Height,
                    },
                    FontSize = NumOfValuesEntering_SummEr.FontSize,
                    TextAlignment = NumOfValuesEntering_SummEr.TextAlignment,

                });

                textBoxesList[i].KeyUp += Value_KeyUp;
                textBoxesList[i].KeyDown += Value_KeyDown;
                textBoxesList[i].GotFocus += TextBox_GotFocus;
                textBoxesList[i].LostFocus += TextBox_LostFocus;

                summaryErrorGrid.Children.Add(textBoxesList[i]);
            }
            //textBoxesList[0].AllowFocusOnInteraction = true;
            textBoxesList[0].Focus(FocusState.Keyboard);
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
                    summaryErrorOutput.Focus(FocusState.Programmatic);
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
            if (AllValsEnteredCheck())
            {
                Output();
            }
        }

        private bool AllValsEnteredCheck()
        {
            bool AllValsEnteredCheck = true;
            for (int i = 0; i < NumOfVal; i++)
            {
                if (!formatOfValIsCorrect[i])
                {
                    AllValsEnteredCheck = false;
                }
            }
            return AllValsEnteredCheck;
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
                    if (p <= tableOfTpn[0, j] && n <= tableOfTpn[i, 0])
                    {
                        return tableOfTpn[i, j];
                    }
                }
            }
            return 0;
        }

        private void Output()
        {
            if (language == (byte)LanguageEnum.English)
            {
                SummaryErrorCalculating(values);
                summaryErrorOutput.Text = $"Medium value: {sumMedium}\nAbsolute error: {Round(sumAbsError, 4)}\nRelative error: {Round(sumRelativeError, 2)}% ";
            }
            else if (language == (byte)LanguageEnum.Ukrainian)
            {
                SummaryErrorCalculating(values);
                summaryErrorOutput.Text = $"Середнє значення: {sumMedium}\nАбсолютна похибка: {Round(sumAbsError, 4)}\nВідносна похибка: {Round(sumRelativeError, 2)}% ";
            }
        }
    }
}

