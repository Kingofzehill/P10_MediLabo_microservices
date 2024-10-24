﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using PatientNoteBackAPI.Data;
using PatientNoteBackAPI.Domain;

namespace PatientNoteBackAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly LocalMongoDbContext _localMongoDb;
        public NoteRepository(LocalMongoDbContext localMongoDb)
        {
            _localMongoDb = localMongoDb;
        }

        /// <summary>Patient Note Repository (CRUD operation). List.</summary>      
        /// <return>List of Patient Notes DTO objects.</return>         
        /// <remarks></remarks>
        public async Task<List<Note>> List(int patientId)
        {
            try
            {
                return await _localMongoDb.Notes.Where(note => note.PatientId == patientId).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Repository (CRUD operation). Get.</summary>      
        /// <param name="id">Object Id (mongoDb Id) of Note to get.</param>
        /// <return>Returns Patient Note DTO object.</return> 
        /// <remarks></remarks>
        public async Task<Note?> Get(string id)
        {
            try
            {
                var note = await _localMongoDb.Notes.FirstOrDefaultAsync(note => note.Id == ObjectId.Parse(id)); 
                if (note is not null)
                {
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Repository (CRUD operation). Create.</summary>      
        /// <param name="note">Note DTO object.</param>
        /// <return>Returns Patient Note DTO object.</return> 
        /// <remarks></remarks>
        public async Task<Note> Create(Note note)
        {
            try
            {
                await _localMongoDb.Notes.AddAsync(note);
                await _localMongoDb.SaveChangesAsync();
                return note;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Repository (CRUD operation). Delete.</summary>      
        /// <param name="id">Object Id (mongoDb Id) of Note to delete.</param>
        /// <return>Returns Patient Note DTO object.</return> 
        /// <remarks></remarks>
        public async Task<Note?> Delete(string id)
        {
            try
            {
                var note = await _localMongoDb.Notes.FirstOrDefaultAsync(note => note.Id == ObjectId.Parse(id));
                if (note is not null)
                {
                    _localMongoDb.Notes.Remove(note);
                    await _localMongoDb.SaveChangesAsync();
                    return note;
                }
                else
                {
                    return null;
                }
                
            }
            catch
            {
                throw;
            }
        }
    }
}
