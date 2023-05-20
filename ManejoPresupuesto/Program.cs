using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace MAnejoPresupuesto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        /*Creamos un sistema de autenticacion de usuarios para que si no está logueado, 
         * se redirija donde queramos */
            var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        //--------------------------------------------------------------------------------

            // Add services to the container.


            builder.Services.AddControllersWithViews(opciones =>
            {
                opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
            });

            builder.Services.AddTransient<IRepositorioTiposCuentas, RepositorioTiposCuentas>();
            builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
            builder.Services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
            builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
            builder.Services.AddTransient<IRepositorioTransacciones, RepositorioTransacciones>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IServicioReportes, ServicioReportes>();
            builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
            builder.Services.AddTransient<SignInManager<Usuario>>();
            builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
            builder.Services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.Password.RequireUppercase= false;
            }).AddErrorDescriber<MensajesDeErrorIdentity>();

            //USO DE COOKIES PARA AUTENTICACION
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/usuarios/login";
            });

            


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); //Para utilizar con las cookies de autenticacion
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Transacciones}/{action=Index}/{id?}");

            app.Run();
        }
    }
}