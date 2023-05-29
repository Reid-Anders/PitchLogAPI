﻿using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public interface IGradesRepository : IRepository<Grade>
    {
        public Task<PagedList<Grade>> GetCollection(BaseResourceParameters parameters, GradeType type);
    }
}