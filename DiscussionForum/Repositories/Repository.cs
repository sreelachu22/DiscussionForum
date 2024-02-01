using DiscussionForum.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
            // Use the DbSet<T> to access entities of type T in the database.
            // Apply the provided predicate- (function taking an entity of type T
            // and returning a boolean (bool) value)- to filter entities.
            // Convert the result to a List<T> and return.
        }

    }
}
