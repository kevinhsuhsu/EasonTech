using Eason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.ViewModels
{
    public class ProjectItemViewModel
    {
        public ProjectItem ProjectItem { get; set; }
        public ProjectItemSearchModel SearchParameter { get; set; }
    }
}