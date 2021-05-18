using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Suggestion
    {
        public Guid ID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}
