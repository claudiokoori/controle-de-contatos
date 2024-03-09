﻿using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContatoModel> Contatos { get; set;}
        public DbSet<UsuarioModel> Usuarios { get; set;}
    }
}
