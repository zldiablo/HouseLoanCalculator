using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HouseLoan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            ResultPanel.Children.Clear();

            int totalTime;
            if (!int.TryParse(ReadLoanYears(LoanYears), out totalTime))
            {
                Reset();
                return;
            }
            if (totalTime < 0 || totalTime > 30)
            {
                Reset();
                return;
            }
            totalTime *= 12;

            double amount;
            if (!double.TryParse(LoanAmount.Text, out amount))
            {
                Reset();
                return;
            }
            amount *= 10000.0;

            double interest = GetInterest(totalTime / 12);

            if (PayType.SelectedIndex == 0)
            {
                //等额本息
                double r = LoanCalculator.Calculate1(amount, interest / 12, totalTime);
                ResultTitle.Text = "月均还款额：";
                ResultTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
                TextBlock tb = new TextBlock();
                tb.Text = string.Format("{0:f} 元", r);
                tb.FontSize = 18;
                ResultPanel.Children.Add(tb);
            }
            else
            {
                //等额本金
                double[] r = LoanCalculator.Calculate2(amount, interest / 12, totalTime);

                for (int i = 0; i < r.Length; i++)
                {
                    ResultPanel.Children.Add(GetResultBlock(i + 1, r[i]));
                }

                ResultTitle.Text = "各月还款额：";
                ResultTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            //ResultPanel.Children.Add(t);
        }

        private TextBlock GetResultBlock(int index, double number)
        {
            TextBlock tb = new TextBlock();
            tb.FontSize = 18;
            tb.Text = string.Format("{0}月: {1:f} 元", index, number);
            return tb;
        }

        private double GetInterest(int years)
        {
            double interest;
            if (LoanType.SelectedIndex == 1)
            {
                if (years > 5) { interest = 0.045; }
                else { interest = 0.04; }
            }
            else
            {
                //if (InterestChange.SelectedIndex == 0)
                //{
                //    interest = 0.0655;
                //}
                //else if (InterestChange.SelectedIndex == 1)
                //{
                //    interest = 0.0556;
                //}
                //else
                //{
                //    interest = 0.0725;
                //}
                if (years > 5) { interest = 0.0655; }
                else { interest = 0.064; }
            }

            if (InterestChange.SelectedIndex == 1)
            {
                interest *= 0.85;
            }
            else if (InterestChange.SelectedIndex == 2)
            {
                interest *= 1.1;
            }
            return interest;
        }

        private void Reset()
        {
        }

        private string ReadLoanYears(ComboBox box)
        {
            int length = box.Items.Count();
            int selectedIndex = box.SelectedIndex;
            if (selectedIndex < length - 1)
            {
                return ((ComboBoxItem)box.SelectedItem).Content.ToString();
            }
            else
            {
                return OtherYears.Text;
            }
        }

        private static string ReadSelectString(ComboBox box)
        {
            var select = box.SelectedValue as ComboBoxItem;
            if (select != null)
            {
                return select.Content.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
