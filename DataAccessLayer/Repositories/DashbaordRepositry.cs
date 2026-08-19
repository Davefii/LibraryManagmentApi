using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DashbaordRepositry
    {
        /*private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly MemberRepository _memberRepository;
        private readonly BorrowingRepository _borrowingRepository;*/
        private readonly AppDbContext _context;
        public DashbaordRepositry(AppDbContext context)
        {
            _context = context;
        }

    }
}
