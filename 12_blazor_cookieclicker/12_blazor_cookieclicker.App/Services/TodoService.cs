using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using _12_blazor_cookieclicker.App.Models;

namespace _12_blazor_cookieclicker.App.Services
{
    public class TodoService
    {
        public List<Todo> GetAll()
        {
            try
            {
                using (var db = new ModelDbContext())
                {
                    return db.Todos.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public void Add(Todo todo)
        {
            try
            {
                using (var db = new ModelDbContext())
                {
                    db.Todos.Add(todo);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}