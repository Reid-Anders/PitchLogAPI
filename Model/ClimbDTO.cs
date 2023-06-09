﻿using System.Text.Json.Serialization;

namespace PitchLogAPI.Model
{
    public abstract class ClimbDTO : BaseDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public GradeDTO Grade { get; set; }

        public int SectorID { get; set; }

        public bool ShouldSerializeSectorID()
        {
            return false;
        }
    }
}
