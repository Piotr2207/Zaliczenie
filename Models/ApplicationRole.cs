using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LibApp.Models;

public class ApplicationRole : IdentityRole<int>  {
    public ICollection<ApplicationCustomerRole> CustomerRoles {get; set;}
}