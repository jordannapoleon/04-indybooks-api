#nullable enable
using Bogus.DataSets;
using IndyBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Services;
public class ApiWriterService : IWriterService
{
    private IndyBooksDataContext _db;
    public ApiWriterService(IndyBooksDataContext db) { _db = db; }
   
    public List<Writer> GetWriterList()
    {
        return _db.Writers.ToList();
    }
    public Writer? GetWriterById(long id)
    {
        return _db.Writers.SingleOrDefault(w=>w.Id == id); //Uses lamda function and extension methods here
    }
    public Writer DeleteWriterById(long id)
    {
        //TODO: Get the Writer at the given id from the db context
        //     Remove the Writer at that id, be sure to SaveChanges()
        Writer RemoveMe = _db.Writers.Include(w => w.Books)
                                     .FirstOrDefault(w => w.Id == id);

        if(RemoveMe is null)
        {
            return null;
        }

        if(RemoveMe.Books is not null)
        {
            _db.Books.RemoveRange(RemoveMe.Books);
        }

        _db.Writers.Remove(RemoveMe);
        _db.SaveChanges();

        //TODO: return the deleted Writer info
        return RemoveMe;
    }
    public long PostWriter(Writer writer)
    {
        //TODO : Add a new Writer to the db context, return the writer id
        _db.Writers.Add(writer);
        _db.SaveChanges();

        return writer.Id;
    }
    public Writer PutWriter(Writer writer, long id)
    {
        //TODO: Update the Writer at the given id, return the Writer
        Writer UpdateMe = _db.Writers.Find(id);

        if( UpdateMe is null )
        {
            return null;
        }

        UpdateMe.Name = writer.Name;
        UpdateMe.Books = writer.Books;

        _db.SaveChanges();

        return UpdateMe ;
    }
}
