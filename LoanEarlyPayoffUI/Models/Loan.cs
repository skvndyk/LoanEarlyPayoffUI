using System;
using System.Text;

namespace LoanEarlyPayoffUI.Models
{
    public class Loan
    {
        public Loan(string name, decimal initialPrincipalRemaining, decimal interestRate,
            decimal monthlyPayment, decimal oneTimePayment)
        {
            Name = name;
            InitialPrincipalRemaining = initialPrincipalRemaining;
            InterestRate = interestRate;
            MonthlyPayment = monthlyPayment;
            OneTimePayment = oneTimePayment;
        }

        public Loan(string name, string initialPrincipalRemaining, string interestRate,
           string monthlyPayment, string oneTimePayment)
        {
            Name = name;
            InitialPrincipalRemaining = decimal.Parse(initialPrincipalRemaining);
            InterestRate = decimal.Parse(interestRate);
            MonthlyPayment = decimal.Parse(monthlyPayment);
            OneTimePayment = decimal.Parse(oneTimePayment);
        }

        public int PaymentsApplied = 0;
        public string Name { get; set; }
        public decimal InitialPrincipalRemaining { get; set; }
        public decimal PrincipalRemaining { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalInterestPaid = 0;
        public decimal MonthlyPayment { get; set; }
        public decimal OneTimePayment { get; set; }
        private decimal InterestMultiplier => InterestRate / 12;
        public DateTime CurrentDT => DateTime.Now;
        public DateTime CalcDT => CurrentDT.AddMonths(PaymentsApplied);

        private decimal CalculateBalanceAfterPayment()
        {
            PaymentsApplied++;
            var interest = CalculateInterest();
            TotalInterestPaid += interest;
            var newBalance = PrincipalRemaining - (MonthlyPayment - interest);
            return newBalance;
        }
        private decimal CalculateInterest() => InterestMultiplier * PrincipalRemaining;

        public string AmoritizeThis()
        {
            StringBuilder sb = new StringBuilder();
            var applyOneTimePayment = OneTimePayment > 0;

            SingleAmoritize(sb);

            if (applyOneTimePayment)
            {
                var secondLoan = CopyLoan();
                secondLoan.SingleAmoritize(sb, true);
                sb.Append($"You saved {Difference(TotalInterestPaid, secondLoan.TotalInterestPaid).ToString("C")} in interest " +
                    $"and {Difference(PaymentsApplied, secondLoan.PaymentsApplied)} months of payments with your one-time payment!");
            }
            return sb.ToString();
        }

        private void SingleAmoritize(StringBuilder sb, bool applyOneTimePayment = false)
        {
            PrincipalRemaining = InitialPrincipalRemaining;
            if (applyOneTimePayment)
            {
                PrincipalRemaining -= OneTimePayment;
            }
            while (PrincipalRemaining - MonthlyPayment > 0)
            {
                var balanceAfterPayment = CalculateBalanceAfterPayment();
                GenerateScheduleLine(sb, balanceAfterPayment);
                PrincipalRemaining = balanceAfterPayment;
            }

            GenerateTotalLines(sb);
        }

        private Loan CopyLoan()
        {
            return new Loan(Name, InitialPrincipalRemaining, InterestRate, MonthlyPayment, OneTimePayment);
        }

        private decimal Difference(decimal val1, decimal val2) => Math.Abs(val2 - val1);

        private void GenerateScheduleLine(StringBuilder sb, decimal balance)
        {
            sb.Append($"{CalcDT.ToString("MM/yy")}\t{balance.ToString("C")}");
            sb.Append(Environment.NewLine);
        }

        private void GenerateTotalLines(StringBuilder sb)
        {
            sb.Append($"Total Payments: {PaymentsApplied}");
            sb.Append(Environment.NewLine);
            sb.Append($"Total Interest Paid: {TotalInterestPaid.ToString("C")}");
            sb.Append(Environment.NewLine);
        }
    }
}
