using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LibraryManagementSystem
{
    internal class LibrarySystem
    {
        private List<Book> books;
        private List<Member> members;

        public LibrarySystem()
        {
            books = new List<Book>();
            members = new List<Member>();
            LoadData();
        }
        private void LoadData()
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
           
                // loading Members
                string memberQuery = "SELECT * FROM Member";
                SqlCommand memberCmd = new SqlCommand(memberQuery, conn);
                SqlDataReader memberReader = memberCmd.ExecuteReader();
                while (memberReader.Read())
                {
                    members.Add(new Member((string)memberReader["Name"], (string)memberReader["CNIC"]));
                }
                memberReader.Close();

                // loading Books
                string bookQuery = "SELECT * FROM Book";
                SqlCommand bookCmd = new SqlCommand(bookQuery, conn);
                SqlDataReader bookReader = bookCmd.ExecuteReader();
                while (bookReader.Read())
                {
                    books.Add(new Book((string)bookReader["Title"], (string)bookReader["Author"]));
                }
                bookReader.Close();
                conn.Close();
            } catch (Exception ex) 
            {
                Console.WriteLine($"Error occured loading data: {ex.Message}");
                conn.Close();
            }

        }
        public void AddMember()
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                // taking input 
                Console.WriteLine("Enter Member details...");
                Console.WriteLine("Name: ");
                string name = Console.ReadLine();
                Console.WriteLine("CNIC no: ");
                string cnic = Console.ReadLine();

                //first making object then adding to list then making changes in db
                Member member = new Member(name, cnic);
                members.Add(member);

                
                string query = $"INSERT into Member (Name, CNIC) VALUES ('{name}', '{cnic}')";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                Console.WriteLine(count > 0 ? "Member added successfully." : "problem adding Member to db....");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding member: {ex.Message}");
                conn.Close();
            }
        }
        public void UpdateMember(string cnic)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                // taking input 
                Console.Write("Enter new Name: ");
                string newName = Console.ReadLine();

                Member member = members.Find(m => m.CNIC == cnic);
                if (member != null)
                {
                    member.Name = newName;
                }

                // updating db
                
                string query = $"UPDATE Member SET Name = '{newName}' where CNIC = '{cnic}';";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                Console.WriteLine(count > 0 ? "Member updated successfully..." : "Problem updating Member to db...");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating member: {ex.Message}");
                conn.Close();
            }
        }
        public void DeleteMember(string cnic)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                // Check if the member has any currently issued books
                string checkQuery = $@"
                SELECT COUNT(*) FROM BorrowedBooks bb
                JOIN Book b ON bb.BookID = b.BookID
                WHERE bb.MemberCNIC = '{cnic}' AND b.IsIssued = 1";

                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                int issuedBooks = (int)checkCmd.ExecuteScalar();

                if (issuedBooks > 0)
                {
                    Console.WriteLine("Cannot delete member. They have books currently issued.");
                    return;
                }

                members.RemoveAll(m => m.CNIC == cnic);
                
                string query = $"DELETE FROM Member WHERE CNIC = '{cnic}';";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                Console.WriteLine(count > 0 ? "Member deleted successfully..." : "Problem deleting Member to db...");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting member: {ex.Message}");
                conn.Close();
            }
        }
        public void DisplayMembers()
        {
            // displaying using db and instead of lists
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            string query = $"SELECT * from Member;";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader memberData = cmd.ExecuteReader();
            Console.WriteLine("\nMembers include:");

            Console.WriteLine($"\n{"MemberID",-10} {"Name",-20} {"CNIC",-15}");
            while (memberData.Read()) 
            {
                Console.WriteLine($"{memberData[0],-10} {memberData[1],-20} {memberData[2],-15}");

            }
            Console.WriteLine();
            conn.Close();
        }

        public void AddBook()
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                Console.Write("Enter Book Title: ");
                string title = Console.ReadLine();
                Console.Write("Enter Author: ");
                string author = Console.ReadLine();

                Book book = new Book(title, author);

                string query = $"INSERT into Book (BookID, Title, Author, IsIssued) VALUES ( {book.BookID}, '{title}', '{author}', 0)";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                Console.WriteLine(count > 0 ? "Book added successfully." : "Problem adding Book to db....");
                conn.Close();
                
                books.Add(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding book: {ex.Message}");
                conn.Close();
            }
        }
        public void UpdateBook(int bId)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                Console.Write("Enter new Title: ");
                string title = Console.ReadLine();
                Console.Write("Enter new Author: ");
                string author = Console.ReadLine();

                Book book = books.Find(b => b.BookID == bId);
                if (book != null)
                {
                    book.Title = title;
                    book.Author = author;
                }

                
                string query = $"UPDATE Book SET Title = '{title}' , Author = '{author}' WHERE BookID = {bId}";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                Console.WriteLine(count > 0 ? "Book updated successfully." : "Problem adding Book to db....");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book: {ex.Message}");
                conn.Close();
            }
        }
        public void DeleteBook(int bId)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {

                string checkQuery = $"SELECT IsIssued FROM Book WHERE BookID = {bId}";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                object isIssued = checkCmd.ExecuteScalar();

                if (isIssued == null)
                {
                    Console.WriteLine("Invalud book ID.");
                    return;
                }
                if ((bool)isIssued)
                {
                    Console.WriteLine("Cannot delete a book that is currently issued.");
                    return;
                }

                books.RemoveAll(b => b.BookID == bId);
                
                string query = $"DELETE FROM Book WHERE BookID = {bId}";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine(count > 0 ? "Book deleted successfully." : "Book not found or could not be deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book: {ex.Message}\nCant delete book bcz book has been borrowed\n");
                conn.Close();
            }
        }
        public void DisplayBooks()
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            // displaying data from db instead of list 
            try
            {
                
                string query = "SELECT * FROM Book";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine($"{"ID",-5} {"Title",-30} {"Author",-20} {"Issued",-10}"); // Header
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0],-5} {reader[1],-30} {reader[2],-20} {reader[3],-10}");
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader[0]}, Title: {reader[1]}, \t\t\tAuthor: {reader[2]}, \t\tIssued: {reader[3]}");
                }
                Console.WriteLine();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying books: {ex.Message}");
                conn.Close();
            }
        }

        public void IssueBook(string bId, string cnic)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {
                string checkMemberQuery = $"SELECT COUNT(*) FROM Member WHERE CNIC = '{cnic}'";
                SqlCommand checkMemberCmd = new SqlCommand(checkMemberQuery, conn);
                int memberExists = (int)checkMemberCmd.ExecuteScalar();
                if (memberExists == 0)
                {
                    Console.WriteLine("Member does not exist.");
                    return;
                }
                string checkQuery = $"SELECT IsIssued FROM Book WHERE BookID = {bId}";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                object result = checkCmd.ExecuteScalar();

                if (result != null && !(bool)result)
                {
                    string updateQuery = $"UPDATE Book SET IsIssued = 1 WHERE BookID = {bId}";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    int updateCount = updateCmd.ExecuteNonQuery();

                    string insertQuery = $"INSERT INTO BorrowedBooks (MemberCNIC, BookID) VALUES ('{cnic}', {bId})";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    int insertCount = insertCmd.ExecuteNonQuery();

                    Console.WriteLine(updateCount > 0 && insertCount > 0 ? "Book issued successfully." : "Error issuing the book.");
                }
                else
                {
                    Console.WriteLine("Book not found or already issued.");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error issuing book: {ex.Message}");
                conn.Close();
            }
        }
        public void ReturnBook(string bId, string cnic)
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {

                string updateQuery = $"UPDATE Book SET IsIssued = 0 WHERE BookID = {bId}";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                int updateCount = updateCmd.ExecuteNonQuery();

                //if (updateCount > 0)
                //{
                //    string deleteQuery = $"DELETE FROM BorrowedBooks WHERE MemberCNIC = '{cnic}' AND BookID = {bId}";
                //    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                //    int deleteCount = deleteCmd.ExecuteNonQuery();

                //    Console.WriteLine(deleteCount > 0 ? "Book returned successfully." : "Error removing the borrow record.");
                //}
                //else
                //{
                //    Console.WriteLine("Book not found or not issued.");
                //}
                Console.WriteLine(updateCount > 0 ? "Book returned successfully." : "Error removing the borrow record.");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error returning book: {ex.Message}");
                conn.Close();
            }
        }
        public void DeclareMostBorrowedBook()
        {

            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            try
            {

                string query = "SELECT TOP 1 BookID, COUNT(*) AS BorrowCount FROM BorrowedBooks GROUP BY BookID ORDER BY BorrowCount DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine($"Most Borrowed Book ID: {reader["BookID"]}, Times Borrowed: {reader["BorrowCount"]}");
                }
                else
                {
                    Console.WriteLine("No books have been borrowed yet.");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error declaring most borrowed book: {ex.Message}");
                conn.Close();
            }
        }
        public void BorrowHistory()
        {
            SqlConnection conn = DBAssistant.GetConnection();
            conn.Open();
            // displaying data from db instead of list 
            try
            {

                string query = "SELECT * FROM BorrowedBooks";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine($"{"BorrowID",-10} {"MemberCNIC",-15} {"BookID",-10}");
                while (reader.Read())
                {
                    if (reader.IsDBNull(1))
                    {
                        Console.WriteLine($"{reader[0],-10} {"deleted",-15} {reader[2],-10}");
                    }
                    else if (reader.IsDBNull(2))
                    {
                        Console.WriteLine($"{reader[0],-10} {reader[1], -15} {"deleted",-10}");
                    }
                    else
                    {
                        Console.WriteLine($"{reader[0],-10} {reader[1],-15} {reader[2],-10}");
                    }
                }
                Console.WriteLine();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying books: {ex.Message}");
                conn.Close();
            }
        }
    }
}
