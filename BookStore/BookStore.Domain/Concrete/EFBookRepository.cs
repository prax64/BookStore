using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else//если не id != 0, то редактируем запись
            {
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Description = book.Description;
                    dbEntry.Price = book.Price;
                    dbEntry.TradePrice = book.TradePrice;
                    dbEntry.Category = book.Category;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.Find(bookId);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
