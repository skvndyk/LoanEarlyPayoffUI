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
        }

        private void AmoritizeButton_Click(object sender, EventArgs e)
        {
            var loan1 = new Loan(name1.Text, InitPrRemainInput1.Text, InterestRateInput1.Text, MonthlyPaymentInput1.Text,
                LumpSumInput1.Text);
            var output = loan1.AmoritizeThis();
            Schedule1.Text = output;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
