using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LibraryManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            LibrarySystem library = new LibrarySystem();
            bool exit = false;

            while (!exit)
            {
                Console.Write("\n--------------------------");
                Console.Write(" Welcome to Library Management System ");
                Console.WriteLine("--------------------------");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. Update Member");
                Console.WriteLine("3. Delete Member");
                Console.WriteLine("4. Display Members");
                Console.WriteLine("5. Add Book");
                Console.WriteLine("6. Update Book");
                Console.WriteLine("7. Delete Book");
                Console.WriteLine("8. Display Books");
                Console.WriteLine("9. Issue Book");
                Console.WriteLine("10. Return Book");
                Console.WriteLine("11. Declare Most Borrowed Book");
                Console.WriteLine("12. View Borrow History");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        library.AddMember();
                        break;

                    case "2":
                        Console.Write("Enter Member CNIC to update: ");
                        string updateCnic = Console.ReadLine();
                        library.UpdateMember(updateCnic);
                        break;

                    case "3":
                        Console.Write("Enter Member CNIC to delete: ");
                        string deleteCnic = Console.ReadLine();
                        library.DeleteMember(deleteCnic);
                        break;

                    case "4":
                        library.DisplayMembers();
                        break;

                    case "5":
                        library.AddBook();
                        break;

                    case "6":
                        Console.Write("Enter Book ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateBookId))
                        {
                            library.UpdateBook(updateBookId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Book ID.");
                        }
                        break;

                    case "7":
                        Console.Write("Enter Book ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteBookId))
                        {
                            library.DeleteBook(deleteBookId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Book ID.");
                        }
                        break;

                    case "8":
                        library.DisplayBooks();
                        break;

                    case "9":
                        Console.Write("Enter Book ID to issue: ");
                        string issueBookId = Console.ReadLine();
                        Console.Write("Enter Member CNIC to issue to: ");
                        string issueCnic = Console.ReadLine();
                        library.IssueBook(issueBookId, issueCnic);
                        break;

                    case "10":
                        Console.Write("Enter Book ID to return: ");
                        string returnBookId = Console.ReadLine();
                        Console.Write("Enter Member CNIC: ");
                        string returnCnic = Console.ReadLine();
                        library.ReturnBook(returnBookId, returnCnic);
                        break;

                    case "11":
                        library.DeclareMostBorrowedBook();
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting Library Management System......");
                        break;

                    case "12":
                        library.BorrowHistory();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose a number from 0 to 11.");
                        break;
                }
            }
        }
    }
}
