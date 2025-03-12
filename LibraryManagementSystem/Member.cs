using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    internal class Member
    {
        private string name;
        private string cnic;
        private List<int> borrowedBooks;    // took type as int because we are supposed to store the book ids and not the books
        
        public Member(string name, string cnic)
        {
            this.name = name;
            this.cnic = cnic;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string CNIC 
        { 
            get { return cnic; }
        }
        public bool HasBorrowed(int id) 
        { 
            return borrowedBooks.Contains(id);
        }

    }
}
