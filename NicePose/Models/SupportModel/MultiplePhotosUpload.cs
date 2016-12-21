using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePose.Models.SupportModel
{
    public class ManyPhotos
    {
        public IEnumerable<HttpPostedFileBase> lst { get; set; }

    }
}