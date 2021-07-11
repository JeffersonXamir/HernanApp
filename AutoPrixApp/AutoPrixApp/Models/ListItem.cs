using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class ListItem
    {
        public Int64 Id { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public string CheckboxImage { get; set; }
        public bool isVisibleDetail { get; set; }
        public  string CVC { get; set; }
    }
}
