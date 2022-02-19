using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementSystem.Models
{
    public class AllModel
    {

        public static List<Loan> Loans { get; set; }
        public static List<LoanPlan> LoanPlans { get; set; }

        public static List<LoanType> LoanType { get; set; }

        public static List<UserAccount> Account { get; set; }
    }
}
