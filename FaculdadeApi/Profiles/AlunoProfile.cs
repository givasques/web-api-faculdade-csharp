using AutoMapper;
using FaculdadeApi.Dtos.AlunoDtos;
using FaculdadeApi.Models;

namespace FaculdadeApi.Profiles;

public class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<Aluno, ReadAlunoDto>();
    }
}
