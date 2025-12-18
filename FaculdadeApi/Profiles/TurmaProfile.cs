using AutoMapper;
using FaculdadeApi.Dtos.TurmaDtos;
using FaculdadeApi.Models;

namespace FaculdadeApi.Profiles;

public class TurmaProfile : Profile
{
    public TurmaProfile()
    {
        CreateMap<Turma, ReadTurmaDto>();
    }
}
