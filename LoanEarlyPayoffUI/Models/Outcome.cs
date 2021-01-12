using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanEarlyPayoffUI.Models
{
    public class Outcome
    {
        public decimal AddlLumpSum1 { get; set; }
        public decimal AddlLumpSum2 { get; set; }
        public decimal AddlMonthly1 { get; set; }
        public decimal AddlMonthly2 { get; set; }
        public decimal InterestPaid1 { get; set; }
        public decimal InterestPaid2 { get; set; }
        public decimal PaymentsApplied1 { get; set; }
        public decimal PaymentsApplied2 { get; set; }
        public decimal TotalInterestPaid => InterestPaid1 + InterestPaid2;
    }
}
