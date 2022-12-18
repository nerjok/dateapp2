using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    public readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
      _mapper = mapper;
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
        var users = await _userRepository.GetMembersAsync();

        // var usersToReturn  = _mapper.Map<IEnumerable<MemberDto>>(users);

        return Ok(users);
    }

    // [Authorize]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username) {
        // var user = await _userRepository.GetUserByNameAsync(username);

        // return _mapper.Map<MemberDto>(user);

        return await _userRepository.GetMemberAsync(username);
    }

    // [Authorize]
    // [HttpGet("{id}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id) {
    //     return await context.Users.FindAsync(id);
    // }
  }
}