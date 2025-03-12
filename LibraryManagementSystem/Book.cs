using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    internal class Book
    {
        private int bookID;
        private string title;
        private string author;
        private bool isIssued;

        private int GenerateBookID()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
        public Book(string title, string author) 
        {
            bookID = GenerateBookID();
            this.title = title;
            this.author = author;
            isIssued = false;
        }
        public int BookID
        {
            get { return bookID; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public bool IsIssued
        {
            get { return isIssued; }
        }
        public void IsIssuedF()
        {
            isIssued = true;
        }
        public void ReturnBook()
        {
            isIssued = false;
        }
        public override string ToString()
        {
            return $"BookID: {bookID}   Title: {title}  Author: {author}    IsIssued: {isIssued}\n";
        }
    }
}
