global using BreadcrumbsAPI.Interfaces;
global using BreadcrumbsAPI.Models;
global using BreadcrumbsAPI.Models.Dtos;
global using BreadcrumbsAPI.Repositories;
global using BreadcrumbsAPI.Constants;
global using BreadcrumbsAPI.Services;
global using BreadcrumbsAPI.Applications.Crumbs.Commands;
global using BreadcrumbsAPI.Applications.Crumbs.Queries;
global using BreadcrumbsAPI.Applications.Groups.Commands;
global using BreadcrumbsAPI.Applications.Groups.Queries;
global using BreadcrumbsAPI.Applications.Users.Commands;
global using BreadcrumbsAPI.Applications.Users.Queries;
global using BreadcrumbsAPI.Applications.CodeValues.Queries;
global using BreadcrumbsAPI.Data;
global using BreadcrumbsAPI.Helpers;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Configuration;
global using Microsoft.OpenApi.Models;

global using System.IdentityModel.Tokens.Jwt;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Data;
global using System.Security.Claims;
global using System.Text;

global using MediatR;
global using Mapster;
global using Ardalis.GuardClauses;