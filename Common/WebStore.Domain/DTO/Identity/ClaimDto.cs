using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity;

public abstract class ClaimDto : UserDto
{
    public IEnumerable<Claim> Claims { get; set; }
}

public class AddClaimDto : ClaimDto
{
}
public class RemoveClaimDto : ClaimDto
{
}
public class ReplaceClaimDto : UserDto
{
    public Claim OldClaim { get; set; }
    public Claim NewClaim { get; set; }
}