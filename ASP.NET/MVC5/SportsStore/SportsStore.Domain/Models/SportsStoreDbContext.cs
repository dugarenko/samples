using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SportsStore.Domain.Models
{
    /// <summary>
    /// Interfejs kontekstu modelu danych.
    /// </summary>
    public interface ISportsStoreDbContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<Product> Products { get; set; }

        /// <summary>
        /// Creates a Database instance for this context that allows for creation/deletion/existence checks for the underlying database.
        /// </summary>
        Database Database { get; }
        /// <summary>
        /// Provides access to features of the context that deal with change tracking of entities.
        /// </summary>
        DbChangeTracker ChangeTracker { get; }
        /// <summary>
        /// Provides access to configuration options for the context.
        /// </summary>
        DbContextConfiguration Configuration { get; }

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry object for the given entity
        /// providing access to information about the entity and the ability to perform actions
        /// on the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry Entry(object entity);  

        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry`1 object for the given
        /// entity providing access to information about the entity and the ability to perform
        /// actions on the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
       
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Type GetType();  

        /// <summary>
        /// Validates tracked entities and returns a Collection of System.Data.Entity.Validation.DbEntityValidationResult containing validation results.
        /// </summary>
        /// <returns>Collection of validation results for invalid entities. The collection is never
        /// null and must not contain null values or results for valid entities.</returns>
        /// <remarks>1. This method calls DetectChanges() to determine states of the tracked entities
        /// unless DbContextConfiguration.AutoDetectChangesEnabled is set to false. 2. By
        /// default only Added on Modified entities are validated. The user is able to change
        /// this behavior by overriding ShouldValidateEntity method.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        /// An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates
        /// an optimistic concurrency violation; that is, a row has been changed in the database
        /// since it was queried.
        /// </exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous
        /// commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The context or connection have been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before
        /// or after sending commands to the database.
        /// </exception>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains
        /// the number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the context has been disposed.
        /// </exception>
        /// <remarks>Multiple active operations on the same context instance are not supported. Use
        /// 'await' to ensure that any asynchronous operations have completed before calling
        /// another method on this context.</remarks>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result contains
        /// the number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are
        /// created for many-to-many relationships and relationships where there is no foreign
        /// key property included in the entity class (often referred to as independent associations).</returns>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        /// An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates
        /// an optimistic concurrency violation; that is, a row has been changed in the database
        /// since it was queried.
        /// </exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous
        /// commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The context or connection have been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before
        /// or after sending commands to the database.
        /// </exception>
        /// <remarks>Multiple active operations on the same context instance are not supported. Use
        /// 'await' to ensure that any asynchronous operations have completed before calling
        /// another method on this context.</remarks>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Returns a non-generic System.Data.Entity.DbSet instance for access to entities
        /// of the given type in the context and the underlying store.
        /// </summary>
        /// <param name="entityType">The type of entity for which a set should be returned.</param>
        /// <returns>A set for the given entity type.</returns>
        /// <remarks>Note that Entity Framework requires that this method return the same instance
        /// each time that it is called for a given context instance and entity type. Also,
        /// the generic System.Data.Entity.DbSet`1 returned by the System.Data.Entity.DbContext.Set(System.Type)
        /// method must wrap the same underlying query and set of entities. These invariants
        /// must be maintained if this method is overridden for anything other than creating
        /// test doubles for unit testing. See the System.Data.Entity.DbSet class for more
        /// details.</remarks>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set")]
        DbSet Set(Type entityType);

        /// <summary>
        /// Returns a System.Data.Entity.DbSet`1 instance for access to entities of the given
        /// type in the context and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        /// <remarks>Note that Entity Framework requires that this method return the same instance
        /// each time that it is called for a given context instance and entity type. Also,
        /// the non-generic System.Data.Entity.DbSet returned by the System.Data.Entity.DbContext.Set(System.Type)
        /// method must wrap the same underlying query and set of entities. These invariants
        /// must be maintained if this method is overridden for anything other than creating
        /// test doubles for unit testing. See the System.Data.Entity.DbSet`1 class for more
        /// details.</remarks>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set")]
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        ///// <summary>
        ///// Disposes the context. The underlying System.Data.Entity.Core.Objects.ObjectContext
        ///// is also disposed if it was created is by this context or ownership was passed
        ///// to this context when this context was created. The connection to the database
        ///// (System.Data.Common.DbConnection object) is also disposed if it was created is
        ///// by this context or ownership was passed to this context when this context was
        ///// created.
        ///// </summary>
        ///// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        //void Dispose(bool disposing);

        ///// <summary>
        ///// This method is called when the model for a derived context has been initialized,
        ///// but before the model has been locked down and used to initialize the context.
        ///// The default implementation of this method does nothing, but it can be overridden
        ///// in a derived class such that the model can be further configured before it is
        ///// locked down.
        ///// </summary>
        ///// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        ///// <remarks>Typically, this method is called only once when the first instance of a derived
        ///// context is created. The model for that context is then cached and is for all
        ///// further instances of the context in the app domain. This caching can be disabled
        ///// by setting the ModelCaching property on the given ModelBuidler, but note that
        ///// this can seriously degrade performance. More control over caching is provided
        ///// through use of the DbModelBuilder and DbContextFactory classes directly.</remarks>
        //void OnModelCreating(DbModelBuilder modelBuilder);

        ///// <summary>
        ///// Extension point allowing the user to override the default behavior of validating only added and modified entities.
        ///// </summary>
        ///// <param name="entityEntry">DbEntityEntry instance that is supposed to be validated.</param>
        ///// <returns>true to proceed with validation; false otherwise.</returns>
        //bool ShouldValidateEntity(DbEntityEntry entityEntry);

        ///// <summary>
        ///// Extension point allowing the user to customize validation of an entity or filter
        ///// out validation results. Called by System.Data.Entity.DbContext.GetValidationErrors.
        ///// </summary>
        ///// <param name="entityEntry">DbEntityEntry instance to be validated.</param>
        ///// <param name="items">User-defined dictionary containing additional info for custom validation. It
        ///// will be passed to System.ComponentModel.DataAnnotations.ValidationContext and
        ///// will be exposed as System.ComponentModel.DataAnnotations.ValidationContext.Items.
        ///// This parameter is optional and can be null.</param>
        ///// <returns>Entity validation result. Possibly null when overridden.</returns>
        //DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items);
    }

    /// <summary>
    /// Reprezentuje kontekst modelu danych.
    /// </summary>
    public partial class SportsStoreDbContext : ISportsStoreDbContext
    {
    }
}
