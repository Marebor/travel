using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Travel.Common.Auth;
using Travel.Common.Cqrs;
using Travel.Domain.Travel.Commands;
using Travel.ReadModel.Queries;

namespace Travel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TravelsController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryExecutor queryExecutor;

        public TravelsController(ICommandDispatcher commandDispatcher, IQueryExecutor queryExecutor)
        {
            this.commandDispatcher = commandDispatcher;
            this.queryExecutor = queryExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTravels(int? skip, int? take)
        {
            string user = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            IEnumerable<ReadModel.Models.Travel> travels = 
                await queryExecutor.Execute<UserTravels, IEnumerable<ReadModel.Models.Travel>>(new UserTravels
                {
                    User = user,
                    Skip = skip,
                    Take = take,
                });

            return Ok(travels);
        }

        [HttpGet("all-users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllUsersTravels(int? skip, int? take)
        {
            IEnumerable<ReadModel.Models.Travel> travels =
                await queryExecutor.Execute<AllTravels, IEnumerable<ReadModel.Models.Travel>>(new AllTravels
                {
                    Skip = skip,
                    Take = take,
                });

            return Ok(travels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTravel(CreateTravel command)
        {
            command.Id = Guid.NewGuid();
            command.Owner = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            await commandDispatcher.Dispatch(command);

            return Accepted(command.Id);
        }

        [HttpPut]
        public async Task<IActionResult> EditTravel(EditTravel command)
        {
            command.Requester = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            command.RequesterRole = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            await commandDispatcher.Dispatch(command);

            return Accepted(command.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTravel(DeleteTravel command)
        {
            command.Requester = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            command.RequesterRole = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            await commandDispatcher.Dispatch(command);

            return Accepted(command.Id);
        }
    }
}