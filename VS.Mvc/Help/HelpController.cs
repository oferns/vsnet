using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Mvc._Extensions;

namespace VS.Mvc.Help {

    [Controller]
    public class HelpController {




        [NavLink("Help", "Help", 0)]
        public IActionResult Index() => new ViewResult { ViewName = "Help" };

    }
}
