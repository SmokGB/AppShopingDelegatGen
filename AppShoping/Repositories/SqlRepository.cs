using AppShoping.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AppShoping.Repositories;

public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()

{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    public event EventHandler<T?>? ItemAdded, ProductDeleted;

    public SqlRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }


    public IEnumerable<T> GetAll() => _dbSet.ToList();
    public T? GetById(int id) => _dbSet.Find(id);
  
    public void Remove(T item)
    {
        _dbSet.Remove(item);
        ProductDeleted?.Invoke(this, item);
    }
    public void Add(T item)
    {
        _dbSet.Add(item);
        ItemAdded?.Invoke(this, item);
    }
    public void Save() => _dbContext.SaveChanges();
}