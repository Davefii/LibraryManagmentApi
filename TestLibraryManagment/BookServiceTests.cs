using BusinessLayer.Services;
using BusinessLayer.DTOs;
using BusinessLayer.Services.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using Moq;
using Xunit;
namespace TestLibraryManagment
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly BookService _bookService;
        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _bookService = new BookService
                (
                     _bookRepositoryMock.Object,
                     _authorRepositoryMock.Object,
                     _categoryRepositoryMock.Object
                );
        }
        /*[Fact]
        public async Task GetBookById_BookExists_ReturnsBookDTO()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Clean Code",
                Isbn = "9780132350884",
                Description = "A book about clean programming",
                PublishYear = 2008,
                TotalCopies = 5,
                AvailableCopies = 3,
                IsAvailable = true
            };
            _bookRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            // Act
            var result = await _bookService.GetBookById(1);

            // Assert 
            Assert.NotNull(result);

            Assert.Equal(1, result.Id);
            Assert.Equal("Clean Code", result.Title);
            Assert.Equal("9780132350884", result.ISBN);
            Assert.Equal(5, result.CopiesCount);
            Assert.Equal(3, result.AvailableCopies);

            _bookRepositoryMock.Verify(repo => repo.GetByIdAsync(1),Times.Once);
        }
        [Fact]
        public async Task GetBookById_BookDoesNotExist_ReturnsNull()
        {
            // Arrange
            _bookRepositoryMock
                .Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync((Book?)null);

            // Act
            var result = await _bookService.GetBookById(100);

            // Assert
            Assert.Null(result);

            _bookRepositoryMock.Verify(
                repo => repo.GetByIdAsync(100),
                Times.Once
            );
        }*/
        /*[Fact]
        public async Task GetBookByTitle_BookExists_ReturnsBookDTO()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Clean Code",
                Isbn = "9780132350884",
                Description = "A book about clean programming",
                PublishYear = 2008,
                TotalCopies = 5,
                AvailableCopies = 3,
                IsAvailable = true
            };
            _bookRepositoryMock.Setup(repo => repo.GetByTitleAsync("Clean Code")).ReturnsAsync(book);
            // Act 
            var result = await _bookService.GetBookByTitle("Clean Code");
            //Assert
            Assert.NotNull(result);

            Assert.Equal(1, result.Id);
            Assert.Equal("Clean Code", result.Title);
            Assert.Equal("9780132350884", result.ISBN);
            Assert.Equal(5, result.CopiesCount);
            Assert.Equal(3, result.AvailableCopies);

            _bookRepositoryMock.Verify(repo => repo.GetByTitleAsync("Clean Code"), Times.Once);
        }
        [Fact]
        public async Task GetBookByTitle_BookDoesNotExist_ReturnsNull()
        {
            _bookRepositoryMock.Setup(repo => repo.GetByTitleAsync("C++")).ReturnsAsync((Book?)null);

            var result = await _bookService.GetBookByTitle("C++");

            Assert.Null(result);
            _bookRepositoryMock.Verify(repo => repo.GetByTitleAsync("C++"), Times.Once);
        }*/
        [Fact]
        public async Task AddBook_AuthorDoesNotExist_ThrowsException()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Programming"
            };
            var bookDto = new CreateBookDTO
            {
                Title = "Clean Code",
                ISBN = "9780132350884",
                Description = "A book about clean programming",
                PublishYear = 2008,
                TotalCopies = 5,
                AvailableCopies = 3,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CoverImage = "1.jpg"
            };
            var book = new Book
            {
                Title = bookDto.Title,
                Isbn = bookDto.ISBN,
                Description = bookDto.Description,
                PublishYear = bookDto.PublishYear,
                TotalCopies = bookDto.TotalCopies,
                AvailableCopies = bookDto.AvailableCopies,
                IsAvailable = bookDto.IsAvailable,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CoverImage = bookDto.CoverImage
            };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Author?)null);
            _bookRepositoryMock.Setup(repo => repo.AddAsync(book));
            var result = await Assert.ThrowsAsync<Exception>(() => _bookService.AddBook(bookDto));
            Assert.Equal("Author not found", result.Message);
            _bookRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Book>()), Times.Never);
        }
    }
}
