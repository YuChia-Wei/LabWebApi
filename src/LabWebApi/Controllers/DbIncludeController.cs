﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LabWebApi.Repository;
using LabWebApi.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabWebApi.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DbIncludeController : ControllerBase
    {
        private readonly EfCoreSampleContext _context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public DbIncludeController(EfCoreSampleContext context)
        {
            _context = context;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<DbFirstTable> GenerateAsync()
        {
            var data = new DbFirstTable
            {
                MainData = "123",
                AmountField = 123,
                DateTimeField = DateTime.Now,
                Sub = new SubTable
                {
                    SubData = "this is sub",
                    End = new EndTable
                    {
                        EndData = "this is end"
                    }
                }
            };
            _context.DbFirstTables.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var dbFirstTables = await _context.DbFirstTables
                                              .Where(o => o.MainId == id)
                                              .Include(o => o.Sub)
                                              .ThenInclude(o => o.End)
                                              .ToListAsync();
            return Ok(dbFirstTables);
        }
    }
}