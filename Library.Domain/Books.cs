using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ISBN { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
    }
}
