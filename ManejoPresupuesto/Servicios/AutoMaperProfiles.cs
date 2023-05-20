using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMaperProfiles : Profile
    {

        public AutoMaperProfiles() 
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
            CreateMap<TransaccionActualizacionViewModel, Transaccion>().ReverseMap();
        }
    }
}
