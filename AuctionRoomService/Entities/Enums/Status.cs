using System.ComponentModel;

namespace AuctionRoomService.Entities.Enums
{
    public enum Status
    {
        [Description("Live")]
        Live = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Active")]
        Active = 3,
        [Description("Finished")]
        Finished = 4,
        [Description("NotActive")]
        NotActive = 5,
        [Description("ReserveNotMet")]
        ReserveNotMet = 6
    }
}
