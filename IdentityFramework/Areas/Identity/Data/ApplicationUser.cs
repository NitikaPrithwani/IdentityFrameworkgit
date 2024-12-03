using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityFramework.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityFramework.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public ICollection<BorrowingRecords> BorrowingRecords { get; set; }
}

