using Microsoft.AspNetCore.Identity;

namespace LibApp.Models;

public class ApplicationCustomerRole: IdentityUserRole<int> {
    public Customer Customer {get; set;}
    public ApplicationRole Role {get; set;}

}