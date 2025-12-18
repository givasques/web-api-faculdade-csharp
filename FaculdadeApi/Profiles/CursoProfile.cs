using AutoMapper;
using FaculdadeApi.Dtos.CursoDtos;
using FaculdadeApi.Models;

namespace FaculdadeApi.Profiles;

public class CursoProfile : Profile
{
    public CursoProfile()
    {
        CreateMap<Curso, ReadCursoDto>();
    }
}
