using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Api.Common
{
    public class UserRolesResolver : IValueResolver<UserProfile, UserProfileResponseDto, List<string>>
    {
        private readonly UserManager<UserProfile> _userManager;

        public UserRolesResolver(UserManager<UserProfile> userManager)
        {
            _userManager = userManager;
        }

        public List<string> Resolve(UserProfile source, UserProfileResponseDto destination, List<string> destMember, ResolutionContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var roles = _userManager.GetRolesAsync(source).Result;
            return roles.ToList();
        }
    }
}