using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Book book, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Book book)
        {
            lineCollection.RemoveAll(l => l.Book.BookId == book.BookId);
        }

        public decimal ComputeTotalValue()
        {
            decimal sumPrice = lineCollection.Where(e => e.Quantity < 5).
                Sum(e => e.Book.Price * e.Quantity);
            decimal sumTradePrice = lineCollection.Where(e => e.Quantity >= 5).
                Sum(e => e.Book.TradePrice * e.Quantity);
            decimal sumTradePriceUndiscounted = lineCollection.Where(e => e.Quantity >= 5).
                Sum(e => e.Book.Price * e.Quantity);
            if (sumPrice > 2000)
            {
                sumPrice *=  0.9M;
            }
            else if (sumPrice >= 1000 && sumPrice <= 2000)
            {
                sumPrice *= 0.95M;
            }

            if(sumTradePriceUndiscounted >= 5000)
            {
                sumTradePrice *= 0.97M;
            }

            return sumPrice + sumTradePrice;
        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
