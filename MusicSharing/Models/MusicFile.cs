using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSharing.Models
{
    public class MusicFile
    {

        public int musicFileId { get; set; }
        public string musicFileName { get; set; }
        public string musicFileUrl { get; set; }
        public string size { get; set; }
        public DateTime addedDate { get; set; }


    }

}