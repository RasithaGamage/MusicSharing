namespace MusicSharing.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Music_File
    {
        public int Music_FileId { get; set; }
        public string Music_File_name { get; set; }
        public string Music_File_url { get; set; }

    }

}