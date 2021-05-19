using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class PlacementCreateEditViewModel
    {
        public Int32 PatientId { get; set; }
        public Int32 WardId { get; set; }
        public Int32 Bed { get; set; }
    }
}
