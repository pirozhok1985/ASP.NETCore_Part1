using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity;

public abstract class ClaimDTO : UserDTO
{
    public IEnumerable<Claim> Claims { get; set; }
}

public class AddClaimDto : ClaimDTO { }

public class RemoveClaimDto : ClaimDTO { }

public class ReplaceClaimDto : UserDTO
{
    public Claim Claim { get; set; }

    public Claim NewClaim { get; set; }
}