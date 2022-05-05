using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Enrollment itm = await _context.Enrollments.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.Enrollments.Remove(itm);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lst = await _context.Enrollments.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Enrollment itm = await _context.Enrollments.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Enrollments.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");

                }
                exists = new Enrollment();
                exists.SectionId = _Item.SectionId;
                exists.StudentId = _Item.StudentId;
                exists.EnrollDate = _Item.EnrollDate;
                exists.SchoolId = _Item.SchoolId;
                _context.Enrollments.Add(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Enrollments.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exists != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exists = new Enrollment();
                exists.SectionId = _Item.SectionId;
                exists.StudentId = _Item.StudentId;
                exists.EnrollDate = _Item.EnrollDate;
                exists.SchoolId = _Item.SchoolId;
                _context.Enrollments.Update(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}