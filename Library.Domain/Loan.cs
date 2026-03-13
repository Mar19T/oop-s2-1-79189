using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class Loan
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public Books Book { get; set; } 
        public int MemberID{ get; set; }
        public Member Member { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
