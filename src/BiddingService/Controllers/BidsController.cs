using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Features.Commands;
using BiddingService.Features.Queries;
using KernelShared;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingService.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;


        public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, IMediator mediator)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }


        [Authorize, HttpPost("{auctionId}/bids")]
        public async Task<ActionResult<BidDTO>> PlaceBid(string auctionId, int amount)
        {
            var command = new BidCommand
            {
                AuctionId = auctionId,
                Amount = amount,
                Bidder = User.Identity.Name
            };

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest("Cannot place bid on this auction.");
            }

            await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(result));

            return Ok(result);
        }

        [HttpGet("{auctionId}/bids")]
        public async Task<ActionResult<List<APIResponse>>> GetBidsForAuction(string auctionId)
        {
            var result = await _mediator.Send(new GetBidsForAuctionQuery { AuctionId = auctionId });

            if (result == null || !result.Any())
            {
                return NotFound("No bids found for this auction.");
            }

            return Ok(result);
        }
    }
}
