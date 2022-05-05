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
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Section itmSec = await _context.Sections.Where(x => x.SectionNo == KeyValue).FirstOrDefaultAsync();
                _context.Sections.Remove(itmSec);
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
            List<Section> lstSec = await _context.Sections.OrderBy(x => x.SectionNo).ToListAsync();
            return Ok(lstSec);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Section itmSec = await _context.Sections.Where(x => x.SectionNo == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSec);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section already exists.");

                }
                exists = new Section();
                exists.SectionId = _Item.SectionId;
                exists.CourseNo = _Item.CourseNo;
                exists.SectionNo = _Item.SectionNo;
                exists.SchoolId = _Item.SchoolId;
                exists.InstructorId = _Item.InstructorId;
                _context.Sections.Add(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionNo);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo).FirstOrDefaultAsync();
                if (exists != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exists = new Section();
                exists.SectionId = _Item.SectionId;
                exists.CourseNo = _Item.CourseNo;
                exists.SectionNo = _Item.SectionNo;
                exists.SchoolId = _Item.SchoolId;
                exists.InstructorId = _Item.InstructorId;
                _context.Sections.Update(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionNo);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}