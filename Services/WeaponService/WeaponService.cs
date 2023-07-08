using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _autoMapper;

    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper autoMapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _autoMapper = autoMapper;
    }

    // TODO: Create a separate service for all equipment-related actions (inventory, equipping, etc.)
    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterWeapon(AddCharacterWeaponDto newWeapon)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c =>
                    c.Id == newWeapon.CharacterId &&
                    c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (character == null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }
            else
            {
                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();
                response.Data = _autoMapper.Map<GetCharacterDto>(character);
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
