using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TechShop.Application.Services.UserService;
using TechShop.Domain.DTOs.UserDto;

public class Benchmark(IUserService _userService, IMapper _mapper)
{
    [Benchmark]
    public List<ApplicationUserDto> BenchmarkGetUsers()
    {
        var users = _userService.GetUsers();
        var response = _mapper.Map<List<ApplicationUserDto>>(users);

        return response;
    }

    [Benchmark]
    public List<ApplicationUserDto> BenchmarkGetUsersSplit()
    {
        var users = _userService.GetUsersSplit();
        var response = _mapper.Map<List<ApplicationUserDto>>(users);

        return response;
    }
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}
