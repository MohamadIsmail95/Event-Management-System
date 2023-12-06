using ClinicSystem.Services.IServices;
using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Dtos.Event;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities.Users;
using System.Security.Claims;
using Domain.Interfaces;

namespace BackendTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Normal,Admin")]

    public class EventController : ControllerBase
    {
        private readonly IUnitOfWorkService _initOfWorkService;
        private readonly IjwtService _jwtService;
        public EventController(IUnitOfWorkService initOfWorkService, IjwtService jwtService)
        {
            _initOfWorkService = initOfWorkService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateEvent input)
        {
            input.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
            var data = await _initOfWorkService.EventService.CreateAsync(input);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> Get(Guid id)
        {

            var data = await _initOfWorkService.EventService.GetEntityDtoByIdAsync(id);
            return Ok(data);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Put(Guid id, UpdateEvent input)
        {
          
            if (id != input.Id)
            {
                return BadRequest();
            }
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEvent = await _initOfWorkService.EventService.GetEntityDtoByIdAsync(id);
            if(userName!= userEvent.data.CreatedBy)
            {
                return Unauthorized();
            }
            var data = await _initOfWorkService.EventService.UpdateAsync(id, input);
            return Ok(data);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(Guid id)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEvent = await _initOfWorkService.EventService.GetEntityDtoByIdAsync(id);
            if (userName != userEvent.data.CreatedBy)
            {
                return Unauthorized();
            }
            var data = await _initOfWorkService.EventService.DeleteByIdAsync(id);
            return Ok(data);

        }


        [HttpPost("GetList")]

        public async Task<ActionResult> GetList(ApiRequestFilter input)
        {

            var data = await _initOfWorkService.EventService.GetListAsync(input);
            return Ok(data);

        }


        [HttpGet("event-booking")]
        public async Task<ActionResult> eventBook(Guid eventid,int numOfTicket)
        {
            var userid = new Guid(User.FindFirst(ClaimTypes.Sid)?.Value);

            var data = await _initOfWorkService.EventService.Booking(userid, eventid, numOfTicket);
            return Ok(data);
        }

        [HttpGet("GetUserTickets")]
        public ActionResult GetUserTickets()
        {
            var userid = new Guid(User.FindFirst(ClaimTypes.Sid)?.Value);
            var data =  _initOfWorkService.EventService.GetUserTickets(userid);
            return Ok(data);
        }

        [HttpGet("CancelTicket")]
        public ActionResult CancelTicket(Guid eventid)
        {
            var userid = new Guid(User.FindFirst(ClaimTypes.Sid)?.Value);
            var data = _initOfWorkService.EventService.CancelTickets(userid,eventid);
            return Ok(data);
        }
    }
}
