global using BreadcrumbsAPI.Interfaces;
global using BreadcrumbsAPI.Models;
global using BreadcrumbsAPI.Models.Dtos;
global using BreadcrumbsAPI.Repositories;
global using BreadcrumbsAPI.Constants;
global using BreadcrumbsAPI.Services;
global using BreadcrumbsAPI.Applications.Commands;
global using BreadcrumbsAPI.Applications.Queries;
global using BreadcrumbsAPI.Data;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;

global using System.IdentityModel.Tokens.Jwt;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Data;
global using System.Security.Claims;
global using System.Text;

global using MediatR;
global using Mapster;
global using Ardalis.GuardClauses;