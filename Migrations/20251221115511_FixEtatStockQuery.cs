using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BrasilBurger.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixEtatStockQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Correction : On laisse la méthode Up vide car les tables existent déjà dans PostgreSQL.
            // Cela évite l'erreur "42P07: relation burger already exists".
            // EF Core enregistrera simplement que cette migration a été passée.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // On laisse également vide pour éviter de supprimer des tables existantes 
            // par erreur lors d'un rollback.
        }
    }
}