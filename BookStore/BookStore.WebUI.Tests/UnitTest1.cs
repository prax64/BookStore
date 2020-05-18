using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Models;
using BookStore.WebUI.HtmlHelpers;

namespace BookStore.WebUI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Игра1"},
                new Book { BookId = 2, Name = "Игра2"},
                new Book { BookId = 3, Name = "Игра3"},
                new Book { BookId = 4, Name = "Игра4"},
                new Book { BookId = 5, Name = "Игра5"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Игра4");
            Assert.AreEqual(books[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Игра1"},
                new Book { BookId = 2, Name = "Игра2"},
                new Book { BookId = 3, Name = "Игра3"},
                new Book { BookId = 4, Name = "Игра4"},
                new Book { BookId = 5, Name = "Игра5"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Act
            BooksListViewModel result
                = (BooksListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Игра1", Category="Cat1"},
                new Book { BookId = 2, Name = "Игра2", Category="Cat2"},
                new Book { BookId = 3, Name = "Игра3", Category="Cat1"},
                new Book { BookId = 4, Name = "Игра4", Category="Cat2"},
                new Book { BookId = 5, Name = "Игра5", Category="Cat3"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Book> result = ((BooksListViewModel)controller.List("Cat2", 1).Model)
                .Books.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book { BookId = 1, Name = "Игра1", Category="Симулятор"},
                new Book { BookId = 2, Name = "Игра2", Category="Симулятор"},
                new Book { BookId = 3, Name = "Игра3", Category="Шутер"},
                new Book { BookId = 4, Name = "Игра4", Category="RPG"},
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "RPG");
            Assert.AreEqual(results[1], "Симулятор");
            Assert.AreEqual(results[2], "Шутер");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book { BookId = 1, Name = "Игра1", Category="Симулятор"},
                new Book { BookId = 2, Name = "Игра2", Category="Шутер"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Шутер";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Book_Count()
        {
            /// Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Игра1", Category="Cat1"},
                new Book { BookId = 2, Name = "Игра2", Category="Cat2"},
                new Book { BookId = 3, Name = "Игра3", Category="Cat1"},
                new Book { BookId = 4, Name = "Игра4", Category="Cat2"},
                new Book { BookId = 5, Name = "Игра5", Category="Cat3"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((BooksListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((BooksListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((BooksListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}