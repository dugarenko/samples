using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pharmacy.UnitTest.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pharmacy.UnitTest.Extensions
{
    /// <summary>
    /// Rozszerzenie klasy ModelBuilder.
    /// </summary>
    public static class ModelBuilderEx
    {
        private static IEnumerable<UniqueIndexAttribute> GetUniqueIndexAttributes(IMutableEntityType entityType, IMutableProperty property)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            else if (entityType.ClrType == null)
            {
                throw new ArgumentNullException(nameof(entityType.ClrType));
            }
            else if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            else if (property.Name == null)
            {
                throw new ArgumentNullException(nameof(property.Name));
            }
            var propInfo = entityType.ClrType.GetProperty(property.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (propInfo == null)
            {
                return null;
            }
            return propInfo.GetCustomAttributes<UniqueIndexAttribute>();
        }

        /// <summary>
        /// Konwertuje atrybuty [UniqueIndex] na indeksy w bazie danych.
        /// </summary>
        /// <param name="entityTypes">Typy encji zdefiniowane w modelu.</param>
        public static void SqliteAddIndexes(IEnumerable<IMutableEntityType> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var properties = entityType.GetProperties();
                if ((properties != null) && (properties.Any()))
                {
                    foreach (var property in properties)
                    {
                        var uniqueIndexes = GetUniqueIndexAttributes(entityType, property);
                        if (uniqueIndexes != null)
                        {
                            foreach (var uniqueIndex in uniqueIndexes.Where(x => x.Order == 0))
                            {
                                if (string.IsNullOrWhiteSpace(uniqueIndex.GroupName))
                                {
                                    // ===================================
                                    // Unikalny indeks dla jednej kolumny.
                                    // ===================================

                                    if (entityType.FindIndex(property) == null)
                                    {
                                        entityType.AddIndex(property).IsUnique = true;
                                    }
                                }
                                else
                                {
                                    // =================================
                                    // Unikalny indeks dla wielu kolumn.
                                    // =================================

                                    var mutableProperties = new List<IMutableProperty>();
                                    properties.ToList().ForEach(x =>
                                    {
                                        var uixs = GetUniqueIndexAttributes(entityType, x);
                                        if (uixs != null)
                                        {
                                            foreach (var uix in uixs)
                                            {
                                                if ((uix != null) && (uix.GroupName == uniqueIndex.GroupName))
                                                {
                                                    mutableProperties.Add(x);
                                                }
                                            }
                                        }
                                    });
                                    entityType.AddIndex(mutableProperties).IsUnique = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Konwertuje atrybuty [UniqueIndex] na indeksy w bazie danych.
        /// </summary>
        /// <param name="modelBuilder">Obiekt ModelBuilder.</param>
        public static void AddIndexes(this ModelBuilder modelBuilder)
        {
            SqliteAddIndexes(modelBuilder.Model.GetEntityTypes());
        }
    }
}
