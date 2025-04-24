using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    /// <summary>
    /// This abstract class has functionality that will create mock data sets and mock context.
    /// </summary>
    public abstract class DataStoreTestBase
    {

        /// <summary>
        /// Creates a mock db set for the provided <paramref name="data"/>.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="data">The data as <seealso cref="IQueryable{T}"/>.</param>
        /// <returns>The mock db set of of the entity <seealso cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="data"/> is null.
        /// </exception>
        public Mock<DbSet<T>> SetupMockDbSet<T>(IQueryable<T> data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);

            var mockDbSetSetupSequence = mockDbSet.As<IQueryable<T>>().SetupSequence(x => x.GetEnumerator())
                .Returns(data.GetEnumerator());

            for (var i = 0; i < 100; i++)
            {
                mockDbSetSetupSequence.Returns(data.GetEnumerator());
            }

            return mockDbSet;
        }

        /// <summary>
        /// Creates a mock db set for the provided <paramref name="data"/> and projected data
        /// <paramref name="projectionData"/>. This is used for async get calls, like FirstOrDefaultAsync
        /// and using projected objects.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <typeparam name="TProjection">The type of the projected data.</typeparam>
        /// <param name="data">The data as <seealso cref="IQueryable{T}"/>.</param>
        /// <param name="projectionData">The projected data as <seealso cref="IQueryable{T}"/>.</param>
        /// <returns>The mock db set of of the entity <seealso cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="data"/> is null
        /// or
        /// When <paramref name="projectionData"/> is null.
        /// </exception>
        public Mock<DbSet<T>> SetupMockDbSetWithProjectionAsync<T, TProjection>(IQueryable<T> data,
            IQueryable<TProjection> projectionData) where T : class where TProjection : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (projectionData == null)
            {
                throw new ArgumentNullException(nameof(projectionData));
            }

            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IAsyncEnumerable<TProjection>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<TProjection>(projectionData.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TProjection>(data.Provider));

            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);
            var mockDbSetSetupSequence = mockDbSet.As<IQueryable<T>>().SetupSequence(x => x.GetEnumerator())
                .Returns(data.GetEnumerator());

            for (var i = 0; i < 100; i++)
            {
                mockDbSetSetupSequence.Returns(data.GetEnumerator());
            }

            return mockDbSet;
        }

        /// <summary>
        /// Creates a mock db set for the provided <paramref name="data"/>. This is used for async get calls,
        /// like FirstOrDefaultAsync and pulling the full entity back.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="data">The data as <seealso cref="IQueryable{T}"/>.</param>
        /// <returns>The mock db set of of the entity <seealso cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="data"/> is null.
        /// </exception>
        public Mock<DbSet<T>> SetupMockDbSetAsync<T>(IQueryable<T> data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(data.Provider));

            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);
            var mockDbSetSetupSequence = mockDbSet.As<IQueryable<T>>().SetupSequence(x => x.GetEnumerator())
                .Returns(data.GetEnumerator());

            for (var i = 0; i < 100; i++)
            {
                mockDbSetSetupSequence.Returns(data.GetEnumerator());
            }

            return mockDbSet;
        }

        /// <summary>
        /// Creates a mock context
        /// </summary>
        /// <returns>The mock <seealso cref="XspocDbContext"/>.</returns>
        public Mock<NoLockXspocDbContext> SetupMockContext()
        {

            var contextOptions = new Mock<DbContextOptions<NoLockXspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(NoLockXspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var mockInterceptor = new Mock<IDbConnectionInterceptor>();
            var mockContext = new Mock<NoLockXspocDbContext>(contextOptions.Object, mockInterceptor.Object, mockDateTimeConverter.Object);

            return mockContext;
        }

    }
}
