using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FINNINGWEB.Entities
{
    public partial class AdventureWorksContext : DbContext
    {
        public virtual DbSet<AccionCorrectiva> AccionCorrectiva { get; set; }
        public virtual DbSet<AccionInmediata> AccionInmediata { get; set; }
        public virtual DbSet<AccionInmediataPersona> AccionInmediataPersona { get; set; }
        public virtual DbSet<Analisis> Analisis { get; set; }
        public virtual DbSet<AnalisisCausaRaiz> AnalisisCausaRaiz { get; set; }
        public virtual DbSet<AnalisisPersona> AnalisisPersona { get; set; }
        public virtual DbSet<Archivo> Archivo { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Causa> Causa { get; set; }
        public virtual DbSet<Causa1> Causa1 { get; set; }
        public virtual DbSet<Causa2> Causa2 { get; set; }
        public virtual DbSet<Causa3> Causa3 { get; set; }
        public virtual DbSet<Causa3Causa2> Causa3Causa2 { get; set; }
        public virtual DbSet<Causa4> Causa4 { get; set; }
        public virtual DbSet<Causa5> Causa5 { get; set; }
        public virtual DbSet<CausaIntermedia1> CausaIntermedia1 { get; set; }
        public virtual DbSet<CausaIntermedia2> CausaIntermedia2 { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Componente> Componente { get; set; }
        public virtual DbSet<ComponenteParte> ComponenteParte { get; set; }
        public virtual DbSet<Equipo> Equipo { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Evaluacion> Evaluacion { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<FallaPrimaria> FallaPrimaria { get; set; }
        public virtual DbSet<FallaSecundaria> FallaSecundaria { get; set; }
        public virtual DbSet<Mensajes> Mensajes { get; set; }
        public virtual DbSet<Modelo> Modelo { get; set; }
        public virtual DbSet<ModeloComponente> ModeloComponente { get; set; }
        public virtual DbSet<OrigenFalla> OrigenFalla { get; set; }
        public virtual DbSet<Parte> Parte { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PersonaArea> PersonaArea { get; set; }
        public virtual DbSet<PersonaEvento> PersonaEvento { get; set; }
        public virtual DbSet<PersonaEventoInvolucrado> PersonaEventoInvolucrado { get; set; }
        public virtual DbSet<PersonaSubArea> PersonaSubArea { get; set; }
        public virtual DbSet<Proceso1> Proceso1 { get; set; }
        public virtual DbSet<Proceso2> Proceso2 { get; set; }
        public virtual DbSet<Proceso3> Proceso3 { get; set; }
        public virtual DbSet<Proceso4> Proceso4 { get; set; }
        public virtual DbSet<Proceso5> Proceso5 { get; set; }
        public virtual DbSet<RespuestaCliente> RespuestaCliente { get; set; }
        public virtual DbSet<SubArea> SubArea { get; set; }
        public virtual DbSet<TipoEvento> TipoEvento { get; set; }
        public virtual DbSet<TipoFalla> TipoFalla { get; set; }
        public virtual DbSet<Verificacion> Verificacion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Test;Integrated Security=False;User ID=finning;Password=finning2017;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccionCorrectiva>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.AccionCorrectiva1)
                    .HasColumnName("AccionCorrectiva")
                    .HasColumnType("nchar(500)");

                entity.Property(e => e.Causa).HasColumnType("nchar(200)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.EventoId).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaEjecucion).HasColumnType("datetime");

                entity.Property(e => e.FechaLimite).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.AccionCorrectiva)
                    .HasForeignKey(d => d.EventoId)
                    .HasConstraintName("FK__AccionCor__Event__529933DA");

                entity.HasOne(d => d.RutPersonaNavigation)
                    .WithMany(p => p.AccionCorrectiva)
                    .HasForeignKey(d => d.RutPersona)
                    .HasConstraintName("FK__AccionCor__RutPe__538D5813");
            });

            modelBuilder.Entity<AccionInmediata>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.EventoId).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.AccionInmediata)
                    .HasForeignKey(d => d.EventoId)
                    .HasConstraintName("FK__AccionInm__Event__46335CF5");
            });

            modelBuilder.Entity<AccionInmediataPersona>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdAccionInmediata)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Analisis>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.AplicaRca).HasColumnName("AplicaRCA");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.EventoId).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.EventoId)
                    .HasConstraintName("FK__Analisis__Evento__4727812E");
            });

            modelBuilder.Entity<AnalisisCausaRaiz>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Creador).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaLimite).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnName("idEvento")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.AnalisisCausaRaiz)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__AnalisisC__idEve__4BEC364B");

                entity.HasOne(d => d.IdFallaPrimariaNavigation)
                    .WithMany(p => p.AnalisisCausaRaiz)
                    .HasForeignKey(d => d.IdFallaPrimaria)
                    .HasConstraintName("FK__AnalisisC__IdFal__4A03EDD9");

                entity.HasOne(d => d.IdFallaSecundariaNavigation)
                    .WithMany(p => p.AnalisisCausaRaiz)
                    .HasForeignKey(d => d.IdFallaSecundaria)
                    .HasConstraintName("FK__AnalisisC__IdFal__4AF81212");

                entity.HasOne(d => d.IdOrigenFallaNavigation)
                    .WithMany(p => p.AnalisisCausaRaiz)
                    .HasForeignKey(d => d.IdOrigenFalla)
                    .HasConstraintName("FK__AnalisisC__IdOri__490FC9A0");
            });

            modelBuilder.Entity<AnalisisPersona>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdAnalisis)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Archivo>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__Archivo__6F0F98BF080C1B80");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Identificador)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Identificador2).HasColumnType("nchar(50)");

                entity.Property(e => e.Nombre).HasColumnType("nchar(200)");

                entity.Property(e => e.Persona).HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.RutJefe).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RutPersona).HasColumnName("rutPersona");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Causa>(entity =>
            {
                entity.Property(e => e.Causa1)
                    .HasColumnName("Causa")
                    .HasColumnType("nchar(500)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdRca)
                    .HasColumnName("IdRCA")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");
            });

            modelBuilder.Entity<Causa1>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Causa2>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Causa3>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Causa3Causa2>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Causa4>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Causa5>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<CausaIntermedia1>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<CausaIntermedia2>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Componente>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.NumeroSerie).HasColumnType("nchar(50)");

                entity.Property(e => e.Posicion).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.SerieEquipo).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");
            });

            modelBuilder.Entity<Evaluacion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnName("idEvento")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.Evaluacion)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Evaluacio__idEve__5575A085");
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Categorizacion).HasColumnType("nchar(500)");

                entity.Property(e => e.Creador).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(2000)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Ncaceptada).HasColumnName("NCAceptada");

                entity.Property(e => e.NumeroParteComponente).HasColumnType("nchar(50)");

                entity.Property(e => e.PosicionComponente).HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.Responsable).HasColumnType("nchar(50)");

                entity.Property(e => e.RutJefeInvolucrado)
                    .HasColumnName("rutJefeInvolucrado")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.SerieComponente)
                    .HasColumnName("Serie Componente")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.SerieEquipo).HasColumnType("nchar(50)");

                entity.Property(e => e.TipoFalla).HasColumnType("nchar(50)");

                entity.Property(e => e.Woanterior)
                    .HasColumnName("WOAnterior")
                    .HasColumnType("nchar(100)");

                entity.Property(e => e.WorkOrder).HasColumnType("nchar(100)");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Cliente)
                    .HasConstraintName("FK__Evento__Cliente__3AC1AA49");

                entity.HasOne(d => d.ComponenteNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Componente)
                    .HasConstraintName("FK__Evento__Componen__3D9E16F4");

                entity.HasOne(d => d.EquipoNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Equipo)
                    .HasConstraintName("FK__Evento__Equipo__3BB5CE82");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Estado)
                    .HasConstraintName("FK__Evento__Estado__407A839F");

                entity.HasOne(d => d.ModeloNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Modelo)
                    .HasConstraintName("FK__Evento__Modelo__3CA9F2BB");

                entity.HasOne(d => d.ParteNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.Parte)
                    .HasConstraintName("FK__Evento__Parte__3E923B2D");

                entity.HasOne(d => d.RutJefeInvolucradoNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.RutJefeInvolucrado)
                    .HasConstraintName("FK__Evento__rutJefeI__4EC8A2F6");

                entity.HasOne(d => d.SubAreaNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.SubArea)
                    .HasConstraintName("FK__Evento__SubArea__444B1483");

                entity.HasOne(d => d.TipoEventoNavigation)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.TipoEvento)
                    .HasConstraintName("FK__Evento__TipoEven__4356F04A");
            });

            modelBuilder.Entity<FallaPrimaria>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<FallaSecundaria>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Mensajes>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnName("idEvento")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<OrigenFalla>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");
            });

            modelBuilder.Entity<Parte>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.NumeroParte).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Rut)
                    .HasName("PK__Persona__C2B74E7753C8F467");

                entity.Property(e => e.Rut)
                    .HasColumnName("rut")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.ApellidoMaterno).HasColumnType("nchar(50)");

                entity.Property(e => e.ApellidoPaterno).HasColumnType("nchar(50)");

                entity.Property(e => e.Cargo).HasColumnType("nchar(100)");

                entity.Property(e => e.Email).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.IdArea).HasColumnName("idArea");

                entity.Property(e => e.IdSubArea).HasColumnName("idSubArea");

                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");

                entity.Property(e => e.RutJefe)
                    .HasColumnName("rutJefe")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.RutSupervisor).HasColumnType("nchar(50)");

                entity.Property(e => e.Sexo).HasColumnType("nchar(15)");
            });

            modelBuilder.Entity<PersonaArea>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.PersonaArea)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK__PersonaAr__IdAre__58520D30");

                entity.HasOne(d => d.RutPersonaNavigation)
                    .WithMany(p => p.PersonaArea)
                    .HasForeignKey(d => d.RutPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaAr__RutPe__575DE8F7");
            });

            modelBuilder.Entity<PersonaEvento>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdAccionCorrectiva).HasColumnType("nchar(50)");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.PersonaEvento)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaEv__IdEve__50B0EB68");

                entity.HasOne(d => d.RutPersonaNavigation)
                    .WithMany(p => p.PersonaEvento)
                    .HasForeignKey(d => d.RutPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaEv__RutPe__51A50FA1");
            });

            modelBuilder.Entity<PersonaEventoInvolucrado>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.PersonaEventoInvolucrado)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaEv__IdEve__4CE05A84");

                entity.HasOne(d => d.RutPersonaNavigation)
                    .WithMany(p => p.PersonaEventoInvolucrado)
                    .HasForeignKey(d => d.RutPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaEv__RutPe__4DD47EBD");
            });

            modelBuilder.Entity<PersonaSubArea>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdSubAreaNavigation)
                    .WithMany(p => p.PersonaSubArea)
                    .HasForeignKey(d => d.IdSubArea)
                    .HasConstraintName("FK__PersonaSu__IdSub__5A3A55A2");

                entity.HasOne(d => d.RutPersonaNavigation)
                    .WithMany(p => p.PersonaSubArea)
                    .HasForeignKey(d => d.RutPersona)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PersonaSu__RutPe__59463169");
            });

            modelBuilder.Entity<Proceso1>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Proceso2>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Proceso3>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Proceso4>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Proceso5>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<RespuestaCliente>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(2000)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnName("idEvento")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");
            });

            modelBuilder.Entity<SubArea>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<TipoEvento>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("nchar(100)");

                entity.Property(e => e.Tipo).HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<TipoFalla>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");
            });

            modelBuilder.Entity<Verificacion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("nchar(50)");

                entity.Property(e => e.Descripcion).HasColumnType("nchar(500)");

                entity.Property(e => e.Estado).HasColumnType("nchar(50)");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdEvento)
                    .IsRequired()
                    .HasColumnName("idEvento")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.RutPersona).HasColumnType("nchar(50)");

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.Verificacion)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Verificac__idEve__5669C4BE");
            });
        }
    }
}