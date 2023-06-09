﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogData;
using System.ComponentModel.DataAnnotations;

namespace PitchLogAPI.Services
{
    public abstract class BaseService
    {
        protected readonly PitchLogContext _context;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly ProblemDetailsFactory _problemDetailsFactory;

        public BaseService(PitchLogContext context,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }

        protected ProblemDetails AreaNotFound(int ID)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                _contextAccessor.HttpContext,
                statusCode: 404,
                detail: $"Area with id {ID} not found. Please ensure you have the correct ID",
                instance: _contextAccessor.HttpContext.Request.GetAbsoluteUri());
        }

        protected ProblemDetails SectorNotFound(int areaID, int ID)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                _contextAccessor.HttpContext,
                statusCode: 404,
                detail: $"Sector with id {ID} not found for area with id {areaID}. Please ensure you have the correct ids",
                instance: _contextAccessor.HttpContext.Request.GetAbsoluteUri());
        }

        protected ProblemDetails RouteNotFound(int areaID, int sectorID, int ID)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                _contextAccessor.HttpContext,
                statusCode: 404,
                detail: $"Route with id {ID} not found for sector with id {sectorID} in area with id {areaID}. Please ensure you have the correct ids",
                instance: _contextAccessor.HttpContext.Request.GetAbsoluteUri());
        }
    }
}
