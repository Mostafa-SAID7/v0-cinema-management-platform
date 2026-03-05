using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserRequest> _registerValidator;
        private readonly IValidator<UpdateUserProfileRequest> _updateValidator;

        public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<RegisterUserRequest> registerValidator,
            IValidator<UpdateUserProfileRequest> updateValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _registerValidator = registerValidator;
            _updateValidator = updateValidator;
        }


        // GET: api/users
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<UserSummaryResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<UserSummaryResponse>>>> Get()
        {
            var users = await _userRepository.GetUsersAsync();
            var response = _mapper.Map<List<UserSummaryResponse>>(users);
            return Ok(BaseResponse<List<UserSummaryResponse>>.Success(response));
        }

        // GET api/users/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> Get(long id)
        {
            var user = await _userRepository.GetUserAsync(id);

            if (user == null)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var response = _mapper.Map<UserResponse>(user);
            return Ok(BaseResponse<UserResponse>.Success(response));
        }

        // POST api/users
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> Post([FromBody] RegisterUserRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var registerRequest = _mapper.Map<RegisterRequest>(request);
            var id = await _userRepository.CreateUserAsync(registerRequest);
            
            return CreatedAtAction(nameof(Get), new { id }, BaseResponse<long>.Success(id, "User created successfully"));
        }

        // PUT api/users
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<bool>>> Put([FromBody] UpdateUserProfileRequest request)
        {
            var existing = await _userRepository.GetUserByUsername(request.Username);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var userProfile = new UserProfile
            {
                Username = request.Username,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            var result = await _userRepository.UpdateUserAsync(userProfile);
            return Ok(BaseResponse<bool>.Success(result, "User updated successfully"));
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _userRepository.GetUserAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var result = await _userRepository.DeleteUserAsync(id);
            return Ok(BaseResponse<bool>.Success(result, "User deleted successfully"));
        }

        // PUT api/users/5/role
        [HttpPut("{id}/role")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var existing = await _userRepository.GetUserAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            if (string.IsNullOrWhiteSpace(request.Role))
                return BadRequest(BaseResponse<object>.Failure("Role is required"));

            if (request.Role != "User" && request.Role != "Admin")
                return BadRequest(BaseResponse<object>.Failure("Invalid role. Must be 'User' or 'Admin'"));

            var result = await _userRepository.UpdateUserRoleAsync(id, request.Role);
            
            if (result > 0)
                return Ok(BaseResponse<object>.Success(null, "User role updated successfully"));

            return StatusCode(500, BaseResponse<object>.Failure("Failed to update user role"));
        }
    }

    public class UpdateRoleRequest
    {
        public string Role { get; set; }
    }
}
