using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Domain.Entities;
using CleanArch.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customer;
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _customer = dbContext.Set<Customer>();
        }
     
    }
}
