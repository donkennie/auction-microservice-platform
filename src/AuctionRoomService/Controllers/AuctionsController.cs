using AuctionRoomService.DTOs;
using AuctionRoomService.Features.Commands;
using AuctionRoomService.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionRoomService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuctionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all auctions.
        /// </summary>
        /// <returns>A list of auction DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDTO>>> GetAuctions([FromQuery] GetAuctionsQuery query)
        {
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }

        /// <summary>
        /// Retrieves a specific auction by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the auction.</param>
        /// <returns>An auction DTO.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
        {
            var result = await _mediator.Send(new GetAuctionsQuery{Id = id});
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new auction.
        /// </summary>
        /// <param name="command">The auction creation command.</param>
        /// <returns>The created auction DTO.</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuctionDTO>> CreateAuction([FromBody] CreateAuctionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAuctionById), new { id = result.Data }, result);
        }

        /// <summary>
        /// Updates an existing auction.
        /// </summary>
        /// <param name="id">The unique identifier of the auction to update.</param>
        /// <param name="command">The auction update command.</param>
        /// <returns>A response indicating the outcome of the operation.</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuction(Guid id, [FromBody] UpdateAuctionItemsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            command.Id = id;
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes an auction.
        /// </summary>
        /// <param name="id">The unique identifier of the auction to delete.</param>
        /// <returns>A response indicating the outcome of the operation.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(Guid id)
        {
            string currentUser = User?.Identity?.Name;
            var result = await _mediator.Send(new DeleteAuctionCommand { Id = id, UserId = currentUser });

            /*if (!result.IsSuccessful)
            {
                var auction = await _mediator.Send(new GetAuctionQuery.Query(id));
                if (auction == null)
                {
                    return NotFound();
                }

                return Forbid();
            }*/

            return NoContent();
        }
    }
}
