using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoanEarlyPayoffUI.Models;

namespace LoanEarlyPayoffUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            NameInput1.Text = "Juke";
            InitPrRemainInput1.Text = "7924.59";
            InterestRateInput1.Text = "0.0469";
            MonthlyPaymentInput1.Text = "280.94";
            LumpSumInput1.Text = "3960";

            NameInput2.Text = "g37";
            InitPrRemainInput2.Text = "4699.5";
            InterestRateInput2.Text = "0.0259";
            MonthlyPaymentInput2.Text = "266.81";
            LumpSumInput2.Text = "1040";
        }

        private void AmoritizeButton_Click(object sender, EventArgs e)
        {
            //FindLowestInterestPaidPlan();
            var loan1 = new Loan(name1.Text, InitPrRemainInput1.Text, InterestRateInput1.Text, MonthlyPaymentInput1.Text,
                LumpSumInput1.Text, AddlMonthlyInput1.Text);
            var output1 = loan1.AmoritizeThis();
            Schedule1.Text = output1;

            var loan2 = new Loan(name2.Text, InitPrRemainInput2.Text, InterestRateInput2.Text, MonthlyPaymentInput2.Text,
              LumpSumInput2.Text, AddlMonthlyInput2.Text);
            var output2 = loan2.AmoritizeThis();
            Schedule2.Text = output2;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void FindLowestInterestPaidPlan()
        {
            var loan1 = new Loan(name1.Text, InitPrRemainInput1.Text, InterestRateInput1.Text, MonthlyPaymentInput1.Text,
                LumpSumInput1.Text, AddlMonthlyInput1.Text);
            var loan2 = new Loan(name2.Text, InitPrRemainInput2.Text, InterestRateInput2.Text, MonthlyPaymentInput2.Text,
               LumpSumInput2.Text, AddlMonthlyInput2.Text);
            var maxAddl = 500;
            var addlPayment1 = maxAddl;
            var addlpayment2 = 0;

            var paymentIncrement = 5;

            var outcomes = new List<Outcome>();
            while (addlPayment1 >= 0 && addlpayment2 <= maxAddl)
            {
                loan1.PrincipalRemaining = loan1.InitialPrincipalRemaining;
                loan2.PrincipalRemaining = loan2.InitialPrincipalRemaining;

                loan1.OneTimePayment = addlPayment1;
                loan2.OneTimePayment = addlpayment2;

                var outcome = new Outcome() { AddlPayment1 = addlPayment1, AddlPayment2 = addlpayment2 };
                loan1.AmoritizeThis();
                loan2.AmoritizeThis();

                outcome.InterestPaid1 = loan1.SecondLoan.TotalInterestPaid;
                outcome.InterestPaid2 = loan2.SecondLoan.TotalInterestPaid;
                outcome.PaymentsApplied1 = loan1.SecondLoan.PaymentsApplied;
                outcome.PaymentsApplied2 = loan2.SecondLoan.PaymentsApplied;

                outcomes.Add(outcome);

                addlPayment1 -= paymentIncrement;
                addlpayment2 += paymentIncrement;

                loan1 = loan1.CopyLoan();
                loan2 = loan2.CopyLoan();
            }
            int v = 4;
            var best = outcomes.Where(o1 => o1.TotalInterestPaid == outcomes.Min(o => o.TotalInterestPaid)).First();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
