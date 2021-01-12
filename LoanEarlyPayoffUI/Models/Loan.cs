using System;
using System.Text;

namespace LoanEarlyPayoffUI.Models
{
    public class Loan
    {
        public Loan(string name, decimal initialPrincipalRemaining, decimal interestRate,
            decimal initialMonthlyPayment, decimal? oneTimePayment=0, decimal? addlMonthlyPayment=0)
        {
            Name = name;
            InitialPrincipalRemaining = initialPrincipalRemaining;
            InterestRate = interestRate;
            InitialMonthlyPayment = initialMonthlyPayment;
            OneTimePayment = oneTimePayment ?? 0;
            AddlMonthlyPayment = addlMonthlyPayment ?? 0;
        }

        public Loan(string name, string initialPrincipalRemaining, string interestRate,
           string initialMonthlyPayment, string oneTimePayment, string addlMonthlyPayment)
        {
            Name = name;
            InitialPrincipalRemaining = decimal.Parse(initialPrincipalRemaining);
            InterestRate = decimal.Parse(interestRate);
            InitialMonthlyPayment = decimal.Parse(initialMonthlyPayment);
            OneTimePayment = string.IsNullOrWhiteSpace(oneTimePayment) ? 0 : decimal.Parse(oneTimePayment);
            AddlMonthlyPayment = string.IsNullOrWhiteSpace(addlMonthlyPayment) ? 0 : decimal.Parse(addlMonthlyPayment);
        }

        public int PaymentsApplied = 0;
        public string Name { get; set; }
        public decimal InitialPrincipalRemaining { get; set; }
        public decimal PrincipalRemaining { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalInterestPaid = 0;
        public decimal InitialMonthlyPayment { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal AddlMonthlyPayment { get; set; }
        public decimal OneTimePayment { get; set; }
        private decimal InterestMultiplier => InterestRate / 12;
        public DateTime CurrentDT => DateTime.Now;
        public DateTime CalcDT => CurrentDT.AddMonths(PaymentsApplied);

        public Loan SecondLoan { get; set; }

        private decimal CalculateBalanceAfterPayment()
        {
            PaymentsApplied++;
            var interest = CalculateInterest();
            TotalInterestPaid += interest;
            var newBalance = PrincipalRemaining - (MonthlyPayment - interest);
            return newBalance;
        }
        private decimal CalculateInterest() => InterestMultiplier * PrincipalRemaining;

        public string AmoritizeThis(bool? defaultAddlPaymentSetting=false)
        {
            StringBuilder sb = new StringBuilder();
            var applyAddlPayments = defaultAddlPaymentSetting == true ||  OneTimePayment > 0 || AddlMonthlyPayment > 0;
            SingleAmoritize(sb);

            if (applyAddlPayments)
            {
                SecondLoan = CopyLoan();
                SecondLoan.SingleAmoritize(sb, true);
                sb.Append($"You saved {Difference(TotalInterestPaid, SecondLoan.TotalInterestPaid).ToString("C")} in interest " +
                    $"and {Difference(PaymentsApplied, SecondLoan.PaymentsApplied)} months of payments with your additional payment(s)!");
            }
            return sb.ToString();
        }

        private void SingleAmoritize(StringBuilder sb, bool applyAddlPayments = false)
        {
            PrincipalRemaining = InitialPrincipalRemaining;
            MonthlyPayment = InitialMonthlyPayment;

            if (applyAddlPayments)
            {
                PrincipalRemaining -= OneTimePayment;
                MonthlyPayment += AddlMonthlyPayment;
            }
            while (PrincipalRemaining - MonthlyPayment > 0)
            {
                var balanceAfterPayment = CalculateBalanceAfterPayment();
                GenerateScheduleLine(sb, balanceAfterPayment);
                PrincipalRemaining = balanceAfterPayment;
            }

            GenerateTotalLines(sb);
        }

        public Loan CopyLoan()
        {
            return new Loan(Name, InitialPrincipalRemaining, InterestRate, InitialMonthlyPayment, OneTimePayment, AddlMonthlyPayment);
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
