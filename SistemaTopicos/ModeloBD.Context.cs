﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaTopicos
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SistemaTopicosEntities : DbContext
    {
        public SistemaTopicosEntities()
            : base("name=SistemaTopicosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Piezas> Piezas { get; set; }
        public DbSet<TiposTablilla> TiposTablilla { get; set; }
    }
}
